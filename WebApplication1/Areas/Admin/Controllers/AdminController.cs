using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICasinoService _casinoService;


        public AdminController(AppDbContext context, IConfiguration configuration, ICasinoService casinoService)
        {
            _context = context;
            _configuration = configuration;
            _casinoService = casinoService;
        }

        #region Login,Logout
        [AllowAnonymous]
<<<<<<< HEAD
        [HttpGet("admin/login")]
=======
        [HttpGet("login")]
>>>>>>> 19e1c65 (18.11.2026)
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        // --- GİRİŞ ƏMƏLİYYATI (POST) ---
        [AllowAnonymous]
<<<<<<< HEAD
        [HttpPost("admin/login")]
=======
        [HttpPost("login")]
>>>>>>> 19e1c65 (18.11.2026)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            string adminUser = _configuration["AdminSettings:Username"] ?? "admin";
            string adminPass = _configuration["AdminSettings:Password"] ?? "123456";

            if (username == adminUser && password == adminPass)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

<<<<<<< HEAD
                return RedirectToAction("Dashboard");
=======
                return RedirectToAction("Index");
>>>>>>> 19e1c65 (18.11.2026)
            }

            ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır!");
            return View();
        }
<<<<<<< HEAD
        // --- ÇIXIŞ ---
=======
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        #endregion

<<<<<<< HEAD

        // Admin Analitika Paneli: /admin/dashboard

        [HttpGet]
