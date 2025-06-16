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
            
            //Person Relationship with Patient
            modelBuilder.Entity<Person>()
                .HasOne(p => p.PatientProfile)
                .WithOne(pat => pat.Person)
                .HasForeignKey<Patient>(pat => pat.PersonID)
                .OnDelete(DeleteBehavior.Cascade);
            
            //Person Relationship with ApplicationUser  
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(au => au.Person)
                .WithOne()
                .HasForeignKey<ApplicationUser>(au => au.PersonID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            //Person Relationship with AuditLog
            modelBuilder.Entity<Person>()
                .HasMany(p => p.AuditLogEntries)
                .WithOne(al => al.Person)
                .HasForeignKey(al => al.PersonID);

        }
    }
}
