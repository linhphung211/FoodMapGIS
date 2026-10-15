using FoodMapGIS.Application.Common.Interface.Infrastructure;
using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Services
{
    //public class CategoryService : ICategoryService
    //{
    //    private readonly IApplicationDbContext _context;

    //    public CategoryService(IApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<List<CategoryResponseDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    //    {
    //        // AsNoTracking giúp truy vấn nhanh hơn vì ta chỉ đọc dữ liệu, ko cần sửa
    //        return await _context.Categories
    //            .AsNoTracking()
    //            .Select(c => new CategoryResponseDto
    //            {
    //                Id = c.Id,
    //                Name = c.Name,
    //                Description = c.Description,
    //                IconUrl = c.IconUrl
    //            })
    //            .ToListAsync(cancellationToken);
    //    }
    //}

    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CategoryResponseDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Categories.GetAllDtosAsync(cancellationToken);
        }
    }
}
