using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ZaykaMvc.Models;

namespace ZaykaMvc.Controllers
{
	public class AdminController : Controller
	{
        private readonly ZaykaDbContext context;
        private IWebHostEnvironment environment;

        public AdminController(ZaykaDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }



        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Username, string Password)
        {
            try
            {

                if (Username.Equals("ZaykaAdmin") && Password.Equals("Zayka@admin"))
                {
                    HttpContext.Session.SetString("admin", Username);
                    return RedirectToAction("AllDish");
                }
                else
                {
                    string error = "Invalid Credentials";
                    ViewBag.error = error;
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        public IActionResult AddDish()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddDish(FakeDish fakedish)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToAction("Login");
            }
            if (fakedish.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image File Is Required");
            }

            if (!ModelState.IsValid)
            {
                return View(fakedish);
            }


            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(fakedish.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                fakedish.ImageFile.CopyTo(stream);
            }
            Dish dish = new Dish();

            dish.Name = fakedish.Name;
            dish.Price = fakedish.Price;
            dish.Quantity = fakedish.Quantity;
            dish.ImageFileName = newFileName;

            context.Dishes.Add(dish);
            context.SaveChanges();

            ViewBag.Message = "Dish Added Successfully";

            return View();

        }

        public async Task<IActionResult> AllDish()
        {
            if (HttpContext.Session.GetString("admin") == null) {
                return RedirectToAction("Login");
            }
            return context.Dishes != null ?
                        View(await context.Dishes.ToListAsync()) :
                        Problem("Entity set 'ZaykaDbContext.Users'  is null.");
        }

        public async Task<IActionResult> EditDish(int? id)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToAction("Login");
            }
            if (id == null || context.Dishes == null)
            {
                return NotFound();
            }

            var user = await context.Dishes.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDish(int id,Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(dish);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("AllDish");
            }
            return View(dish);
        }



        public async Task<IActionResult> DeleteDish(int? id)
        {
            if (id == null || context.Dishes == null)
            {
                return NotFound();
            }

            var user = await context.Dishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (context.Dishes == null)
            {
                return Problem("Entity set 'ZaykaDbContext.Users'  is null.");
            }
            var dish = await context.Dishes.FindAsync(id);
            if (dish != null)
            {
                context.Dishes.Remove(dish);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("AllDish");
        }

        public IActionResult LogOut() {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }

        public IActionResult AllOrders()
        {
            if (context == null) {
                return NotFound();
            }
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToAction("Login");
            }
            return context.Dishes != null ?
                        View( context.Orders.ToList()) :
                        Problem("Entity set 'ZaykaDbContext.Users'  is null.");
        }
    }

   
}

    


