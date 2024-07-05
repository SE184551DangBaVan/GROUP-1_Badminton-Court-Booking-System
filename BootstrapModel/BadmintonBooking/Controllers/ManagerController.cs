using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonBooking.Controllers
{
    //[Authorize(Roles ="Manager")]
    public class ManagerController : Controller
    {
        public IActionResult Booking()
        {
            return View();
        }
    }
}
