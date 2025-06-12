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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: Call the base method first for Identity configurations
            base.OnModelCreating(modelBuilder);

            // --- Relationships and Constraints ---

            // ApplicationUser to Person (One-to-One with Person as the principal, or One-to-Many from Person to ApplicationUser)
            // Assuming one Person can be associated with AT MOST one ApplicationUser (as a system login account).
            // A Person doesn't necessarily need a linked ApplicationUser (e.g., a patient who doesn't log in).
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(au => au.Person) // An ApplicationUser has one Person
                .WithOne()               // This Person has one ApplicationUser (implicitly, as no specific navigation property on Person side for this)
                .HasForeignKey<ApplicationUser>(au => au.PersonID) // FK is on ApplicationUser
                .IsRequired(false)       // PersonID in ApplicationUser is nullable
                .OnDelete(DeleteBehavior.SetNull); // If a Person record is deleted, set PersonID in ApplicationUser to null

            // --- Person Relationships (Person as various roles in transactions) ---

            // Person as Patient in IncomeBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.IncomeBillsAsPatient)
                .WithOne(ib => ib.Patient)
                .HasForeignKey(ib => ib.PatientID)
                .OnDelete(DeleteBehavior.NoAction); // Prevent deleting a Person if they have associated income bills

            // Person as Operator in IncomeBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.IncomeBillsAsOperator) // Ensure this navigation property exists in Person.cs
                .WithOne(ib => ib.Operator)
                .HasForeignKey(ib => ib.OperatorID)
                .OnDelete(DeleteBehavior.NoAction); // Prevent deleting an Operator if they've handled income bills

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
                .OnDelete(DeleteBehavior.NoAction); // Or .Cascade based on your specific business rule

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
                .OnDelete(DeleteBehavior.NoAction); // Or .Cascade

            // --- Default Values (as per schema improvements) ---
            modelBuilder.Entity<IncomeBill>()
                .Property(ib => ib.IsVoided)
                .HasDefaultValue(false); // BIT DEFAULT 0

            modelBuilder.Entity<ExpenseBill>()
                .Property(eb => eb.IsVoided)
                .HasDefaultValue(false); // BIT DEFAULT 0

            modelBuilder.Entity<PatientCheckinOut>()
                .Property(pco => pco.IsBilled)
                .HasDefaultValue(false); // BIT DEFAULT 0


            // --- Other Specific Configurations ---

            // ClinicDocument.TransactionID
            modelBuilder.Entity<ClinicDocument>()
                .HasOne(cd => cd.PostingTransaction)
                .WithMany(pt => pt.ClinicDocuments)
                .HasForeignKey(cd => cd.TransactionID)
                .IsRequired(false) // TransactionID is nullable
                .OnDelete(DeleteBehavior.NoAction); // Assuming you don't delete documents when transaction is deleted


            // For tables without a Primary Key in your SQL schema (TicketHeader, TicketFooter)
            // If you added an 'ID' property to the model, EF Core will treat it as PK by convention.
            // If they truly have no PK and are just simple data containers, you would use:
            // modelBuilder.Entity<TicketHeader>().HasNoKey();
            // modelBuilder.Entity<TicketFooter>().HasNoKey();
            // However, your C# models for these now include an 'ID' so this is not needed.
            // EF Core will create an ID column for these tables as auto-incrementing PKs.

        }
    }
}
