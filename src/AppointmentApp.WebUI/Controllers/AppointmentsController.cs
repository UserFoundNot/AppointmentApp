using AppointmentApp.Core.Entities;
using AppointmentApp.Core.Services;
using AppointmentApp.Infrastructure.Data;
using AppointmentApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentApp.WebUI.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;
        public AppointmentsController(AppDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _db.Appointments
                                 .Where(a => a.UserId == userId)
                                 .OrderBy(a => a.Date)
                                 .ToListAsync();
            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentViewModel vm)
        {
            if (vm.Date < DateTime.Now)
                ModelState.AddModelError(nameof(vm.Date), "Geçmiş tarihe randevu alınamaz.");

            if (!ModelState.IsValid) return View(vm);

            var apt = new Appointment
            {
                Date = vm.Date,
                Title = vm.Title,
                Description = vm.Description,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            _db.Appointments.Add(apt);
            await _db.SaveChangesAsync();
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _emailSender.SendEmailAsync(
                userEmail,
                "Randevu Onayınız",
                $"Merhaba, {apt.Date:g} tarihli randevunuz başarıyla alındı!");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var apt = await _db.Appointments.FindAsync(id);
            if (apt == null || apt.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();
            if (apt.Date < DateTime.Now)
                return BadRequest("Sadece gelecekteki randevular düzenlenebilir.");

            var vm = new AppointmentViewModel
            {
                Id = apt.Id,
                Date = apt.Date,
                Title = apt.Title,
                Description = apt.Description
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var apt = await _db.Appointments.FindAsync(vm.Id);
            if (apt == null || apt.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();
            if (apt.Date < DateTime.Now)
                return BadRequest("Sadece gelecekteki randevular düzenlenebilir.");

            apt.Date = vm.Date;
            apt.Title = vm.Title;
            apt.Description = vm.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var apt = await _db.Appointments.FindAsync(id);
            if (apt == null || apt.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return View(apt);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apt = await _db.Appointments.FindAsync(id);
            if (apt.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            _db.Appointments.Remove(apt);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
