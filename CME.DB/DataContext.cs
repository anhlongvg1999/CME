using CME.Entities;
using CME.Entities.Constants;
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
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<TrainingProgram_User> TrainingProgram_User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = Guid.Parse(Default.OrganizationId),
                    Name = Default.OrganizationName,
                    Code = Default.OrganizationCode,
                    Address = Default.OrganizationAddress,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<TrainingForm>().HasData(
                new TrainingForm
                {
                    Id = Guid.Parse(Default.TrainingFormId),
                    Name = Default.TrainingFormName,
                    Code = Default.TrainingFormCode,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<TrainingSubject>().HasData(
                new TrainingSubject
                {
                    Id = Guid.Parse(Default.TrainingSubjectId_Participant),
                    Name = Default.TrainingSubjectName_Participant,
                    Amount = Default.TrainingSubjectAmount_Participant,
                    TrainingFormId = Guid.Parse(Default.TrainingFormId),
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<TrainingSubject>().HasData(
                new TrainingSubject
                {
                    Id = Guid.Parse(Default.TrainingSubjectId_Owner),
                    Name = Default.TrainingSubjectName_Owner,
                    Amount = Default.TrainingSubjectAmount_Owner,
                    TrainingFormId = Guid.Parse(Default.TrainingFormId),
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

        }
    }
}
