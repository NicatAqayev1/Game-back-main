using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTOs
{
    public class CreateCasinoDto
    {
        [Required(ErrorMessage = "Casino name is required.")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required.")]
        [MaxLength(100, ErrorMessage = "Slug cannot exceed 100 characters.")]
        public string Slug { get; set; } = string.Empty;
        public string LogoText { get; set; } = string.Empty;
        public string LogoColor { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        [Precision(3, 1)]
        public decimal Rating { get; set; } = 0.0m;

        public string BonusTitle { get; set; } = string.Empty;
        public string BonusValue { get; set; } = string.Empty;
        public string? PromoCode { get; set; }

        [Required(ErrorMessage = "Affiliate URL is required.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string AffiliateUrl { get; set; } = string.Empty; // Modelinizdəki ad ilə eyniləşdirildi

        public string Categories { get; set; } = string.Empty;
        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true; // Adətən yeni yaradılan aktiv olaraq başlaya bilər
        public string? AllowedIPs { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
