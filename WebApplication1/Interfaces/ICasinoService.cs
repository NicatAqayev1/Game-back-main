using WebApplication1.Common;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

public interface ICasinoService
{
    Task<ServiceResult> CreateCasinoAsync(CreateCasinoDto dto);
    Task<ServiceResult> UpdateCasinoAsync(UpdateCasinoDto dto);
    Task<ServiceResult> ToggleStatusAsync(int id);
    Task<ServiceResult> DeleteCasinoAsync(int id);

    Task<List<CasinoListDto>> GetAllCasinosAsync();
    Task<CasinoListDto?> GetCasinoBySlugAsync(string slug);
    Task<UpdateCasinoDto?> GetCasinoForEditAsync(int id);
}