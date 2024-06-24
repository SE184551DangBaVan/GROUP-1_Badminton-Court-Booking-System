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

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(string txtSearch)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var courtlist = context.Courts.Where(c => c.CoStatus == true).ToList();


            return View(courtlist);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Date(int CoId, string Types)
        {
            _httpContextAccessor.HttpContext.Session.SetString("CoId", CoId.ToString());            
            _httpContextAccessor.HttpContext.Session.SetString("Types", Types);
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Book(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }
        
        public IActionResult Book2(int page = 1, string address = "", string sortOrder = "")
        {
            DemobadmintonContext context = new DemobadmintonContext();
            int NoOfRecordPerPage = 5;

            ViewBag.SearchAddress = address;


            // Get court list based on group
            var data = context.Courts
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
        public IActionResult CheckIn()
        {
            DemobadmintonContext context = new DemobadmintonContext();
            // var guestid = _httpContextAccessor.HttpContext.Session.GetString("GuestId");
            var data = context.Bookings.Include(b => b.TimeSlots).Include(b => b.Co).ToList();
            return View(data);
        }
        public IActionResult Detail(int bookingId)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.TimeSlots.Include(ts => ts.Co).Include(ts => ts.BIdNavigation).Where(ts => ts.BId == bookingId).ToList();
            return View(data);

        }
        public IActionResult Approve(int tsid, int bookingId)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var updated = context.TimeSlots.FirstOrDefault(ts => ts.TsId == tsid);
            updated.TsCheckedIn = true;
            context.TimeSlots.Update(updated);
            context.SaveChanges();
            return RedirectToAction("Detail", new { bookingId = bookingId });


        }
    }
}
