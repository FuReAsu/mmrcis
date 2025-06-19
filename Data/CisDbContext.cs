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
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<PatientCheckInOut> PatientCheckInOuts { get; set; }
        public DbSet<PatientVisitRecord> PatientVisitRecords { get; set; }
        public DbSet<PatientVitals> PatientVitalss { get; set; }
        
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

            //Appointment Relationship with Person
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Person)
                .WithMany(p => p.AppointmentsAsAssignedToDoctor)
                .HasForeignKey(a => a.PersonID)
                .OnDelete(DeleteBehavior.Restrict);
            
            //Appointment Relationship with Patient
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            //PatientCheckInOut Relationship with IncomeBill
            modelBuilder.Entity<PatientCheckInOut>()
                .HasOne(pcio => pcio.IncomeBill)
                .WithOne(ib => ib.PatientCheckInOut)
                .HasForeignKey<IncomeBill>(ib => ib.PatientCheckInOutID)
                .OnDelete(DeleteBehavior.Restrict);
                
            //PatientCheckInOut Relationship with Patient
            modelBuilder.Entity<PatientCheckInOut>()
                .HasOne(pcio => pcio.Patient)
                .WithMany(p => p.PatientCheckInOuts)
                .HasForeignKey(pcio => pcio.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            //PatientCheckInOut Relationship with PatientVisitRecord
            modelBuilder.Entity<PatientCheckInOut>()
                .HasOne(pcio => pcio.PatientVisitRecord)
                .WithOne(pvr => pvr.PatientCheckInOut)
                .HasForeignKey<PatientVisitRecord>(pvr => pvr.PatientCheckInOutID)
                .OnDelete(DeleteBehavior.Cascade);
                
            //PatientCheckInOut Relationship with Appointment
            modelBuilder.Entity<PatientCheckInOut>()
                .HasOne(pcio => pcio.Appointment)
                .WithOne(a => a.PatientCheckInOut)
                .HasForeignKey<PatientCheckInOut>(pcio => pcio.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            //PatientVisitRecord Relationship with Patient
            modelBuilder.Entity<PatientVisitRecord>()
                .HasOne(pvr => pvr.Patient)
                .WithMany(p => p.PatientVisitRecord)
                .HasForeignKey(pvr => pvr.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            //PatientVisitRecord Relationship with Doctor(Person)
            modelBuilder.Entity<PatientVisitRecord>()
                .HasOne(pvr => pvr.Doctor)
                .WithMany(p => p.PatientVisitRecordAsReceivedByDoctor)
                .HasForeignKey(pvr => pvr.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            //PatientVisitRecord Relationship with PatientVitals
            modelBuilder.Entity<PatientVisitRecord>()
                .HasOne(pvr => pvr.PatientVitals)
                .WithOne(pv => pv.PatientVisitRecord)
                .HasForeignKey<PatientVitals>(pv => pv.PatientVisitRecordID)
                .OnDelete(DeleteBehavior.Cascade);

            //PatientVitals Relationship with MedicalStaff(Person)
            modelBuilder.Entity<PatientVitals>()
                .HasOne(pv => pv.MedicalStaff)
                .WithMany(ms => ms.PatientVitalsAsMedicalStaff )
                .HasForeignKey(pv => pv.MedicalStaffID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
