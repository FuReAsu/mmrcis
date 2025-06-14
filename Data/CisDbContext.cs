// Data/CisDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mmrcis.Models; // Ensure this matches your project's Models namespace
using System.Reflection.Emit; // Not strictly needed, but common to have system usings

namespace mmrcis.Data // Ensure this matches your project's Data namespace
{
    // Inherit from IdentityDbContext to include all standard ASP.NET Core Identity tables
    // (AspNetUsers, AspNetRoles, AspNetUserClaims, etc.)
    public class CisDbContext : IdentityDbContext<ApplicationUser>
    {
        public CisDbContext(DbContextOptions<CisDbContext> options)
            : base(options)
        {
        }

        // --- DbSets for your custom application models ---
        public DbSet<Person> Persons { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<CostRate> CostRates { get; set; }
        public DbSet<IncomeBill> IncomeBills { get; set; }
        public DbSet<ExpenseBill> ExpenseBills { get; set; }
        public DbSet<IncomeBillItem> IncomeBillItems { get; set; }
        public DbSet<ExpenseBillItem> ExpenseBillItems { get; set; }
        public DbSet<PostingTransaction> PostingTransactions { get; set; }
        public DbSet<ClinicDocument> ClinicDocuments { get; set; }
        public DbSet<PatientDocument> PatientDocuments { get; set; }
        public DbSet<PatientLabRecord> PatientLabRecords { get; set; }
        public DbSet<PatientVital> PatientVitals { get; set; }
        public DbSet<PatientCheckinOut> PatientCheckinOuts { get; set; }
        public DbSet<TicketHeader> TicketHeaders { get; set; }
        public DbSet<TicketFooter> TicketFooters { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Relationships and Constraints ---

            // Appointment to Patient (FK: PatientID)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Restrict

            // Appointment to Doctor/Staff (Person) (FK: DoctorStaffID)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.DoctorStaff)
                .WithMany()
                .HasForeignKey(a => a.DoctorStaffID)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Restrict

            // Patient to Person (FK: PersonID in Patient)
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Person)
                .WithOne() // Assuming a 1-to-1 relationship between Patient and Person
                .HasForeignKey<Patient>(p => p.PersonID)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Restrict

            // ApplicationUser to Person (FK: PersonID in ApplicationUser)
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(au => au.Person)
                .WithOne()
                .HasForeignKey<ApplicationUser>(au => au.PersonID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // SetNull is fine, doesn't cause cycle errors

            // --- Person Relationships (Person as various roles in transactions) ---
            // All these were already NoAction, which is good.

            // Person as Patient in IncomeBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.IncomeBillsAsPatient)
                .WithOne(ib => ib.Patient)
                .HasForeignKey(ib => ib.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Operator in IncomeBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.IncomeBillsAsOperator) // Ensure this navigation property exists in Person.cs
                .WithOne(ib => ib.Operator)
                .HasForeignKey(ib => ib.OperatorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Patient in ExpenseBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.ExpenseBillsAsPatient)
                .WithOne(eb => eb.Patient)
                .HasForeignKey(eb => eb.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Operator in ExpenseBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.ExpenseBillsAsOperator) // Ensure this navigation property exists in Person.cs
                .WithOne(eb => eb.Operator)
                .HasForeignKey(eb => eb.OperatorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Doctor in PatientLabRecord
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PatientLabRecordsAsDoctor)
                .WithOne(plr => plr.Doctor)
                .HasForeignKey(plr => plr.DoctorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Operator in PatientVital
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PatientVitalsAsOperator)
                .WithOne(pv => pv.Operator)
                .HasForeignKey(pv => pv.OperatorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Patient in PatientCheckinOut
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PatientCheckinOutsAsPatient)
                .WithOne(pco => pco.Patient)
                .HasForeignKey(pco => pco.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Doctor in PatientCheckinOut
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PatientCheckinOutsAsDoctor)
                .WithOne(pco => pco.Doctor)
                .HasForeignKey(pco => pco.DoctorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Operator in PostingTransaction
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PostingTransactionsAsOperator)
                .WithOne(pt => pt.Operator)
                .HasForeignKey(pt => pt.OperatorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as CheckedPerson in PostingTransaction
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PostingTransactionsAsCheckedPerson)
                .WithOne(pt => pt.CheckedPerson)
                .HasForeignKey(pt => pt.CheckedPersonID)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Unique Constraints (from your original schema, fixed for unique indexes) ---
            modelBuilder.Entity<CostRate>()
                .HasIndex(cr => cr.CostCode)
                .IsUnique();

            modelBuilder.Entity<CostRate>()
                .HasIndex(cr => cr.AccountCode)
                .IsUnique()
                .HasFilter("[AccountCode] IS NOT NULL"); // IMPORTANT for nullable unique columns in SQL Server

            // --- Foreign Key to non-Primary Key columns (e.g., CostCode and AccountCode) ---

            // IncomeBillItem referencing CostRate.CostCode
            modelBuilder.Entity<IncomeBillItem>()
                .HasOne(ibi => ibi.CostRate)
                .WithMany(cr => cr.IncomeBillItems) // Ensure IncomeBillItems collection exists in CostRate
                .HasPrincipalKey(cr => cr.CostCode) // The unique key on CostRate that is referenced
                .HasForeignKey(ibi => ibi.CostCode)
                .OnDelete(DeleteBehavior.NoAction);

            // ExpenseBillItem referencing CostRate.CostCode
            modelBuilder.Entity<ExpenseBillItem>()
                .HasOne(ebi => ebi.CostRate)
                .WithMany(cr => cr.ExpenseBillItems) // Ensure ExpenseBillItems collection exists in CostRate
                .HasPrincipalKey(cr => cr.CostCode)
                .HasForeignKey(ebi => ebi.CostCode)
                .OnDelete(DeleteBehavior.NoAction);

            // PostingTransaction referencing CostRate.AccountCode
            modelBuilder.Entity<PostingTransaction>()
                .HasOne(pt => pt.CostRate)
                .WithMany(cr => cr.PostingTransactions) // Ensure PostingTransactions collection exists in CostRate
                .HasPrincipalKey(cr => cr.AccountCode)
                .HasForeignKey(pt => pt.AccountCode)
                .IsRequired(false) // As AccountCode is nullable in PostingTransaction
                .OnDelete(DeleteBehavior.NoAction);

            // --- Default Values (as per schema improvements) ---
            modelBuilder.Entity<IncomeBill>()
                .Property(ib => ib.IsVoided)
                .HasDefaultValue(false);

            modelBuilder.Entity<ExpenseBill>()
                .Property(eb => eb.IsVoided)
                .HasDefaultValue(false);

            modelBuilder.Entity<PatientCheckinOut>()
                .Property(pco => pco.IsBilled)
                .HasDefaultValue(false);

            // --- Other Specific Configurations ---

            // ClinicDocument.TransactionID
            modelBuilder.Entity<ClinicDocument>()
                .HasOne(cd => cd.PostingTransaction)
                .WithMany(pt => pt.ClinicDocuments)
                .HasForeignKey(cd => cd.TransactionID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
