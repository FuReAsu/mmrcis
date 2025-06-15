// Data/CisDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mmrcis.Models;

namespace mmrcis.Data
{
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
        public DbSet<ExpenseBill> ExpenseBills { get; set; }
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
        public DbSet<Patient> Patients { get; set; } // Ensure Patient DbSet is present
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<IncomeBill> IncomeBills { get; set; } // ADDED: DbSet for IncomeBill
        public DbSet<IncomeBillItem> IncomeBillItems { get; set; } // ADDED: DbSet for IncomeBillItem
        public DbSet<Payment> Payments { get; set; } // ADDED: Assuming you'll have a Payment model. If not, remove.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Relationships and Constraints ---

            // Person to Patient (One-to-one relationship)
            modelBuilder.Entity<Person>()
                .HasOne(p => p.PatientProfile) // Person has one PatientProfile
                .WithOne(pat => pat.Person)    // PatientProfile has one Person
                .HasForeignKey<Patient>(pat => pat.PersonID) // Patient.PersonID is the FK
                .OnDelete(DeleteBehavior.Cascade); // If Person is deleted, PatientProfile is also deleted

            // ApplicationUser to Person (FK: PersonID in ApplicationUser)
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(au => au.Person)
                .WithOne() // ApplicationUser has one Person
                .HasForeignKey<ApplicationUser>(au => au.PersonID)
                .IsRequired(false) // PersonID in ApplicationUser is nullable
                .OnDelete(DeleteBehavior.SetNull); // Set FK to NULL if Person is deleted

            // --- Person Role-Based Relationships ---

            // Person as an Operator who created an IncomeBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.IncomeBillsAsCreatedByOperator)
                .WithOne(ib => ib.Operator)
                .HasForeignKey(ib => ib.OperatorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Operator who received a Payment
            modelBuilder.Entity<Person>()
                .HasMany(p => p.PaymentsAsReceivedByOperator)
                .WithOne(pay => pay.ReceivedByOperator)
                .HasForeignKey(pay => pay.ReceivedByOperatorID)
                .OnDelete(DeleteBehavior.NoAction);

            // Person as Operator in ExpenseBill
            modelBuilder.Entity<Person>()
                .HasMany(p => p.ExpenseBillsAsOperator)
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

            // --- Patient (Model) Specific Relationships ---

            // Patient to Appointment
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient to IncomeBill
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.IncomeBills)
                .WithOne(ib => ib.Patient)
                .HasForeignKey(ib => ib.PatientID)
                .IsRequired(false) // PatientID in IncomeBill can be null
                .OnDelete(DeleteBehavior.NoAction);

