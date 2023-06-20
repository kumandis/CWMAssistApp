using CWMAssistApp.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CWMAssistApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Packet> Packets { get; set; }
        public DbSet<Personal> Personals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<CustomerAppointment> CustomerAppointments { get; set; }
        public DbSet<CustomerPacket> CustomerPackets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SmsPacket> SmsPackets { get; set; }
        public DbSet<UserSmsPacket> UserSmsPackets { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AlarmEntity> Alarms { get; set; }
    }
}