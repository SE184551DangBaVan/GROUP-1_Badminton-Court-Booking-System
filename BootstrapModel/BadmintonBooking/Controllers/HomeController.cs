using BadmintonBooking.Models;
using BadmintonBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PayPal.Api;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BadmintonBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DemobadmintonContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, DemobadmintonContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public IActionResult Index(string txtSearch)
        {
            var courtlist = _context.Courts.Where(c => c.CoStatus == true).ToList();


            return View(courtlist);
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            return View();
        }

        public IActionResult Date(int? CoId, string Types, int? BId, int Remain, int Hours)
        {
            if (BId != null)
            {
                ViewData["Hours"] = Hours;
                ViewData["Remain"] = Remain;
                ViewData["BookingId"] = BId;
                Types = "Flexible";
            }
            if (CoId.HasValue && !string.IsNullOrEmpty(Types))
            {
                _httpContextAccessor.HttpContext.Session.SetString("CoId", CoId.ToString());
                _httpContextAccessor.HttpContext.Session.SetString("Types", Types);
            }
            ViewData["Types"] = Types;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Book(int id)
        {
            var data = _context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        public IActionResult Book2(int page = 1, string address = "", string sortOrder = "", TimeOnly? selectedTime = null)
        {
            int NoOfRecordPerPage = 5;

            ViewBag.SearchAddress = address;
            ViewBag.SelectedTime = selectedTime;

            // Get court list based on group
            var data = _context.Courts.Include(c => c.TimeSlots).Include(c => c.Bookings)
                               .Where(c => (string.IsNullOrEmpty(address) || c.CoAddress == address) && c.CoStatus==true)
                               .ToList();
            //Search
            if (selectedTime.HasValue)
            {
                // data = data.Where(c => c.TimeSlots.All(t => t.TsStart != search && t.TsDate == DateOnly.FromDateTime(DateTime.Today)).Any())
                //.ToList();
                //data = data.Where(c => c.TimeSlots.Any(t => t.TsStart!= search && t.TsDate == DateOnly.FromDateTime(DateTime.Today)))
                //.ToList();
                data = data.Where(c => !c.TimeSlots.Any(ts =>
                               ts.TsDate == DateOnly.FromDateTime(DateTime.Today) &&
                               ts.TsStart == selectedTime.Value))
                 .ToList();

            }

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
            string userId = _userManager.GetUserId(User);
            var booking = _context.Bookings.FirstOrDefault(c => c.UserId == userId && c.BTotalHours.HasValue && c.BTotalHours > 0);
            //missing court compare
            if (booking != null && userId != null)
            {
                var time = _context.Payments.FirstOrDefault(c => c.BId == booking.BId).PDateTime.AddDays(30);
                int remain = (time - DateTime.Now).Days;
                if(remain > 0)
                {
                    ViewData["Hours"] = booking.BTotalHours;
                    ViewData["CoId"] = booking.CoId;
                    ViewData["BookingId"] = booking.BId;
                    ViewData["Remain"] = remain;
                    ViewData["Message"] = "Your flexible booking are not yet over!";
                }               
            }
            return View(pagedData);
        }

        /*      public IActionResult UserBookCourt(int id)
              {
                  DemobadmintonContext context = new DemobadmintonContext();
                  var courtobject = context.Courts.FirstOrDefault(c=>c.CoId==id);
                  if (TempData.ContainsKey("UserId"))
                  {
                      string userId = TempData["UserId"].ToString();
                      ViewData["UserId"] = userId;
                      var BookingUserid = userId;
                      Court bookCourt = new Court()
                      {
                          CoName = courtobject.CoName,
                          CoPath = courtobject.CoPath,
                          CoStatus = courtobject.CoStatus,
                          CoAddress = courtobject.CoAddress,
                          CoInfo = courtobject.CoInfo,
                          CoPrice = courtobject.CoPrice,
                          UserId = BookingUserid
                      };
                      context.Courts.Add(bookCourt);
                      context.SaveChanges();


                  }
                  else
                  {
                      return RedirectToAction("ErrorBooking");

                  }
                  return RedirectToAction("Index");


              } */

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ErrorBooking()
        {
            return View();
        }
        //staff section
        [Authorize]
        [Authorize(Roles = "Staff")]
        public IActionResult Staff(int page=1, string searchTerm = "")
        {
            int NoOfRecordPerPage = 7;

            /* var data = _context.Courts
            .Where(c => string.IsNullOrEmpty(searchTerm) ||
                        c.CoId.ToString().Equals(searchTerm) ||
                        c.CoName.ToLower().Equals(searchTerm.ToLower()) ||

                        c.CoInfo.ToLower().Equals(searchTerm.ToLower()))
            .ToList();*/
            var allCourts = _context.Courts.ToList();

            List<Court> data;

            // First part: Search by CoId
            if (int.TryParse(searchTerm, out int searchId))
            {
                data = allCourts
                    .Where(c => c.CoId == searchId)
                    .ToList();
            }
            else
            {
                // Second part: Search by other fields
                data = allCourts
                    .Where(c =>
                        string.IsNullOrEmpty(searchTerm) ||
                        c.CoName.ToLower().Contains(searchTerm.ToLower()) ||
                        c.CoInfo.ToLower().Contains(searchTerm.ToLower())||
                        c.CoAddress.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
            }
            //pagination
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
           //ViewBag.SortOrder = sortOrder;
           ViewBag.SearchTerm = searchTerm;
            return View(pagedData);
        }
        [Authorize]
        [Authorize(Roles = "Staff")]
        public IActionResult CheckIn(int courtId, int page = 1, string searchTerm = "", string sortOrder = "")
        {
            int NoOfRecordPerPage = 6;
            searchTerm = searchTerm?.ToLower();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            //TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            // First part: filter by courtId
            var courtFilteredData = _context.Bookings
                .Where(b => b.CoId == courtId && b.TimeSlots.Any(ts => ts.TsDate == currentDate))
                .Include(b => b.TimeSlots)
                .Include(b => b.Co)
                .ToList();

            /*    // Second part: apply search filter
                var data = courtFilteredData
                    .Where(b => string.IsNullOrEmpty(searchTerm) ||
                                b.BId.ToString().ToLower().Equals(searchTerm) ||
                                b.Co.CoName.ToLower().Equals(searchTerm) ||
                                b.BBookingType.ToLower().Equals(searchTerm) ||
                                b.Co.CoAddress.ToLower().Equals(searchTerm)

                                )
                    .ToList(); */

            List<Booking> data;

            // Second part: apply search filter
            if (int.TryParse(searchTerm, out int searchId))
            {
                // Search by Booking ID
                data = courtFilteredData
                    .Where(b => b.BId == searchId)
                    .ToList();
            }
            else
            {
                // Search by other fields
                data = courtFilteredData
                    .Where(b => string.IsNullOrEmpty(searchTerm) ||
                                b.Co.CoName.ToLower().Contains(searchTerm) ||
                                b.BBookingType.ToLower().Contains(searchTerm) ||
                                b.Co.CoAddress.ToLower().Contains(searchTerm))
                    .ToList();
            }

            switch (sortOrder)
            {
                case "recent_Booking":
                    data = data.OrderByDescending(b => b.BId).ToList();
                    break;
                case "old_Booking":
                    data = data.OrderBy(b => b.BId).ToList();
                    break;
                default:
                    data = data.OrderBy(b => b.BId).ToList();
                    break;
            }
            int totalRecords = data.Count;
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            //ViewBag properties
            ViewBag.CourtID = courtId;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchTerm = searchTerm;

            return View(pagedData);
        }
        public IActionResult Detail(int bookingId, int page = 1, string sortOrder = "", string searchTerm = "")
        {
            int NoOfRecordPerPage = 5;
            searchTerm = searchTerm?.ToLower().Trim();

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            //TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            // Filter by bookingId
            var filteredData = _context.TimeSlots
                .Include(ts => ts.Co)
                .Include(ts => ts.BIdNavigation)
                .Where(ts => ts.BId == bookingId && ts.TsDate == currentDate).ToList();
            /*&& ts.TsStart >= currentTime*/

            // Apply search filter
            /*   var data = filteredData
                   .Where(ts => string.IsNullOrEmpty(searchTerm) ||
                                ts.TsId.ToString().ToLower().Equals(searchTerm) ||
                                ts.Co.CoName.ToLower().Equals(searchTerm.ToLower()) ||
                                ts.TsDate.ToString().ToLower().Equals(searchTerm) ||
                                ts.TsStart.ToString().Equals(searchTerm)||
                                ts.TsEnd.ToString().Equals(searchTerm)||
                                ts.BIdNavigation.BBookingType.ToLower().Equals(searchTerm.ToLower()))
                   .ToList();*/
            List<TimeSlot> data;

            // Apply search filter
            if (int.TryParse(searchTerm, out int searchId))
            {
                // Search by TsId
                data = filteredData
                    .Where(ts => ts.TsId == searchId)
                    .ToList();
            }
            else
            {
                // Search by other fields
                data = filteredData
                    .Where(ts => string.IsNullOrEmpty(searchTerm) ||
                                 ts.Co.CoName.ToLower().Contains(searchTerm.ToLower()) ||
                                 ts.TsDate.ToString().ToLower().Contains(searchTerm.ToLower()) ||
                                 ts.TsStart.ToString().Contains(searchTerm.ToLower()) ||
                                 ts.TsEnd.ToString().Contains(searchTerm.ToLower()) ||
                                 ts.BIdNavigation.BBookingType.ToLower().Equals(searchTerm.ToLower()))
                    .ToList();
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "old":
                    data = data.OrderBy(ts => ts.TsId).ToList();
                    break;
                case "new":
                    data = data.OrderByDescending(ts => ts.TsId).ToList();
                    break;
                case "needtodo":
                    data = data.Where(ts => ts.TsCheckedIn == false).ToList();
                    break;
                case "today":
                    data = data.Where(ts => ts.TsDate == DateOnly.FromDateTime(DateTime.Today)).ToList();
                    break;
                default:
                    data = data.OrderBy(ts => ts.TsId).ToList();
                    break;
            }

            // Paging logic
            int totalRecords = data.Count;
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            // ViewBag properties
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.bookingId = bookingId;
            return View(pagedData);
        }
        [Authorize(Roles = "Staff")]
        public IActionResult Approve(int tsid, int bookingId)
        {
            // DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            var updated = _context.TimeSlots.FirstOrDefault(ts => ts.TsId == tsid);
            if (currentTime < updated.TsStart)
            {
                TempData["error"] = "You can not check in right now!";
                return RedirectToAction("Detail", new { bookingId = bookingId });
            }
            updated.TsCheckedIn = true;
            _context.TimeSlots.Update(updated);
            _context.SaveChanges();
            TempData["message"] = "Checked in successfully!";
            return RedirectToAction("Detail", new { bookingId = bookingId });


        }
        //User section:
        [Authorize]
        public IActionResult Customer(int page = 1, string searchTerm = "")
        {
            int NoOfRecordPerPage = 6;
            var userId = _userManager.GetUserAsync(User).Result?.Id;
            //get court info based on booking
            //var courts = _context.Courts
            //   .Where(c => _context.Bookings.Any(b => b.UserId == userId && b.CoId == c.CoId))
            //   .ToList();
            var filteredCourts = _context.Courts
           .Where(c => _context.Bookings.Any(b => b.UserId == userId && b.CoId == c.CoId)).ToList();

            List<Court> data;

            if (int.TryParse(searchTerm, out int searchId))
            {
                // Search by CoId
                data = filteredCourts
                    .Where(c => c.CoId == searchId||c.CoPrice==searchId)
                    .ToList();
            }
            else
            {
                // Search by other fields
                data = filteredCourts
                    .Where(c =>
                        string.IsNullOrEmpty(searchTerm) ||
                        c.CoName.ToLower().Contains(searchTerm.ToLower()) ||
                        c.CoAddress.ToLower().Contains(searchTerm.ToLower()) ||
                        c.CoInfo.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
            }


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
            ViewBag.SearchTerm = searchTerm;
            return View(pagedData);
        }
        [Authorize]
        public IActionResult CheckOut(int courtId, int page = 1, string searchTerm = "", string sortOrder = "")
        {
            int NoOfRecordPerPage = 6;

            string userId = _userManager.GetUserId(User);
            // First part: filter by courtId
            var courtFilteredData = _context.Bookings
                .Where(b => b.CoId == courtId && b.UserId == userId)
                .Include(b => b.TimeSlots)
                .Include(b => b.Co)
                .ToList();
            /*  var data = courtFilteredData
          .Where(b =>
                     string.IsNullOrEmpty(searchTerm) ||
                     b.BId.ToString().Equals(searchTerm) ||
                     b.Co.CoName.ToLower().Equals(searchTerm.ToLower()) ||
                     b.BBookingType.ToLower().Equals(searchTerm.ToLower()))
                     .ToList(); */

            List<Booking> data;

            if (int.TryParse(searchTerm, out int searchId))
            {
                // Search by BId
                data = courtFilteredData
                    .Where(b => b.BId == searchId)
                    .ToList();
            }
            else
            {
                // Search by other fields
                data = courtFilteredData
                    .Where(b =>
                        string.IsNullOrEmpty(searchTerm) ||
                        b.Co.CoName.ToLower().Contains(searchTerm.ToLower()) ||
                        b.BBookingType.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
            }

            switch (sortOrder)
            {
                case "recent":
                    data = data.OrderByDescending(b => b.BId).ToList();
                    break;
                case "old":
                    data = data.OrderBy(b => b.BId).ToList();
                    break;
                default:
                    data = data.OrderBy(b => b.BId).ToList();
                    break;
            }
            int totalRecords = data.Count;
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            //ViewBag properties
            ViewBag.CourtID = courtId;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchTerm = searchTerm;
            return View(pagedData);

        }



        [HttpGet]
        public IActionResult CheckoutDetail(int bookingId, int courtId, int page = 1, string searchTerm = "", string sortOrder = "")
        {
            var userId = _userManager.GetUserAsync(User).Result?.Id;
            string txtsearchTerm = searchTerm?.ToLower();
            int NoOfRecordPerPage = 6; // Adjust this number as needed

            // Retrieve the data with the necessary filtering and joins
            var courtFilteredData = _context.TimeSlots
                .Include(ts => ts.Co)
                .Include(ts => ts.BIdNavigation)
                .Where(ts => ts.BId == bookingId && ts.BIdNavigation.UserId == userId && ts.CoId == courtId).ToList();

            // Apply search filter
            /*    var data = courtFilteredData
             .Where(ts =>
                        string.IsNullOrEmpty(txtsearchTerm) ||
                        ts.TsId.ToString().Equals(txtsearchTerm) ||
                        ts.Co.CoName.ToLower().Equals(txtsearchTerm.ToLower()) || ts.Co.CoAddress.ToLower().Equals(txtsearchTerm.ToLower())
                        || ts.TsDate.ToString().Equals(txtsearchTerm)
                        ).ToList(); */
            List<TimeSlot> data;

            // First part: Search by TsId
            if (int.TryParse(txtsearchTerm, out int searchId))
            {
                data = courtFilteredData
                    .Where(ts => ts.TsId == searchId)
                    .ToList();
            }
            else
            {
                // Second part: Search by other fields
                data = courtFilteredData
                    .Where(ts =>
                        string.IsNullOrEmpty(txtsearchTerm) ||
                        ts.Co.CoName.ToLower().Contains(txtsearchTerm) ||
                        ts.Co.CoAddress.ToLower().Contains(txtsearchTerm) ||
                        ts.TsDate.ToString().Contains(txtsearchTerm)||
                        ts.TsStart.ToString().Contains(txtsearchTerm)||
                        ts.TsEnd.ToString().Contains(txtsearchTerm))
                    .ToList();
            }


            // Apply sorting
            switch (sortOrder)
            {
                case "new":
                    data = data.OrderByDescending(ts => ts.TsId).ToList();
                    break;
                case "old":
                    data = data.OrderBy(ts => ts.TsId).ToList();
                    break;
                case "today":
                    data = data.Where(ts => ts.TsDate == DateOnly.FromDateTime(DateTime.Today)).ToList();
                    break;
                case "pending":
                    data = data.Where(ts => ts.TsCheckedIn == false).ToList();
                    break;
                default:
                    data = data.OrderByDescending(ts => ts.TsId).ToList();
                    break;
            }

            int totalRecords = data.Count;
            int NoOfPages = (int)Math.Ceiling((double)totalRecords / NoOfRecordPerPage);
            if (NoOfPages == 0) NoOfPages = 1;

            // Pagination logic
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            var pagedData = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            //ViewBag properties
            ViewBag.courtId = courtId;
            ViewBag.bookingId = bookingId;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchTerm = searchTerm;


            return View(pagedData);
        }
        [HttpGet]
        public IActionResult Rating(int courtId)
        {
            //_httpContextAccessor.HttpContext.Session.Remove("CoId");
            //if (courtId.HasValue)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("CoId", courtId.ToString());
            //}
            //int coid = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId"));

            string userId = _userManager.GetUserId(User);
            var data = _context.Courts.Where(c => _context.Bookings.Any(b => b.UserId == userId && b.CoId == c.CoId)).Include(c => c.Bookings)
                                       .FirstOrDefault(c => c.CoId == courtId);

            // Check if court data is found
            if (data == null)
            {
                TempData["error"] = "Court not found for rating";
                return RedirectToAction("Customer", "Home");
            }

            return View(data);
        }
        [HttpPost]
        public IActionResult RatingSubmit(Rating rating)
        {
            Rating ratingCourt = new Rating
            {


                CourtId = rating.CourtId,
                UserId = rating.UserId,
                Review = rating.Review,
                Rating1 = rating.Rating1,
               CreatedAt = DateTime.Now,
            };
            _context.Ratings.Add(ratingCourt);
            _context.SaveChanges();
            TempData["message"] = "Your review has been submitted";
            return RedirectToAction("customer");
        }
        public IActionResult CourtDetail(int CourtId, int page = 1)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            int NoOfRecordPerPage = 3;
            var data = context.Ratings.Include(r => r.Court).Where(r => r.CourtId == CourtId).ToList();
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
            ViewBag.CourtId = CourtId;
            return View(pagedData);
        }


        //public IActionResult BookingHistory(string userID)
        //{


        //    DemobadmintonContext context = new DemobadmintonContext();
        //    var bookingHistory = context.Bookings.Where(b => b.UserId == userID)
        //    .Include(b => b.Payments)
        //    .Include(b => b.Co)
        //    .ToList();
        //    return View(bookingHistory);
        //}

        public IActionResult BookingHistory(string userID, DateTime? startDate = null, DateTime? endDate = null)
        {
            var bookingHistory = _context.Bookings
                .Where(b => b.UserId == userID)
                .Include(b => b.Payments)
                .Include(b => b.Co)
                .AsQueryable();

            if (startDate.HasValue)
            {
                bookingHistory = bookingHistory.Where(b => b.Payments.Any(p => p.PDateTime >= startDate.Value));
            }

            if (endDate.HasValue)
            {
                bookingHistory = bookingHistory.Where(b => b.Payments.Any(p => p.PDateTime <= endDate.Value));
            }

            var sortedBookingHistory = bookingHistory
                .OrderByDescending(b => b.Payments.Max(p => p.PDateTime))
                .ToList();

            ViewBag.UserID = userID;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(sortedBookingHistory);
        }


        //public IActionResult UpComingEvent(string userID, string filter = null)
        //{
        //    DemobadmintonContext context = new DemobadmintonContext();
        //    var futureTimeSlot = context.Bookings
        //        .Where(b => b.UserId == userID)
        //        .Include(b => b.Co)
        //        .Include(b => b.TimeSlots)
        //        .ToList();
        //    var today = DateOnly.FromDateTime(DateTime.Today);
        //    var validFutureTimeSlot = futureTimeSlot
        //        .Where(b => b.TimeSlots.Any(ts =>
        //            ts.TsCheckedIn == false &&
        //            ts.TsDate > today))
        //        .ToList();

        //    if (!string.IsNullOrEmpty(filter))
        //    {
        //        validFutureTimeSlot = validFutureTimeSlot
        //            .Where(b => b.BBookingType.Normalize().ToString() == filter.Normalize())
        //            .ToList();
        //    }
        //    ViewBag.UserID = userID;
        //    ViewBag.CurrentFilter = filter;
        //    return View(validFutureTimeSlot);
        //}
        [Authorize]
        public IActionResult UpComingEvent(string userID, string filter = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var futureTimeSlot = context.Bookings
                .Where(b => b.UserId == userID)
                .Include(b => b.Co)
                .Include(b => b.TimeSlots)
                .ToList();

            var today = DateOnly.FromDateTime(DateTime.Today);
            var validFutureTimeSlot = futureTimeSlot
                .Where(b => b.TimeSlots.Any(ts =>
                    ts.TsCheckedIn == false &&
                    ts.TsDate > today))
                .ToList();

            if (!string.IsNullOrEmpty(filter))
            {
                validFutureTimeSlot = validFutureTimeSlot
                    .Where(b => b.BBookingType.Normalize().ToString() == filter.Normalize())
                    .ToList();
            }

            // Apply date range filter
            if (startDate.HasValue && endDate.HasValue)
            {
                var startDateOnly = DateOnly.FromDateTime(startDate.Value);
                var endDateOnly = DateOnly.FromDateTime(endDate.Value);

                validFutureTimeSlot = validFutureTimeSlot
                    .Where(b => b.TimeSlots.Any(ts =>
                        ts.TsDate >= startDateOnly && ts.TsDate <= endDateOnly))
                    .ToList();
            }

            // Sort the timeslots
            validFutureTimeSlot = validFutureTimeSlot
                .OrderBy(b => b.TimeSlots.Min(ts => ts.TsDate))
                .ThenBy(b => b.TimeSlots.Min(ts => ts.TsStart))
                .ToList();

            ViewBag.UserID = userID;
            ViewBag.CurrentFilter = filter;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(validFutureTimeSlot);
        }
        public async Task<IActionResult> CourtToCheckQuality()
        {
            var court = await _context.Courts.ToListAsync();
            return View(court);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public IActionResult CourtQualityCheck(int? CoId)
        {
            _httpContextAccessor.HttpContext.Session.Remove("CoId");
            if (CoId.HasValue)
            {
                _httpContextAccessor.HttpContext.Session.SetString("CoId", CoId.ToString());
            }
            return View();
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public IActionResult CourtQualityCheck(CourtQualityViewModel model)
        {
            if(ModelState.IsValid)
            {
                int CourtId = int.Parse(HttpContext.Session.GetString("CoId"));
                CourtCondition courtCondition = new CourtCondition() 
                { 
                    CdCleanlinessCondition = model.CdCleanlinessCondtion,
                    CdLightningCondition = model.CdLightningCondition,
                    CdNetCondition = model.CdNetCondition,
                    CdSurfaceCondition = model.CdSurfaceCondition,
                    CdOverallCondition = model.CdOverallCondition,
                    CoId = CourtId,
                    CdCreatedAt = DateTime.Now,
                    CdNotes = model.CdNotes,
                };
                _context.CourtConditions.Add(courtCondition);
                _context.SaveChanges();
                if(User.IsInRole("Staff")) return RedirectToAction("Staff", "Home");
                if(User.IsInRole("Manager")) return RedirectToAction("Booking", "Manager");
                return RedirectToAction("CourtToCheckQuality", "Home");
            }
            return View()
;
        }
        [Authorize(Roles = "Manager")]
        public IActionResult QualityCheckHistory(int CoID)
        {
            var qcHistory = _context.CourtConditions.Where(c => c.CoId == CoID).Include(c =>c.Co).ToList();
            return View(qcHistory);
        }
    }
}
