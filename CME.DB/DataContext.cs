using CME.Entities;
using CME.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using System;

namespace SERP.Filenet.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<FileCSV> FileCSVs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
