using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.DTOs.Categories;
using FoodMapGIS.Application.DTOs.Categories.Mapping;
using FoodMapGIS.Domain.Entities;
using FoodMapGIS.Infrastructure.Database.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FoodMapGIS.Infrastructure.Database.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<CategoryResponseDto>> GetAllDtosAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Select(CategoryMappingExtensions.EntityToDtoExpression)
                .ToListAsync(cancellationToken);
        }
    }
}
