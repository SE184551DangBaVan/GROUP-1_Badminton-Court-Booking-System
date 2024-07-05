using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BadmintonBooking.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Caching.Memory;


namespace BadmintonBooking.Controllers
{
    public class BookingController : Controller
    {
        private readonly DemobadmintonContext _demobadmintonContext;
        private static List<TimeSlot> _slots = new List<TimeSlot>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _UserManager;

        public BookingController(DemobadmintonContext demobadmintonContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, IMemoryCache memoryCache)
        {
            _demobadmintonContext = demobadmintonContext;
            _httpContextAccessor = httpContextAccessor;
            _UserManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookSlots()
        {
            int court = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId"));
            var booked = await _demobadmintonContext.TimeSlots.Where(x => x.CoId == court).ToListAsync();
            return Ok(booked);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel()
        {
            _slots.Clear();
            _httpContextAccessor.HttpContext.Session.Remove("Booking");
            return Ok(new { message = "Cancel successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingData bookingData)
        {
            string userId = _UserManager.GetUserId(User);
            // Check if user ID is found
            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated. Redirecting to login." });
            }
            try
            {
                if (bookingData == null)
                {
                    return BadRequest("Invalid booking data.");
                }

                TimeOnly time = TimeOnly.ParseExact(bookingData.Time, "h:mm tt", CultureInfo.InvariantCulture);
                DateOnly date = DateOnly.ParseExact(bookingData.Date, "MMM d", CultureInfo.InvariantCulture);
                bool booked = bookingData.Booked;


                Console.WriteLine($"Parsed Booking Data - Time: {time}, Date: {date}, Booked: {booked}");


                TimeSlot slot = new TimeSlot()
                {
                    CoId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId")),
                    TsCheckedIn = false,
                    TsDate = date,
                    TsStart = time,
                    TsEnd = time.AddHours(1),
                };
                _slots.Add(slot);
                int quantity = _slots.Count;
                return Ok(new { message = "Booking data received successfully." });
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error processing booking: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBooking(int totalWeeks, int totalHours, int totalPrice)
        {
            int quantity = _slots.Count;
            string types = _httpContextAccessor.HttpContext.Session.GetString("Types");
            Booking booking = new Booking()
            {
                UserId = _UserManager.GetUserId(User),
                BBookingType = types,
                CoId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CoId")),
                BTotalHours = totalHours,
                BGuestName = _UserManager.GetUserName(User)
            };
            if (types == "Fixed")
            {
                foreach (var item in _slots)
                {
                    for (int i = 0; i < totalWeeks; i++)
                    {
                        TimeSlot timeSlot = new TimeSlot()
                        {
                            CoId = item.CoId,
                            TsCheckedIn = item.TsCheckedIn,
                            TsDate = item.TsDate.AddDays(7 * i),
                            TsStart = item.TsStart,
                            TsEnd = item.TsEnd,
                        };
                        booking.TimeSlots.Add(timeSlot);
                    }
                }
                Payment payment = new Payment()
                {
                    PDateTime = DateTime.Now,
                    PAmount = totalPrice,
                };
                booking.Payments.Add(payment);
                quantity = booking.TimeSlots.Count;
            }
            else if (booking.BBookingType == "Flexible")
            {
                booking.BTotalHours = totalHours - quantity;
                Payment payment = new Payment()
                {
                    PDateTime = DateTime.Now,
                    PAmount = totalPrice,
                };
                booking.Payments.Add(payment);
                foreach (var item in _slots)
                {
                    booking.TimeSlots.Add(item);
                }
                quantity = totalHours;
            }
            else
            {
                Payment payment = new Payment()
                {
                    PDateTime = DateTime.Now,
                    PAmount = totalPrice,
                };
                booking.Payments.Add(payment);
                foreach (var item in _slots)
                {
                    booking.TimeSlots.Add(item);
                }
            }
            foreach (var ts in booking.TimeSlots.ToList())
            {
                Console.WriteLine($"TimeSlot - Date: {ts.TsDate}, Start: {ts.TsStart}, End: {ts.TsEnd}, CheckedIn: {ts.TsCheckedIn}, CoId: {ts.CoId}");
            }
            _slots.Clear();
            _httpContextAccessor.HttpContext.Session.SetString("quantity", quantity.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("totalPrice", totalPrice.ToString());
            var jsonString = JsonConvert.SerializeObject(booking);
            _httpContextAccessor.HttpContext.Session.SetString("Booking", jsonString);
            return RedirectToAction("PaymentWithPaypal", "PayPal");
        }

        [HttpPost]
        public async Task<IActionResult> UseFlexible(int bId)
        {
            int quantity = _slots.Count;
            foreach (var item in _slots)
                {
                    item.BId = bId;
                    await _demobadmintonContext.TimeSlots.AddAsync(item);                   
                }
            var booking = _demobadmintonContext.Bookings.FirstOrDefault(c => c.BId == bId);
            booking.BTotalHours -= quantity;
            _demobadmintonContext.Bookings.Update(booking);
            await _demobadmintonContext.SaveChangesAsync();
            _slots.Clear();
            TempData["Message"] = "Booked successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SaveBookingToDb()
        {
            try
            {
                var jsonString = _httpContextAccessor.HttpContext.Session.GetString("Booking");
                if (string.IsNullOrEmpty(jsonString))
                {
                    return null;
                }

                var booking = JsonConvert.DeserializeObject<Booking>(jsonString);
                if (booking == null)
                {
                    return BadRequest("Booking data is missing.");
                }

                var saveResult = await SaveBookingsToDatabase(booking);
                if (!saveResult)
                {
                    return StatusCode(500, "Failed to save booking to database.");
                }

                return RedirectToAction("PaymentSuccess", "Paypal");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error saving booking to database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<bool> SaveBookingsToDatabase(Booking booking)
        {
            try
            {
                await _demobadmintonContext.AddAsync(booking);
                await _demobadmintonContext.SaveChangesAsync();

                //get Bid for paypal invoice
                _httpContextAccessor.HttpContext.Session.SetInt32("BId", booking.BId);
                _httpContextAccessor.HttpContext.Session.Remove("Booking");

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error saving bookings to database: {ex.Message}");
                return false;
            }
        }
    }
}
