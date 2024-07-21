using BadmintonBooking.Models;
using BadmintonBooking.Repository.Service;
using BadmintonBooking.ViewModels;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal;
using PayPal.Api;

namespace BadmintonBooking.Controllers
{
    public class PayPalController : Controller
    {
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DemobadmintonContext _context;
        private PayPal.Api.Payment payment;

        public PayPalController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UserManager<IdentityUser> userManager, DemobadmintonContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public ActionResult PaymentWithPaypal(string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
        {
            var ClientID = _configuration.GetValue<string>("PayPal:Key");
            var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            var mode = _configuration.GetValue<string>("PayPal:mode");

            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);

            try
            {
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/PayPal/PaymentWithPaypal?";
                    string guidd = Convert.ToString(new Random().Next(10000));
                    guid = guidd;

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links Ink = links.Current;
                        if (Ink.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = Ink.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaymentFailed");
                    }
                    //var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
                    return RedirectToAction("SaveBookingToDb", "Booking");
                }
            }
            catch (Exception ex)
            {
                return View("PaymentFailed");
            }
            return View("SuccessView");
        }

        private PayPal.Api.Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new PayPal.Api.Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }


        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            var quantity = int.Parse(httpContextAccessor.HttpContext.Session.GetString("quantity"));
            string Types = httpContextAccessor.HttpContext.Session.GetString("Types");
            int court = int.Parse(httpContextAccessor.HttpContext.Session.GetString("CoId"));
            var price = _context.Courts.FirstOrDefault(x => x.CoId == court).CoPrice;
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            {
                itemList.items.Add(new Item()
                {
                    name = "Book Time Slot "+Types,
                    currency = "USD",
                    price = price.ToString(),
                    quantity = quantity.ToString(),
                    sku = "BOOK-TS-101"
                });
                var payer = new Payer()
                {
                    payment_method = "paypal"
                };

                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl + "&Cancel=true",
                    return_url = redirectUrl
                };

                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = (quantity * price).ToString()
                };

                var amount = new Amount()
                {
                    currency = "USD",
                    total = ((quantity * price)).ToString(), // Total must be equal to sum of tax, shipping and subto //
                    details = details
                };

                var transactionList = new List<Transaction>();
                // Adding description about the transaction

                transactionList.Add(new Transaction()
                {
                    description = "Transaction description",
                    invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No
                    amount = amount,
                    item_list = itemList

                });

                this.payment = new PayPal.Api.Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
                return this.payment.Create(apiContext);
            }
        }

        public IActionResult Invoice(int bid = -1)
        {
            InvoiceViewModel invoiceViewModel = null;
            try
            {

                //for method transfer bid using route data
                int? bId;
                if (bid != -1) bId = bid;
                //for transfering bid using session
                else
                {
                    bId = httpContextAccessor.HttpContext.Session.GetInt32("BId");
                    if (bId == null)
                    {
                        throw new Exception("BId not found in session.");
                    }
                }
                // Fetch payment using BId
                string formattedDate = null;
                string formattedTime = null;
                Models.Payment? payment = _context.Payments.FirstOrDefault(p => p.BId == bId.Value);
                if (payment != null)
                {
                    formattedDate = payment.PDateTime.ToString("MMMM dd, yyyy");
                    formattedTime = payment.PDateTime.ToString("hh:mm tt"); // Format time as HH:MM AM/PM
                    ViewData["formattedDate"] = formattedDate;
                    ViewData["formattedTime"] = formattedTime;
                }
                var booking = _context.Bookings.Include(b => b.TimeSlots).FirstOrDefault(b => b.BId == bId.Value);
                string typeOfBooking = booking.BBookingType;
                string courtName = _context.Courts.FirstOrDefault(c => c.CoId == booking.CoId).CoName;
                int? quantity;
                if (booking.BBookingType == "Flexible")
                {
                     quantity = booking.BTotalHours;
                }
                else
                {
                     quantity = booking.TimeSlots.Count;
                }
                
                invoiceViewModel = new InvoiceViewModel()
                {
                    PId = payment.PId,
                    formattedDate = formattedDate,
                    formattedTime = formattedTime,
                    toUser = _userManager.GetUserName(User),
                    typeOfBooking = typeOfBooking,
                    courtName = courtName,
                    amount = payment.PAmount,
                    Quantity = quantity
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(invoiceViewModel);
        }
    }
}
