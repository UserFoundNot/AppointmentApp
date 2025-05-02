using AppointmentApp.Core.Entities;
using AppointmentApp.Infrastructure.Data;
using AppointmentApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace AppointmentApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminAppointmentsController : Controller
    {
        private readonly AppDbContext _db;
        public AdminAppointmentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.Appointments
                                .Include(a => a.User)
                                .OrderBy(a => a.Date)
                                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var apt = await _db.Appointments.FindAsync(id);
            if (apt == null) return NotFound();

            var vm = new AppointmentViewModel
            {
                Id = apt.Id,
                Date = apt.Date,
                Title = apt.Title,
                Description = apt.Description
            };
            ViewData["UserEmail"] = apt.User?.Email;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var apt = await _db.Appointments.FindAsync(vm.Id);
            if (apt == null) return NotFound();

            apt.Date = vm.Date;
            apt.Title = vm.Title;
            apt.Description = vm.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var apt = await _db.Appointments.FindAsync(id);
            if (apt == null) return NotFound();
            ViewData["UserEmail"] = apt.User?.Email;
            return View(apt);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apt = await _db.Appointments.FindAsync(id);
            if (apt == null) return NotFound();

            _db.Appointments.Remove(apt);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
