using Holoul.Areas.Identity.Data;
using Holoul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holoul.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly EDbContext _context;
        private readonly UserManager<HoloulUser> _userManager;

        public UserController(EDbContext context, UserManager<HoloulUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Dashbord()
        {
            return View();
        }
        public IActionResult categories()
        {
            return View();
        }
        public IActionResult contact()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult contact(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.FeedBacks.Add(feedback);
                    _context.SaveChanges();
                    ViewBag.SuccessMessage = "Thank you for your feedback!we will answer it soon<3";
                    ModelState.Clear();
                    return View(new Feedback());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    ViewBag.Errormsg = "Something went wrong!";
                    return View(feedback);
                }
            }
            return View(feedback);
        }
        public IActionResult submitaproblem()
        {
            return View();
        }
       /* public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(model);
        }*/
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // ✅ **جلب جميع الرسائل الخاصة بالمستخدم**
            var feedbacks = await _context.FeedBacks
                .Where(f => f.Email == user.Email) // 🔹 جلب فقط الرسائل التي تخص المستخدم الحالي
                .OrderByDescending(f => f.CreatedAt) // 🔹 ترتيب الرسائل من الأحدث إلى الأقدم
                .ToListAsync();

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Feedbacks = feedbacks // ✅ **تمرير قائمة الرسائل إلى الملف الشخصي**
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            // Update the user's properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            // Save the changes
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Display a success message
                TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح.";
                return RedirectToAction("Profile"); // Redirect to the profile page
            }
            else
            {
                // Display error messages
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model); // Return to the form with error messages
            }
        }
    }
}
