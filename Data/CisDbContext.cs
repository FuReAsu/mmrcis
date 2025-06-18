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
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<IncomeBill> IncomeBills { get; set; }
        public DbSet<IncomeBillItem> IncomeBillItems { get; set; }
        public DbSet<CostRate> CostRates { get; set; }

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

            //IncomeBill Relationship with PersonAsOperator
            modelBuilder.Entity<IncomeBill>()
                .HasOne(ib => ib.Person)
                .WithMany(p => p.IncomeBillsAsCreatedByOperator)
                .HasForeignKey(ib => ib.PersonID)
                .OnDelete(DeleteBehavior.Restrict);

            //IncomeBill Relationship with Patient
            modelBuilder.Entity<IncomeBill>()
                .HasOne(ib => ib.Patient)
                .WithMany(p => p.IncomeBills)
                .HasForeignKey(ib => ib.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            //IncomeBillItem Relationship with IncomeBill
            modelBuilder.Entity<IncomeBillItem>()
                .HasOne(ibi => ibi.IncomeBill)
                .WithMany(ib => ib.IncomeBillItems)
                .HasForeignKey(ibi => ibi.IncomeBillID)
                .OnDelete(DeleteBehavior.Cascade);

            //IncomeBillItem Relationship with CostRate
            modelBuilder.Entity<IncomeBillItem>()
                .HasOne(ibi => ibi.CostRate)
                .WithMany(cr => cr.IncomeBillItems)
                .HasForeignKey(ibi => ibi.CostRateID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