            // Patient to ExpenseBill
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.ExpenseBills)
                .WithOne(eb => eb.Patient)
                .HasForeignKey(eb => eb.PatientID)
                .IsRequired(false) // PatientID in ExpenseBill can be null
                .OnDelete(DeleteBehavior.NoAction);

            // Patient to PatientDocument
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.PatientDocuments)
                .WithOne(pd => pd.Patient)
                .HasForeignKey(pd => pd.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient to PatientLabRecord
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.PatientLabRecords)
                .WithOne(plr => plr.Patient)
                .HasForeignKey(plr => plr.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient to PatientVital
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.PatientVitals)
                .WithOne(pv => pv.Patient)
                .HasForeignKey(pv => pv.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient to PatientCheckinOut
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.PatientCheckinOuts)
                .WithOne(pco => pco.Patient)
                .HasForeignKey(pco => pco.PatientID)
                .OnDelete(DeleteBehavior.NoAction);


            // --- IncomeBill and IncomeBillItem Relationships ---

            // IncomeBill to IncomeBillItems (One-to-Many)
            modelBuilder.Entity<IncomeBill>()
                .HasMany(ib => ib.IncomeBillItems)
                .WithOne(ibi => ibi.IncomeBill)
                .HasForeignKey(ibi => ibi.IncomeBillID)
                .OnDelete(DeleteBehavior.Cascade);

            // IncomeBill to PatientLabRecords (One-to-Many)
            modelBuilder.Entity<IncomeBill>()
                .HasMany(ib => ib.PatientLabRecords)
                .WithOne(plr => plr.IncomeBill)
                .HasForeignKey(plr => plr.IncomeBillID)
                .OnDelete(DeleteBehavior.NoAction);

            // IncomeBill to Payments (One-to-Many)
            modelBuilder.Entity<IncomeBill>()
                .HasMany(ib => ib.Payments)
                .WithOne(p => p.IncomeBill)
                .HasForeignKey(p => p.IncomeBillID)
                .OnDelete(DeleteBehavior.Cascade); // Assuming payments cascade with bill deletion

            // IncomeBillItem to CostRate (FK to CostRate.CostCode)
            modelBuilder.Entity<IncomeBillItem>()
                .HasOne(ibi => ibi.CostRate)
                .WithMany(cr => cr.IncomeBillItems)
                .HasPrincipalKey(cr => cr.CostCode) // Specify CostCode as the principal key
                .HasForeignKey(ibi => ibi.CostCode)
                .OnDelete(DeleteBehavior.NoAction);


            // --- Other existing relationships ---

            // Appointment to Service
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceID)
                .OnDelete(DeleteBehavior.NoAction);

            // ExpenseBill to ExpenseBillItems (One-to-Many)
            modelBuilder.Entity<ExpenseBill>()
                .HasMany(eb => eb.ExpenseBillItems)
                .WithOne(ebi => ebi.ExpenseBill)
                .HasForeignKey(ebi => ebi.ExpenseBillID)
                .OnDelete(DeleteBehavior.Cascade);

            // ExpenseBillItem to CostRate (FK to CostRate.CostCode)
            modelBuilder.Entity<ExpenseBillItem>()
                .HasOne(ebi => ebi.CostRate)
                .WithMany(cr => cr.ExpenseBillItems)
                .HasPrincipalKey(cr => cr.CostCode) // Specify CostCode as the principal key
                .HasForeignKey(ebi => ebi.CostCode)
                .OnDelete(DeleteBehavior.NoAction);

            // PostingTransaction referencing CostRate.AccountCode
            modelBuilder.Entity<PostingTransaction>()
                .HasOne(pt => pt.CostRate)
                .WithMany(cr => cr.PostingTransactions)
                .HasPrincipalKey(cr => cr.AccountCode) // Specify AccountCode as the principal key
                .HasForeignKey(pt => pt.AccountCode)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // ClinicDocument to PostingTransaction
            modelBuilder.Entity<ClinicDocument>()
                .HasOne(cd => cd.PostingTransaction)
                .WithMany(pt => pt.ClinicDocuments)
                .HasForeignKey(cd => cd.TransactionID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Supplier)
                .WithMany() 
                .HasForeignKey(ii => ii.SupplierID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); 
            
            modelBuilder.Entity<PatientCheckinOut>() 
                .HasOne(pco => pco.PatientVital)    
                .WithMany()                         
                .HasForeignKey(pco => pco.PatientVitalID)
                .IsRequired(false)                  
                .OnDelete(DeleteBehavior.SetNull);  
            
            // --- Unique Constraints ---
            modelBuilder.Entity<CostRate>()
                .HasIndex(cr => cr.CostCode)
                .IsUnique();

            modelBuilder.Entity<CostRate>()
                .HasIndex(cr => cr.AccountCode)
                .IsUnique()
                .HasFilter("[AccountCode] IS NOT NULL"); // Ensures uniqueness only for non-null values


            // --- Default Values (from your original schema) ---
            modelBuilder.Entity<ExpenseBill>()
                .Property(eb => eb.IsVoided)
                .HasDefaultValue(false);

            modelBuilder.Entity<PatientCheckinOut>()
                .Property(pco => pco.IsBilled)
                .HasDefaultValue(false);
        }
    }
}
