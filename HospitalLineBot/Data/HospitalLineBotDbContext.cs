using HospitalLineBot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HospitalLineBot.Data
{
    public class HospitalLineBotDbContext : DbContext
    {


        public HospitalLineBotDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }


        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Hospital> Hospital { get; set; }

    }

}
