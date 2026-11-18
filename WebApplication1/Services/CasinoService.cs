using WebApplication1.Data; // AppDbContext namespace-iniz
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApplication1.Common;
using WebApplication1.Models.DTOs;

public class CasinoService : ICasinoService
{
    private readonly AppDbContext _context;

    public CasinoService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ServiceResult> CreateCasinoAsync(CreateCasinoDto dto)
    {
        // 1. Slug boş gəlibsə Kazino Adından (Name) yaradırıq, doludursa təmizləyirik
        if (string.IsNullOrWhiteSpace(dto.Slug))
        {
            dto.Slug = Slugify(dto.Name);
        }
        else
        {
            dto.Slug = Slugify(dto.Slug);
        }

        // 2. Eyni slug-ın bazada olub-olmadığını yoxlayırıq (Unikallıq qorunması)
        bool slugExists = await _context.Casinos.AnyAsync(c => c.Slug == dto.Slug);
        if (slugExists)
        {
            return ServiceResult.Error("Bu slug (url) artıq mövcuddur. Başqa slug seçin.");
        }

        // 3. DTO-nu Entity-yə (`Casino`) map edirik (köçürürük)
        var casino = new Casino
        {
            Name = dto.Name,
            Slug = dto.Slug,
            LogoText = dto.LogoText,
            LogoColor = dto.LogoColor,
            Rating = dto.Rating,
            BonusTitle = dto.BonusTitle,
            BonusValue = dto.BonusValue,
            PromoCode = dto.PromoCode,
            AffiliateUrl = dto.AffiliateUrl,
            Categories = dto.Categories,
            IsFeatured = dto.IsFeatured,
            IsActive = dto.IsActive,
            AllowedIPs = dto.AllowedIPs,
            CreatedAt = DateTime.UtcNow // Yaradılma tarixi
        };

        // 4. Bazaya əlavə edib yadda saxlayırıq
        await _context.Casinos.AddAsync(casino);
        await _context.SaveChangesAsync();

        return ServiceResult.Success($"'{casino.Name}' kazinosu uğurla yaradıldı.");
    }
    public async Task<ServiceResult> UpdateCasinoAsync(UpdateCasinoDto dto)
    {
        var casino = await _context.Casinos.FindAsync(dto.Id);
        if (casino == null)
        {
            return ServiceResult.Error("Redaktə ediləcək kazino tapılmadı.");
        }

        // Slug-ı təmizləyirik
        string cleanSlug = string.Empty;
        if (string.IsNullOrWhiteSpace(dto.Slug))
        {
            cleanSlug = Slugify(dto.Name);
        }
        else
        {
            cleanSlug = Slugify(dto.Slug);
        }
        // XƏBƏRDARLIQ: Eyni slug-ın başqa kazinoda olub- olmadığını yoxlayırıq 
        // (öz ID-si xaric!)
        bool slugExists = await _context.Casinos
            .AnyAsync(c => c.Slug == dto.Slug && c.Id != dto.Id);

        if (slugExists)
        {
            return ServiceResult.Error("Bu slug başqa kazino tərəfindən istifadə olunur.");
        }

        // Məlumatları yeniləyirik
        casino.Name = dto.Name.Trim();
        casino.Slug = cleanSlug;
        casino.LogoText = dto.LogoText.Trim();
        casino.LogoColor = dto.LogoColor.Trim();
        casino.Rating = dto.Rating;
        casino.BonusTitle = dto.BonusTitle.Trim();
        casino.BonusValue = dto.BonusValue.Trim();
        casino.PromoCode = dto.PromoCode?.Trim();
        casino.AffiliateUrl = dto.AffiliateUrl.Trim();
        casino.Categories = dto.Categories.Trim();
        casino.IsFeatured = dto.IsFeatured;
        casino.IsActive = dto.IsActive;
        casino.AllowedIPs = dto.AllowedIPs?.Trim();
        casino.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ServiceResult.Success($"'{casino.Name}' kazinosu uğurla yeniləndi.");
    }
    public async Task<ServiceResult> ToggleStatusAsync(int id)
    {
        var casino = await _context.Casinos.FindAsync(id);
        if (casino == null)
        {
            return ServiceResult.Error("Kazino tapılmadı.");
        }

        // Statusu tərsinə çevririk
        casino.IsActive = !casino.IsActive;

        await _context.SaveChangesAsync();

        string statusText = casino.IsActive ? "aktivləşdirildi" : "deaktiv edildi";
        return ServiceResult.Success($"Kazino uğurla {statusText}.");
    }
    public async Task<ServiceResult> DeleteCasinoAsync(int id)
    {
        // 1. Kazinonu bazadan tapırıq
        var casino = await _context.Casinos.FindAsync(id);

        if (casino == null)
        {
            return ServiceResult.Error("Silinəcək kazino tapılmadı.");
        }

        // 2. Bazadan silirik
        _context.Casinos.Remove(casino);
        await _context.SaveChangesAsync();

        return ServiceResult.Success($"'{casino.Name}' kazinosu uğurla silindi.");
    }

    public async Task<List<CasinoListDto>> GetAllCasinosAsync()
    {
        return await _context.Casinos
            .Select(c => new CasinoListDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                LogoText = c.LogoText,
                LogoColor = c.LogoColor,
                Rating = c.Rating,
                BonusTitle = c.BonusTitle,
                BonusValue = c.BonusValue,
                PromoCode = c.PromoCode,
                AffiliateUrl = c.AffiliateUrl,
                Categories = c.Categories,
                IsFeatured = c.IsFeatured,
                IsActive = c.IsActive,
                AllowedIPs = c.AllowedIPs,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();
    }
    public async Task<CasinoListDto?> GetCasinoBySlugAsync(string slug)
    {
        string cleanSlug = Slugify(slug);

        return await _context.Casinos
            .AsNoTracking()
            .Where(c => c.Slug == cleanSlug)
            .Select(c => new CasinoListDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                LogoText = c.LogoText,
                LogoColor = c.LogoColor,
                Rating = c.Rating,
                BonusTitle = c.BonusTitle,
                BonusValue = c.BonusValue,
                PromoCode = c.PromoCode,
                AffiliateUrl = c.AffiliateUrl,
                Categories = c.Categories,
                IsFeatured = c.IsFeatured,
                IsActive = c.IsActive,
                AllowedIPs = c.AllowedIPs,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
    public async Task<UpdateCasinoDto?> GetCasinoForEditAsync(int id)
    {
        var casino = await _context.Casinos.FindAsync(id);
        if (casino == null) return null;

        return new UpdateCasinoDto
        {
            Id = casino.Id,
            Name = casino.Name,
            Slug = casino.Slug,
            LogoText = casino.LogoText,
            LogoColor = casino.LogoColor,
            Rating = casino.Rating,
            BonusTitle = casino.BonusTitle,
            BonusValue = casino.BonusValue,
            PromoCode = casino.PromoCode,
            AffiliateUrl = casino.AffiliateUrl,
            Categories = casino.Categories,
            IsFeatured = casino.IsFeatured,
            IsActive = casino.IsActive,
            AllowedIPs = casino.AllowedIPs,
            UpdatedAt = casino.UpdatedAt,
            CreatedAt = casino.CreatedAt
        };
    }

    // Slug generator köməkçi metodu
    private string Slugify(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            return string.Empty;
        }
        return phrase.ToLowerInvariant().Trim().Replace(" ", "-");
    }
}