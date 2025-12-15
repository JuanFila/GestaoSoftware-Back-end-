using Microsoft.EntityFrameworkCore;
using GestaoSoftware.Models;
using GestaoSoftware.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GestaoSoftware.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwareVersion> Versions { get; set; }

        public DbSet<SoftwareStatusHistory> SoftwareStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var statusConverter = new ValueConverter<SoftwareStatus, string>(
                v => v.ToString(), 
                v => (SoftwareStatus)Enum.Parse(typeof(SoftwareStatus), v)
            );

            modelBuilder.Entity<Software>()
                .Property(s => s.Status)
                .HasConversion(statusConverter)
                .HasMaxLength(20);
        }
    }
}
