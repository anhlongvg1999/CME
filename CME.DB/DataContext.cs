using CME.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace SERP.Filenet.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TrainingForm> TrainingForms { get; set; }
        public DbSet<TrainingSubject> TrainingSubjects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "BV Xanh Pôn",
                    Code = "BVXP",
                    Address = "12 Chu Văn An, Điện Bàn, Ba Đình, Hà Nội",
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );
        }
    }
}
