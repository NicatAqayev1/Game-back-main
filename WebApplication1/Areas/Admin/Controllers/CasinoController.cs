using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/casino")]
    public class CasinoController : Controller
    {
        private readonly ICasinoService _casinoService;
        public CasinoController(ICasinoService casinoService)
        {
            _casinoService = casinoService;
        }
        [HttpGet("")]
<<<<<<< HEAD
=======
        [HttpGet("casinos")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> Casinos()
        {
            var casinos = await _casinoService.GetAllCasinosAsync();
            return View(casinos);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCasinoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _casinoService.CreateCasinoAsync(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Casinos));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _casinoService.GetCasinoForEditAsync(id);

            // 2. Əgər belə ID-də kazino yoxdursa, siyahıya geri yönləndiririk
            if (dto == null)
            {
                TempData["ErrorMessage"] = "Kazino tapılmadı.";
                return RedirectToAction(nameof(Casinos));
            }

            // 3. Məlumatları View-a (Edit.cshtml) ötürürük ki, inputlar dolu gəlsin
            return View(dto);
        }

<<<<<<< HEAD
        [HttpPost("edit")]
=======
        [HttpPost("edit/{id}")]
>>>>>>> 19e1c65 (18.11.2026)
        public async Task<IActionResult> Edit(UpdateCasinoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _casinoService.UpdateCasinoAsync(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Casinos));
        }
        [HttpPost("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _casinoService.ToggleStatusAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            else
            {
                TempData["SuccessMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Casinos));
        }
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _casinoService.DeleteCasinoAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Casinos));
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Casinos));
        }
    }
}
