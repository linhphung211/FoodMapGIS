using FoodMapGIS.Application.DTOs.Categories;
using FoodMapGIS.Domain.Entities;

namespace FoodMapGIS.Application.Common.Interface.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<CategoryResponseDto>> GetAllDtosAsync(CancellationToken cancellationToken = default);
    }
}