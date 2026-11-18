
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ConversionTransaction
    {
        public long Id { get; set; }

        // Əlaqəli klik
        public long? ClickLogId { get; set; }
        public virtual ClickLog? ClickLog { get; set; }

        // Əlaqəli kazino
        public int CasinoId { get; set; }
        public virtual Casino? Casino { get; set; }

        // Kazinonun göndərdiyi əməliyyat ID-si
        [MaxLength(255)]
        public string? TransactionId { get; set; }

        // Ləğv edilən ilkin əməliyyatın xarici ID-si
        [MaxLength(255)]
        public string? RelatedTransactionId { get; set; }

        // Bizim bazadakı ilkin əməliyyatın daxili ID-si
        public long? RelatedConversionTransactionId { get; set; }
        public virtual ConversionTransaction? RelatedConversionTransaction { get; set; }

        // Kazinonun oyunçu ID-si
        [MaxLength(255)]
        public string? ExternalPlayerId { get; set; }

        // reg, ftd, ftd_cancel
        [Required]
        [MaxLength(30)]
        public string Type { get; set; } = string.Empty;

        // Depozit məbləğinin dəyişməsi
        [Precision(18, 2)]
        public decimal? DepositAmount { get; set; }

        // Affiliate komissiyasının dəyişməsi
        [Precision(18, 2)]
        public decimal? CommissionAmount { get; set; }

        // Məbləğlərin valyutaları
        [MaxLength(3)]
        public string? DepositCurrency { get; set; }

        [MaxLength(3)]
        public string? CommissionCurrency { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
