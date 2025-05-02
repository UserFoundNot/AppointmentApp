using AppointmentApp.Core.Entities;
using AppointmentApp.Core.Services;
using AppointmentApp.Infrastructure.Data;
using AppointmentApp.WebUI.Controllers;
using AppointmentApp.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AppointmentApp.Tests
{
    public class AppointmentsControllerTests
    {
        private class FakeEmailSender : IEmailSender
        {
            public Task SendEmailAsync(string to, string subject, string body)
                => Task.CompletedTask;
        }
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private AppointmentsController GetController(AppDbContext db, string userId)
        {
            var emailSender = new FakeEmailSender();
            var controller = new AppointmentsController(db, emailSender);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuth"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        [Fact]
        public async Task Create_WithValidModel_AddsAppointment()
        {
            var db = GetDbContext();
            var controller = GetController(db, "user1");
            var vm = new AppointmentViewModel
            {
                Date = DateTime.Now.AddDays(1),
                Title = "Test Appointment",
                Description = "Test Description"
            };

            var result = await controller.Create(vm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Single(db.Appointments);
            var apt = db.Appointments.First();
            Assert.Equal("user1", apt.UserId);
            Assert.Equal(vm.Title, apt.Title);
        }

        [Fact]
        public async Task Edit_PastAppointment_ReturnsBadRequest()
        {
            var db = GetDbContext();
            var userId = "user1";
            var pastApt = new Appointment
            {
                Date = DateTime.Now.AddDays(-1),
                Title = "Past",
                Description = "",
                UserId = userId
            };
            db.Appointments.Add(pastApt);
            await db.SaveChangesAsync();

            var controller = GetController(db, userId);

            var result = await controller.Edit(pastApt.Id);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Sadece gelecekteki randevular düzenlenebilir.", bad.Value);
        }

        [Fact]
        public async Task Edit_OtherUserAppointment_ReturnsForbid()
        {
            var db = GetDbContext();
            var apt = new Appointment
            {
                Date = DateTime.Now.AddDays(1),
                Title = "Future",
                Description = "",
                UserId = "user1"
            };
            db.Appointments.Add(apt);
            await db.SaveChangesAsync();

            var controller = GetController(db, "user2");

            var result = await controller.Edit(apt.Id);

            Assert.IsType<ForbidResult>(result);
        }
    }
}
