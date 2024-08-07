﻿using BadmintonBooking.Models;
using BadmintonBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BadmintonBooking.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DemobadmintonContext _context;
        private readonly IWebHostEnvironment environment;
        private readonly IMemoryCache _cache;

        public ManagerController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, DemobadmintonContext context, IWebHostEnvironment environment, IMemoryCache cache)
        {
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            this.environment = environment;
            _cache = cache;
        }
        public IActionResult Booking(int page = 1, string sortOrder = "")
        {
            string userId = _userManager.GetUserId(User);
            int NoOfRecordPerPage = 5;

            //ViewBag.SearchAddress = address;


            // Get court list based on group
            var data = _context.Courts.Where(c => c.UserId == userId).ToList();

            // Sort data

            ViewBag.SortOrder = sortOrder;
            switch (sortOrder)
            {
                case "name_asc":
                    data = data.OrderBy(c => c.CoName).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(c => c.CoName).ToList();
                    break;
                case "price_asc":
                    data = data.OrderBy(c => c.CoPrice).ToList();
                    break;
                case "price_desc":
                    data = data.OrderByDescending(c => c.CoPrice).ToList();
                    break;
                default:
                    data = data.OrderBy(c => c.CoId).ToList();
                    break;
            }
            //paging
            // Calculate total pages
            int totalRecords = data.Count;
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            //ViewBag properties
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            return View(pagedData);
        }
        public IActionResult Date(int CoId)
        {
            _httpContextAccessor.HttpContext.Session.SetString("CoId", CoId.ToString());
            return View();
        }
        [HttpGet]
        public IActionResult GetPrice()
        {
            int court = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId"));
            var price = _context.Courts.FirstOrDefault(x => x.CoId == court).CoPrice;
            return Ok(price);
        }
        [HttpGet]
        public async Task<IActionResult> GetBookSlots()
        {
            int court = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId"));
            var booked = await _context.TimeSlots.Where(x => x.CoId == court).ToListAsync();
            return Ok(booked);
        }
        [HttpPost]
        public async Task<IActionResult> Cancel()
        {
            _cache.Remove("BookingSlots");
            return Ok(new { message = "Cancel successfully" });
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingData bookingData)
        {
            try
            {
                if (bookingData == null)
                {
                    return BadRequest("Invalid booking data.");
                }

                TimeOnly time = TimeOnly.ParseExact(bookingData.Time, "h:mm tt", CultureInfo.InvariantCulture);
                DateOnly date = DateOnly.ParseExact(bookingData.Date, "MMM d", CultureInfo.InvariantCulture);
                int court = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId"));
                var existing = _context.TimeSlots.Where(x => x.TsStart == time && x.TsDate == date && x.CoId == court);
                if (existing.Any())
                {
                    return Ok(new { success = false });
                }

                TimeSlot slot = new TimeSlot()
                {
                    CoId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId")),
                    TsCheckedIn = false,
                    TsDate = date,
                    TsStart = time,
                    TsEnd = time.AddHours(1),
                };

                var slots = _cache.GetOrCreate("BookingSlots", entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return new List<TimeSlot>();
                });
                slots.Add(slot);
                _cache.Set("BookingSlots", slots);

                int quantity = slots.Count;
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing booking: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(string Guest)
        {
            var slots = _cache.Get<List<TimeSlot>>("BookingSlots") ?? new List<TimeSlot>();
            int CourtId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId"));
            foreach (var item in slots)
            {
                var exist = await _context.TimeSlots.FirstOrDefaultAsync(x => x.TsStart == item.TsStart && x.TsDate == item.TsDate && x.CoId == CourtId);
                if (exist != null)
                {
                    TempData["error"] = "One of the slots is booked!";
                    _cache.Remove("BookingSlots");
                    return RedirectToAction("Booking");
                }
            }
            var booking = new Booking()
            {
                BBookingType = "Casual",
                BGuestName = Guest,
                CoId = CourtId,
                UserId = _userManager.GetUserId(User),
                TimeSlots = slots
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            _cache.Remove("BookingSlots");

            TempData["Message"] = "Booked successfully!";
            return RedirectToAction("Booking", "Manager");
        }


        //----------------------------------------
        //crud court

        public IActionResult AddCourt()
        {
            var addressList = _context.Courts.Where(c => c.CoStatus == true)
       .Select(c => c.CoAddress)
       .Distinct()
       .ToList();
            ViewBag.AddressList = new SelectList(addressList);
            return View();
        }
        [HttpPost]
        public IActionResult AddCourt(Court model, string CoAddressTextBox)
        {

            if (model.CoAddress == null)
            {
                ModelState.Remove("CoAddress");
            }
            else
            {
                ModelState.Remove("CoAddressTextBox");
            }


            DemobadmintonContext context = new DemobadmintonContext();

            string uniqueFileName = UploadImage(model);
            string userid = _userManager.GetUserId(User);
            var data = new Court();

            data.CoName = model.CoName;
            if (model.CoAddress == null)
            {
                data.CoAddress = CoAddressTextBox;
            }
            else
            {
                data.CoAddress = model.CoAddress;
            }

            data.CoInfo = model.CoInfo;
            data.CoPrice = model.CoPrice;
            data.UserId = userid;
            data.CoStatus = true;
            data.CoPath = uniqueFileName;

            context.Courts.Add(data);
            context.SaveChanges();
            TempData["message"] = "Record has been saved successfully";


            return RedirectToAction("Booking", "Manager");

        }



        public IActionResult EditCourt(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var addressList = _context.Courts.Where(c => c.CoStatus == true)
      .Select(c => c.CoAddress)
      .Distinct()
      .ToList();

            ViewBag.AddressList = new SelectList(addressList);

            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data != null)
            {
                return View(data);
            }
            else
            {
                return RedirectToAction("Booking", "Manager");
            }





        }
        [HttpPost]
        public IActionResult EditCourt(Court model)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            string userid = _userManager.GetUserId(User);
            var data = context.Courts.FirstOrDefault(c => c.CoId == model.CoId);

            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                if (data.CoPath != null)
                {
                    string filepath = Path.Combine(environment.WebRootPath, "Upload/Image", data.CoPath);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);

                    }
                }
                uniqueFileName = UploadImage(model);
            }
            data.CoId = model.CoId;
            data.CoName = model.CoName;
            data.CoAddress = model.CoAddress;
            data.CoInfo = model.CoInfo;
            data.CoPrice = model.CoPrice;
            if (model.CoStatus == false)
            {
                data.CoStatus = false;
            }
            else
            {
                data.CoStatus = true;
            }

            data.UserId = userid;

            if (model.ImagePath != null)
            {
                data.CoPath = uniqueFileName;
            }
            context.Courts.Update(data);
            context.SaveChanges();
            TempData["message"] = "Record has been updated successfully";

            return RedirectToAction("Booking", "Manager");
        }
        public IActionResult DeleteCourt(int id, int page, string sortOrder)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            bool hasCurrentBooking = context.Bookings
                .Any(b => b.CoId == id && context.TimeSlots
                    .Any(ts => ts.BId == b.BId && ts.TsDate >= currentDate));

            if (hasCurrentBooking)
            {
                TempData["error"] = "Court cannot be deleted as it has bookings for today or future.";
                return RedirectToAction("Booking", new { page = page, sortOrder = sortOrder });
            }
            if (data == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                data.CoStatus = false;
                context.Courts.Update(data);
                context.SaveChanges();
                TempData["message"] = "Record has been deleted successfully";
            }
            return RedirectToAction("Booking", "Manager");
        }
        public IActionResult Activate(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                data.CoStatus = true;
                context.Courts.Update(data);
                context.SaveChanges();
                TempData["message"] = "Record has been activated successfully";
            }
            return RedirectToAction("Booking", "Manager");
        }


        private string UploadImage(Court model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Upload/Image/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }



            }
            return uniqueFileName;
        }

        public IActionResult Dashboard(int page = 1, DateTime? startDate = null, DateTime? endDate = null,string sortOrder="")
        {
            string userID = _userManager.GetUserId(User);
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            ViewBag.sortOrder = sortOrder;
            IQueryable<dynamic> resultsQuery;
            int NoOfRecordPerPage = 6;


            if (startDate.HasValue && endDate.HasValue)
            {
                // If startDate has a value, filter payments based on PDateTime
                var startDateOnly = DateOnly.FromDateTime(startDate.Value);
                var endDateOnly = DateOnly.FromDateTime(endDate.Value);

                resultsQuery = _context.Courts
                    .Select(c => new
                    {
                        CoId = c.CoId,
                        CoName = c.CoName,
                        TotalAmount = _context.Bookings
                            .Where(b => b.CoId == c.CoId)
                            .SelectMany(b => _context.Payments
                                .Where(p => p.BId == b.BId && c.UserId == userID &&
                                           (DateOnly.FromDateTime(p.PDateTime) >= startDateOnly && DateOnly.FromDateTime(p.PDateTime) <= endDateOnly)))
                            .Sum(p => (decimal?)p.PAmount) ?? 0,
                        PeopleBooked = _context.Bookings
                    .Where(b => b.CoId == c.CoId &&
                                _context.Payments.Any(p => p.BId == b.BId &&
                                                           DateOnly.FromDateTime(p.PDateTime) >= startDateOnly &&
                                                           DateOnly.FromDateTime(p.PDateTime) <= endDateOnly))
                    .Select(b => b.UserId)
                    .Distinct()
                    .Count()
                    })
                    .OrderBy(c => c.CoId);
            }
            else
            {

                // No date filter
                resultsQuery = _context.Courts
                    .Select(c => new
                    {
                        CoId = c.CoId,
                        CoName = c.CoName,
                        TotalAmount = _context.Bookings
                            .Where(b => b.CoId == c.CoId)
                            .SelectMany(b => _context.Payments
                                .Where(p => p.BId == b.BId && c.UserId == userID))
                            .Sum(p => (decimal?)p.PAmount) ?? 0,
                        PeopleBooked = _context.Bookings
                    .Where(b => b.CoId == c.CoId &&
                                _context.Payments.Any(p => p.BId == b.BId))
                    .Select(b => b.UserId)
                    .Distinct()
                    .Count()

                    })
                    .OrderBy(c => c.CoId);
            }

            
            // Execute the query and get results
            var results = resultsQuery.ToList();
            // Apply sorting in-memory
            switch (sortOrder)
            {
                case "income_asc":
                    results = results.OrderBy(c => (decimal)c.TotalAmount).ToList();
                    break;
                case "income_desc":
                    results = results.OrderByDescending(c => (decimal)c.TotalAmount).ToList();
                    break;
                default:
                    results = results.OrderBy(c => c.CoId).ToList();
                    break;
            }


            var totalIncome = results.Sum(r => (decimal)r.TotalAmount);
            ViewBag.TotalIncome = totalIncome;
            var monthlyRevenue = GetMonthlyRevenue();
            //ViewBag.test = monthlyRevenue;
            ViewBag.MonthlyRevenue = JsonConvert.SerializeObject(monthlyRevenue);
            // Map the results to the view model
            var viewModel = results.Select(r => new CourtDashboardViewModel
            {
                CoId = r.CoId,
                CoName = r.CoName,
                TotalAmount = r.TotalAmount,
                PeopleBooked = r.PeopleBooked,
                //MonthlyRevenue = monthlyRevenue.ContainsKey(r.CoId) ? monthlyRevenue[r.CoId] : new Dictionary<string, decimal>()
            });
            // Calculate total pages
            int totalRecords = viewModel.Count();
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = viewModel.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            //ViewBag properties
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            return View(pagedData);
        }
        private IDictionary<int, Dictionary<string, decimal>> GetMonthlyRevenue()
        {
            string userID = _userManager.GetUserId(User);
            var currentYear = DateTime.Now.Year;

            // Fetch all required data into memory
            var courts = _context.Courts.ToList();
            var bookings = _context.Bookings.ToList();
            var payments = _context.Payments
                .Where(p => p.PDateTime.Year == currentYear)
                .ToList();

            // Process the data in memory
            var courtRevenues = courts.ToDictionary(c => c.CoId, c =>
            {
                var relevantPayments = payments
                    .Where(p => bookings
                        .Any(b => b.BId == p.BId && b.CoId == c.CoId) &&
                        p.PDateTime.Year == currentYear &&
                        c.UserId == userID)
                    .GroupBy(p => p.PDateTime.Month)
                    .Select(g => new
                    {
                        MonthYear = $"{g.Key:D2}-{currentYear}", // Format as MM-YYYY
                        SumAmount = g.Sum(p => p.PAmount)
                    })
                    .ToDictionary(x => x.MonthYear, x => x.SumAmount);

                return relevantPayments;
            });

            return courtRevenues;
        }
    }
}
