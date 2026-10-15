using FoodMapGIS.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Interface.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
