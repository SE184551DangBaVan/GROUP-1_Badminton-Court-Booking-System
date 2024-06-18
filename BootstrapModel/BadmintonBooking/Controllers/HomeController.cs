using BadmintonBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace BadmintonBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var courtlist = context.Courts.Where(c => c.CoStatus == true).Take(3).ToList();


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
        [Authorize]
        public IActionResult Date(int CoId, string UserId)
        {
            TempData["CoId"] = CoId;
            TempData["UserId"] = UserId;
            return View();
        }
        
        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Invoice()
        {
            return View();
        }
        [Authorize]
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

        public IActionResult Book2()
        {

            DemobadmintonContext context = new DemobadmintonContext();
            var courtlist = context.Courts.Where(c => c.CoStatus == true).ToList();
            return View(courtlist);
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
    }
}