=======
        [HttpGet]
        [HttpGet("index")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> Index()
        {
            int totalClicks = await _context.ClickLogs.CountAsync();
            int totalRegistrations = await _context.ClickLogs.CountAsync(c => c.IsRegistered);
            int totalDeposits = await _context.ClickLogs.CountAsync(c => c.IsDeposited);
            decimal totalPayout = await _context.ClickLogs.SumAsync(c => c.Payout);

            // --- SON 7 GÜNÜN QRAFİK MƏLUMATLARI ---
            var startDate = DateTime.Today.AddDays(-6);

            // Son 7 gündəki kliklər qruplaşdırılır
            var dailyClicks = await _context.ClickLogs
                .Where(c => c.ClickTime >= startDate)
                .GroupBy(c => c.ClickTime.Date)
                .Select(g => new { Date = g.Key.ToString("dd.MM"), Count = g.Count() })
                .ToListAsync();

            // Son 7 gündəki depozitlər (FTD) qruplaşdırılır
            var dailyFtds = await _context.ClickLogs
                .Where(c => c.ClickTime >= startDate && c.IsDeposited)
                .GroupBy(c => c.ClickTime.Date)
                .Select(g => new { Date = g.Key.ToString("dd.MM"), Count = g.Count() })
                .ToListAsync();

<<<<<<< HEAD
=======
            // 2. Unikal kliklər (təkrarlanmayan IP ünvanlarına görə)
            int uniqueClicks = await _context.ClickLogs
                .Where(c => !string.IsNullOrEmpty(c.IpAddress))
                .Select(c => c.IpAddress)
                .Distinct()
                .CountAsync();

            // 3. Konversiya sayı (IsConverted == true olanlar)
            int conversions = await _context.ClickLogs
                .Where(c => c.IsRegistered || c.IsDeposited)
                .CountAsync();
            // 4. Konversiya Faizi (CR%): (Konversiya / Ümumi Klik) * 100
            double conversionRate = totalClicks > 0
                ? Math.Round((double)conversions / totalClicks * 100, 2)
                : 0;

            ViewBag.TotalClicks = totalClicks;
            ViewBag.UniqueClicks = uniqueClicks;
            ViewBag.TotalRegs = totalRegistrations;
            ViewBag.TotalDeposits = totalDeposits;
            ViewBag.ConversionRate = conversionRate;
            ViewBag.TotalPayout = totalPayout;

            // Qrafik üçün verilənləri View-a ötürürük
            ViewBag.ChartLabels = Newtonsoft.Json.JsonConvert.SerializeObject(Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i).ToString("dd.MM")));

            // Hər günə uyğun klik və FTD massivlərini hazırlayırıq
            var labelsList = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i).ToString("dd.MM")).ToList();

            var clicksData = labelsList.Select(date => dailyClicks.FirstOrDefault(x => x.Date == date)?.Count ?? 0).ToList();
            var ftdsData = labelsList.Select(date => dailyFtds.FirstOrDefault(x => x.Date == date)?.Count ?? 0).ToList();

            ViewBag.ClicksData = Newtonsoft.Json.JsonConvert.SerializeObject(clicksData);
            ViewBag.FtdsData = Newtonsoft.Json.JsonConvert.SerializeObject(ftdsData);

            ViewBag.RecentClicks = await _context.ClickLogs.Include(c => c.Casino).OrderByDescending(c => c.ClickTime).Take(20).ToListAsync();
            ViewBag.RecentPostbacks = await _context.PostbackLogs.OrderByDescending(p => p.ProcessedTime).Take(20).ToListAsync();

            return View();
        }





        // 1. Admin Ana Səhifəsi: Kazinoların siyahısı və klik statistikası
        // 1. Admin Dashboard (Əsas Səhifə + Xülasə Statistikası)
        [Route("dashboard")]
        public async Task<IActionResult> Dashboard()
                {
                    var casinos = await _context.Casinos.ToListAsync();

            // Hər kazino üzrə klik sayı
            var clickCounts = await _context.ClickLogs
                .GroupBy(c => c.CasinoId)
                .Select(g => new { CasinoId = g.Key ?? 0, Count = g.Count() })
                .ToDictionaryAsync(x => x.CasinoId, x => x.Count);

            // Ümumi statistika göstəriciləri
            ViewBag.TotalCasinos = casinos.Count;
                    ViewBag.TotalClicks = await _context.ClickLogs.CountAsync();
                    ViewBag.TodayClicks = await _context.ClickLogs
                        .Where(c => c.ClickTime >= DateTime.Today)
                        .CountAsync();

                    ViewBag.ClickCounts = clickCounts;


            int totalClicks = await _context.ClickLogs.CountAsync();
            int totalRegistrations = await _context.ClickLogs.CountAsync(c => c.IsRegistered);
            int totalDeposits = await _context.ClickLogs.CountAsync(c => c.IsDeposited);
            decimal totalPayout = await _context.ClickLogs.SumAsync(c => c.Payout);

            // --- SON 7 GÜNÜN QRAFİK MƏLUMATLARI ---
            var startDate = DateTime.Today.AddDays(-6);

            // Son 7 gündəki kliklər qruplaşdırılır
            var dailyClicks = await _context.ClickLogs
                .Where(c => c.ClickTime >= startDate)
                .GroupBy(c => c.ClickTime.Date)
                .Select(g => new { Date = g.Key.ToString("dd.MM"), Count = g.Count() })
                .ToListAsync();

            // Son 7 gündəki depozitlər (FTD) qruplaşdırılır
            var dailyFtds = await _context.ClickLogs
                .Where(c => c.ClickTime >= startDate && c.IsDeposited)
                .GroupBy(c => c.ClickTime.Date)
                .Select(g => new { Date = g.Key.ToString("dd.MM"), Count = g.Count() })
                .ToListAsync();

>>>>>>> 19e1c65 (18.11.2026)
            ViewBag.TotalClicks = totalClicks;
            ViewBag.TotalRegs = totalRegistrations;
            ViewBag.TotalFtds = totalDeposits;
            ViewBag.TotalPayout = totalPayout;

            // Qrafik üçün verilənləri View-a ötürürük
            ViewBag.ChartLabels = Newtonsoft.Json.JsonConvert.SerializeObject(Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i).ToString("dd.MM")));

            // Hər günə uyğun klik və FTD massivlərini hazırlayırıq
            var labelsList = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i).ToString("dd.MM")).ToList();

            var clicksData = labelsList.Select(date => dailyClicks.FirstOrDefault(x => x.Date == date)?.Count ?? 0).ToList();
            var ftdsData = labelsList.Select(date => dailyFtds.FirstOrDefault(x => x.Date == date)?.Count ?? 0).ToList();

            ViewBag.ClicksData = Newtonsoft.Json.JsonConvert.SerializeObject(clicksData);
            ViewBag.FtdsData = Newtonsoft.Json.JsonConvert.SerializeObject(ftdsData);

            ViewBag.RecentClicks = await _context.ClickLogs.Include(c => c.Casino).OrderByDescending(c => c.ClickTime).Take(20).ToListAsync();
            ViewBag.RecentPostbacks = await _context.PostbackLogs.OrderByDescending(p => p.ProcessedTime).Take(20).ToListAsync();

