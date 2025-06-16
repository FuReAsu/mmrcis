using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mmrcis.Models;

namespace mmrcis.Data
{
    public class CisDbContext : IdentityDbContext<ApplicationUser>
    {
        public CisDbContext(DbContextOptions<CisDbContext> options)
            : base(options){}
        public DbSet<Person> Persons { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.PatientProfile)
                .WithOne(pat => pat.Person)
                .HasForeignKey<Patient>(pat => pat.PersonID)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(au => au.Person)
                .WithOne()
                .HasForeignKey<ApplicationUser>(au => au.PersonID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
