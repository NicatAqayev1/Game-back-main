<<<<<<< HEAD
﻿using WebApplication1.Models;
=======
﻿
using WebApplication1.Models;
>>>>>>> 19e1c65 (18.11.2026)
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
<<<<<<< HEAD
=======
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

>>>>>>> 19e1c65 (18.11.2026)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

<<<<<<< HEAD
            // Slug sütununun unikal (təkrarsız) olmasını təmin edirik
            modelBuilder.Entity<Casino>()
                .HasIndex(c => c.Slug)
                .IsUnique();
            modelBuilder.Entity<PostbackLog>()
                .Property(p => p.Payout)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PostbackLog>()
                .HasIndex(p => new { p.CasinoId, p.TransactionId })
                .IsUnique()
                .HasFilter("[TransactionId] IS NOT NULL");
            modelBuilder.Entity<PostbackLog>()
                .HasIndex(p => new { p.TransactionId, p.CasinoId })
                .IsUnique()
                .HasFilter("[IsSuccessful] = 1 AND [TransactionId] IS NOT NULL");
            modelBuilder.Entity<PostbackLog>()
                .Property(p => p.RawData)
                .HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Casino>()
                .HasMany(c => c.ClickLogs)   
                .WithOne(l => l.Casino)           
                .HasForeignKey(l => l.CasinoId)    
                .OnDelete(DeleteBehavior.Cascade);
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
=======
            // Casino Slug - Unique
            modelBuilder.Entity<Casino>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            // Postback Payout
            modelBuilder.Entity<PostbackLog>()
                .Property(p => p.Payout)
                .HasColumnType("decimal(18,2)");

            // Postback Idempotency
            modelBuilder.Entity<PostbackLog>()
                .HasIndex(p => new { p.CasinoId, p.TransactionId, p.PostbackType })
                .IsUnique()
                .HasDatabaseName("IX_PostbackLogs_CasinoId_TransactionId_PostbackType")
                .HasFilter("[IsSuccessful] = 1 AND [TransactionId] IS NOT NULL");

            // RawData
            modelBuilder.Entity<PostbackLog>()
                .Property(p => p.RawData)
                .HasColumnType("nvarchar(max)");
            modelBuilder.Entity<PostbackLog>()
                .HasOne(p => p.ReversedPostbackLog)
                .WithMany()
                .HasForeignKey(p => p.ReversedPostbackLogId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostbackLog>()
                .HasIndex(p => p.ReversedPostbackLogId)
                .IsUnique()
                .HasDatabaseName("IX_PostbackLogs_ReversedPostbackLogId")
                .HasFilter("[ReversedPostbackLogId] IS NOT NULL AND [IsSuccessful] = 1");
            // ClickLog RowVersion
            modelBuilder.Entity<ClickLog>()
                .Property(c => c.RowVersion)
                .IsRowVersion();

            // Casino - ClickLogs relationship
            modelBuilder.Entity<Casino>()
                .HasMany(c => c.ClickLogs)
                .WithOne(l => l.Casino)
                .HasForeignKey(l => l.CasinoId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ConversionTransaction>()
                .HasIndex(t => new { t.CasinoId, t.TransactionId })
                .IsUnique()
                .HasDatabaseName("IX_ConversionTransactions_CasinoId_TransactionId")
                .HasFilter("[TransactionId] IS NOT NULL");

            modelBuilder.Entity<ConversionTransaction>()
                .HasOne(t => t.Casino)
                .WithMany(c => c.ConversionTransactions)
                .HasForeignKey(t => t.CasinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConversionTransaction>()
                .HasOne(t => t.ClickLog)
                .WithMany(c => c.ConversionTransactions)
                .HasForeignKey(t => t.ClickLogId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConversionTransaction>()
                .HasOne(t => t.RelatedConversionTransaction)
                .WithMany()
                .HasForeignKey(t => t.RelatedConversionTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
>>>>>>> 19e1c65 (18.11.2026)

        public DbSet<Casino> Casinos { get; set; }
        public DbSet<ClickLog> ClickLogs { get; set; }
        public DbSet<PostbackLog> PostbackLogs { get; set; }
<<<<<<< HEAD
=======
        public DbSet<ConversionTransaction> ConversionTransactions { get; set; }
>>>>>>> 19e1c65 (18.11.2026)
    }
}
