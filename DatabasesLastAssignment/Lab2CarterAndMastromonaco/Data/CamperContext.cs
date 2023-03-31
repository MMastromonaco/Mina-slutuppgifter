using Lab2CarterAndMastromonaco.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CarterAndMastromonaco.Data
{
    public class CamperContext : DbContext
    {
        public CamperContext()
        {

        }
        public CamperContext(DbContextOptions<CamperContext> options) : base(options)
        {

        }

        public DbSet<Camper> Campers { get; set; }
        public DbSet<Counselor> Counselors { get; set; }
        public DbSet<NextOfKin> NextOfKins { get; set; }
        public DbSet<Cabin> Cabins { get; set; }
        public DbSet<PastCamperAndCounselor> PastCamperAndCounselors { get; set; }

        private readonly string connectionMartin = @"Server=localhost, 1433\SQLEXPRESS;Database=CampSleepaway;User Id=SA;Password=XenoYamaha1;Trusted_Connection=True; Integrated Security=false;TrustServerCertificate=true;MultipleActiveResultSets=true";
        private readonly string connectionPaul = @"Server=LAPTOP-5PA0JF9C\SQLEXPRESS;Database=CampSleepaway;Trusted_Connection=True; Integrated Security=true;TrustServerCertificate=true;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(connectionPaul);
            optionsBuilder.UseSqlServer(connectionMartin);
        }
    }
}