using Holoul.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Holoul.Models;

namespace Holoul.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly EDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<HoloulUser> _userManager;

        public AdminController(EDbContext context, IConfiguration configuration, UserManager<HoloulUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }
        public IActionResult categories()
        {
            return View();
        }

        public IActionResult Dashbord()
        {
            return View();
        }
        public async Task<IActionResult> feedback()
        {
            var feedbacks = await _context.FeedBacks
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(feedbacks);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.FeedBacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            try
            {
                _context.FeedBacks.Remove(feedback);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Feedback deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting feedback: " + ex.Message;
            }

            return RedirectToAction(nameof(feedback));
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        /*public async Task<IActionResult> ReplyToFeedback(int id, string Subject, string Message, string Email)
        {
            var feedback = await _context.FeedBacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            try
            {
                // Send email reply
                await SendEmailReply(Email, Subject, Message);

                // Log the reply in the database (optional)
                feedback.IsReplied = true;
                feedback.RepliedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reply sent successfully to " + Email;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error sending reply: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToFeedback(int id, string Subject, string Message, string Email)
        {
            var feedback = await _context.FeedBacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            try
            {
                // حفظ الرد داخل قاعدة البيانات
                feedback.IsReplied = true;
                feedback.RepliedAt = DateTime.Now;
                feedback.ReplyMessage = Message; // ✅ **جديد: حفظ نص الرد**  

                await _context.SaveChangesAsync(); // حفظ التغييرات في قاعدة البيانات

                // إرسال الرد بالبريد الإلكتروني
                await SendEmailReply(Email, Subject, Message);

                TempData["SuccessMessage"] = "تم إرسال الرد وحفظه في قاعدة البيانات بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إرسال الرد: " + ex.Message;
            }

            return RedirectToAction(nameof(feedback));
        }


        // Method to send email
        private async Task SendEmailReply(string toEmail, string subject, string message)
        {
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var password = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(senderEmail, password);

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(senderEmail, senderName);
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);
                }
            }
        }
        public IActionResult problemsandsolutions()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
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
        public IActionResult reports()
        {
            return View();
        }
    }
}
