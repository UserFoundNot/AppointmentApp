using AppointmentApp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Infrastructure.Data
{
    /// <summary>
    /// Uygulamanın EF Core DbContext’i. 
    /// Identity tablolarını da içinde barındırır.
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Appointment> Appointments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Appointment ↔ User ilişkisini kurar
            builder.Entity<Appointment>()
                   .HasOne(a => a.User)
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