<<<<<<< HEAD
            return View();
        }





        // 1. Admin Ana Səhifəsi: Kazinoların siyahısı və klik statistikası
        // 1. Admin Dashboard (Əsas Səhifə + Xülasə Statistikası)
        [Route("admin/dashboard")]
        public async Task<IActionResult> Dashboard()
                {
                    var casinos = await _context.Casinos.ToListAsync();

            // Hər kazino üzrə klik sayı
            var clickCounts = await _context.ClickLogs
                .GroupBy(c => c.CasinoId)
                .Select(g => new { CasinoId = g.Key ?? 0, Count = g.Count() })
                .ToDictionaryAsync(x => x.CasinoId, x => x.Count);

            // Ümumi statistika göstəriciləri
            ViewBag.TotalCasinos = casinos.Count;
                    ViewBag.TotalClicks = await _context.ClickLogs.CountAsync();
                    ViewBag.TodayClicks = await _context.ClickLogs
                        .Where(c => c.ClickTime >= DateTime.Today)
                        .CountAsync();

                    ViewBag.ClickCounts = clickCounts;
                    return View(casinos);
                }

        // 2. Ətraflı Klik Tarixçəsi Səhifəsi (Analytics)
=======
            return View(casinos);
                }

        // 2. Ətraflı Klik Tarixçəsi Səhifəsi (Analytics)
        [Route("analytics1")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> Analytics1()
        {
            // Son 100 kliki tarixi ilə birgə bazadan gətiririk
            var logs = await _context.ClickLogs
                .Include(c => c.Casino)
                .OrderByDescending(c => c.ClickTime)
                .Take(100)
                .ToListAsync();

            return View(logs);
        }
        // JSON Statistika Endpoint-i: /Admin/ApiStats
<<<<<<< HEAD
        [Route("admin/apistats")]
=======
        [Route("apistats")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> ApiStats()
        {
            // 1. Ümumi kliklərin sayı
            int totalClicks = await _context.ClickLogs.CountAsync();

            // 2. Unikal kliklər (təkrarlanmayan IP ünvanlarına görə)
            int uniqueClicks = await _context.ClickLogs
                .Where(c => !string.IsNullOrEmpty(c.IpAddress))
                .Select(c => c.IpAddress)
                .Distinct()
                .CountAsync();

            // 3. Konversiya sayı (IsConverted == true olanlar)
            int conversions = await _context.ClickLogs
<<<<<<< HEAD
                .Where(c => c.IsConverted)
=======
                .Where(c => c.IsRegistered || c.IsDeposited)
>>>>>>> 19e1c65 (18.11.2026)
                .CountAsync();

            // 4. Konversiya Faizi (CR%): (Konversiya / Ümumi Klik) * 100
            double conversionRate = totalClicks > 0
                ? Math.Round((double)conversions / totalClicks * 100, 2)
                : 0;

            // 5. Ümumi Qazanc (Total Payout)
            decimal totalPayout = await _context.ClickLogs
                .SumAsync(c => (decimal?)c.Payout) ?? 0;

<<<<<<< HEAD
            // Nəticəni JSON formatında qaytarırıq
=======
            // Nəticəni JSON formatında qaytarırırıq
>>>>>>> 19e1c65 (18.11.2026)
            return Json(new
            {
                success = true,
                totalClicks = totalClicks,
                uniqueClicks = uniqueClicks,
                conversions = conversions,
                conversionRate = $"{conversionRate}%",
                totalPayout = $"{totalPayout} AZN"
            });
        }
        // 1. Ətraflı Analitika Səhifəsi
<<<<<<< HEAD
        [Route("admin/analytics")]
=======
        [Route("analytics")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> Analytics()
        {
            var logs = await _context.ClickLogs
                .Include(c => c.Casino)
                .OrderByDescending(c => c.ClickTime)
                .Take(100)
                .ToListAsync();

            int totalClicks = await _context.ClickLogs.CountAsync();

            int uniqueClicks = await _context.ClickLogs
                .Where(c => !string.IsNullOrEmpty(c.IpAddress))
                .Select(c => c.IpAddress)
                .Distinct()
                .CountAsync();

            int registrations = await _context.ClickLogs.Where(c => c.IsRegistered).CountAsync();
            int deposits = await _context.ClickLogs.Where(c => c.IsDeposited).CountAsync();

            // CR% adətən FTD / Total Clicks olaraq hesablanır
            double conversionRate = totalClicks > 0
                ? Math.Round((double)deposits / totalClicks * 100, 2)
                : 0;

            decimal totalPayout = await _context.ClickLogs
                .SumAsync(c => (decimal?)c.Payout) ?? 0;

            var viewModel = new AnalyticsViewModel
            {
                Logs = logs,
                TotalClicks = totalClicks,
                UniqueClicks = uniqueClicks,
                TotalRegistrations = registrations,
                TotalDeposits = deposits,
                ConversionRate = conversionRate,
                TotalPayout = totalPayout
            };

            return View(viewModel);
}


        #region Additional Functions
        //EXCEL HESABATI

<<<<<<< HEAD
        [HttpGet("admin/export/clicks")]
=======
        [HttpGet("export/clicks")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> ExportClicksToCsv()
        {
            // Bütün klik loglarını kazino məlumatları ilə birlikdə çəkirik
            var clicks = await _context.ClickLogs
                .Include(c => c.Casino)
                .OrderByDescending(c => c.ClickTime)
                .ToListAsync();

            var csvBuilder = new StringBuilder();

            // CSV Sütun Başlıqları
            csvBuilder.AppendLine("ID,Token,Kazino,Tarix,IP Unvani,Olke,Sub1,Sub2,Sub3,Qeydiyyat,Depozit (FTD),Payout (AZN)");

            // Hər bir klik sətrini əlavə edirik
            foreach (var c in clicks)
            {
                // CSV daxilində vergül xətası olmasın deyə təmizləyirik
                string casinoName = c.Casino?.Name?.Replace(",", " ") ?? "Naməlum";
                string sub1 = c.Sub1?.Replace(",", " ") ?? "";
                string sub2 = c.Sub2?.Replace(",", " ") ?? "";
                string sub3 = c.Sub3?.Replace(",", " ") ?? "";

                csvBuilder.AppendLine($"{c.Id},{c.Token},{casinoName},{c.ClickTime:yyyy-MM-dd HH:mm:ss},{c.IpAddress},{c.CountryCode},{sub1},{sub2},{sub3},{c.IsRegistered},{c.IsDeposited},{c.Payout}");
            }

            // Faylı bayt massivinə çevirib brauzerə yüklənmək üçün göndəririk
            byte[] byteArray = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csvBuilder.ToString())).ToArray();
            string fileName = $"ClickLogs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(byteArray, "text/csv", fileName);
        }

        // --- POSTBACK TEST ALƏTİ METODLARI ---

<<<<<<< HEAD
        [HttpGet("admin/test-postback")]
=======
        [HttpGet("test-postback")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> TestPostback()
        {
            // Adminin asanlıqla token seçə bilməsi üçün son klikləri çəkirik
            var recentClicks = await _context.ClickLogs
                .Include(c => c.Casino)
                .OrderByDescending(c => c.ClickTime)
                .Take(15)
                .ToListAsync();

            ViewBag.RecentClicks = recentClicks;
            return View();
        }

<<<<<<< HEAD
        [HttpPost("admin/test-postback")]
=======
        [HttpPost("test-postback")]
>>>>>>> 19e1c65 (18.11.2026)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestPostback(Guid token, string type, decimal payout)
        {
            var clickLog = await _context.ClickLogs.FirstOrDefaultAsync(c => c.Token == token);
            if (clickLog == null)
            {
                TempData["Error"] = "Göstərilən tokenə uyğun klik tapılmadı!";
                return RedirectToAction(nameof(TestPostback));
            }

            string normalizedType = type?.ToLower() ?? "reg";

            // Məntiqi tətbiq edirik
            if (normalizedType == "reg")
            {
                if (!clickLog.IsDeposited)
                {
                    clickLog.IsRegistered = true;
                    clickLog.Payout = payout;
                }
            }
            else if (normalizedType == "ftd")
            {
                clickLog.IsRegistered = true;
                clickLog.IsDeposited = true;
                clickLog.Payout = payout;
            }
            else if (normalizedType == "ftd_cancel")
            {
                clickLog.IsDeposited = false; 
                clickLog.Payout = 0.0m;       
            }

            // Audit üçün PostbackLog əlavə edirik
            var postbackLog = new PostbackLog
            {
                ProcessedTime = DateTime.UtcNow,
                IpAddress = "127.0.0.1 (Admin Test)",
                PostbackType = normalizedType,
                Payout = payout,
                IsSuccessful = true,
                ResponseMessage = "Admin panelindən test simulyasiyası uğurla icra olundu."
            };

            _context.PostbackLogs.Add(postbackLog);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Uğurlu test! Token: {token} üçün {normalizedType.ToUpper()} qeydə alındı, Payout: {payout} AZN əlavə edildi.";
            return RedirectToAction(nameof(TestPostback));
        }
        #endregion
    }
}