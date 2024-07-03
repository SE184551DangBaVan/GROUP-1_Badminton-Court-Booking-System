using BadmintonBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        public IActionResult Date(int CoId, string Types)
        {
            string userId = _userManager.GetUserId(User);
            var booking = _context.Bookings.FirstOrDefault(c => c.UserId == userId && c.BTotalHours.HasValue && c.BTotalHours > 0);
            //missing court compare
            if(booking != null && userId != null)
            {
                var time = _context.Payments.FirstOrDefault(c => c.BId == booking.BId).PDateTime.AddDays(30);
                ViewData["Hours"] = booking.BTotalHours;
                ViewData["Remain"] = (time - DateTime.Now).Days;
                ViewData["BookingId"] = booking.BId;
                Types = "Flexible";
            }
            _httpContextAccessor.HttpContext.Session.SetString("CoId", CoId.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("Types", Types);
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

        public IActionResult Book2(int page = 1, string address = "", string sortOrder = "")
        {
            int NoOfRecordPerPage = 5;

            ViewBag.SearchAddress = address;


            // Get court list based on group
            var data = _context.Courts
                               .Where(c => string.IsNullOrEmpty(address) || c.CoAddress == address)
                               .ToList();
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
        [Authorize(Roles = "Staff")]
        public IActionResult CheckIn(int page = 1, string searchTerm = "", string sortOrder = "")
        {
            int NoOfRecordPerPage = 10;
            var data = _context.Bookings.Where(b=> string.IsNullOrEmpty(searchTerm) || b.BId.ToString().Equals(searchTerm) ||  b.Co.CoName.Contains(searchTerm) ||
            b.BBookingType.Contains(searchTerm)).Include(b => b.TimeSlots).Include(b => b.Co).ToList();
            switch (sortOrder)
            {
                case "recent_Booking":
                    data = data.OrderByDescending(b => b.BId).ToList();
                    break;
                case "name_Asc":
                    data = data.OrderBy(b => b.Co.CoName).ToList();
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
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchTerm = searchTerm;

            return View(pagedData);
        }
        public IActionResult Detail(int bookingId)
        {
            var data = _context.TimeSlots.Include(ts => ts.Co).Include(ts => ts.BIdNavigation).Where(ts => ts.BId == bookingId).ToList();
            return View(data);

        }
        public IActionResult Approve(int tsid, int bookingId)
        {
            var updated = _context.TimeSlots.FirstOrDefault(ts => ts.TsId == tsid);
            updated.TsCheckedIn = true;
            _context.TimeSlots.Update(updated);
            _context.SaveChanges();
            TempData["message"] = "Checked in successfully!";
            return RedirectToAction("Detail", new { bookingId = bookingId });


        }
        [Authorize]
        public IActionResult CheckOut(int page=1,string searchTerm="",string sortOrder="")
        {
            int NoOfRecordPerPage = 10;
            //string userId = _httpContextAccessor.HttpContext.Session.GetString("CusId");
            string userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();

            }
            var data = _context.Bookings
        .Where(b => b.UserId == userId &&
                  (string.IsNullOrEmpty(searchTerm) ||
                   b.BId.ToString().Equals(searchTerm) ||
                   b.Co.CoName.Contains(searchTerm) ||
                   b.BBookingType.Contains(searchTerm)))
       .Include(b => b.TimeSlots)
       .Include(b => b.Co)
       .ToList();
            switch (sortOrder)
            {
                case "recent_Booking":
                    data = data.OrderByDescending(b => b.BId).ToList();
                    break;
                case "name_Asc":
                    data = data.OrderBy(b=>b.Co.CoName).ToList();
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
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchTerm = searchTerm;
            return View(pagedData);

        }
        public IActionResult CheckoutDetail(int bookingid)
        {

            var data = _context.TimeSlots.Include(ts => ts.Co).Include(ts => ts.BIdNavigation).Where(ts => ts.BId == bookingid).ToList();
            return View(data);
        }
        public IActionResult Rating(int courtid, string userid, int bookingId)
        {
            ViewData["bookingId"] = bookingId;
            var data = _context.Courts.Include(c => c.Bookings).FirstOrDefault(c => c.CoId == courtid);
            return View(data);
        }
        [HttpPost]
        public IActionResult Rating(Rating rating, int bookingId)
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
            return RedirectToAction("CheckoutDetail", new { bookingId = bookingId });
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
    }
}
