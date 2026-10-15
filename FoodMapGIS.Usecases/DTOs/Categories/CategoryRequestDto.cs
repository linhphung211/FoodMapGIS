using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Categories
{
    public record CategoryRequestDto
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? IconUrl { get; init; }
    }
}
