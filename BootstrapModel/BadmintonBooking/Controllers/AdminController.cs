using BadmintonBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BadmintonBooking.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public AdminController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public IActionResult Show()
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var courtlist= context.Courts.ToList();
            return View(courtlist);
        }
        public IActionResult AddCourt()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCourt(Court model)
        {
            try
            {
                ModelState.Remove("UserId");
                ModelState.Remove("User");
                if (ModelState.IsValid)
                {


                    DemobadmintonContext context = new DemobadmintonContext();

                    string uniqueFileName = UploadImage(model);
                    var data = new Court()
                    {
                        CoName = model.CoName,
                        CoAddress = model.CoAddress,
                        CoInfo = model.CoInfo,
                        CoPrice = model.CoPrice,
                        UserId = "1",
                        CoStatus=true,
                        CoPath = uniqueFileName,
                    };
                    context.Courts.Add(data);
                    context.SaveChanges();
                    TempData["Success"] = "Record saved successfully";


                    return RedirectToAction("Show");

                }
                ModelState.AddModelError(string.Empty, "Model properties is not valid, please check");

            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
        public IActionResult DetailCourt(int id)
        {
            DemobadmintonContext context = new DemobadmintonContext();
            var data = context.Courts.FirstOrDefault(c => c.CoId == id);
            if (data==null)
            {
                return NotFound();
            }

            return View(data);


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
    }
}
