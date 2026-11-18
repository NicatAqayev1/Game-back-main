using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTOs
{
    public class CasinoListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string LogoText { get; set; } = string.Empty;
        public string LogoColor { get; set; } = string.Empty;
        public decimal Rating { get; set; } = 0.0m;
        public string BonusTitle { get; set; } = string.Empty;
        public string BonusValue { get; set; } = string.Empty;
        public string? PromoCode { get; set; }
        public string AffiliateUrl { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string? AllowedIPs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
