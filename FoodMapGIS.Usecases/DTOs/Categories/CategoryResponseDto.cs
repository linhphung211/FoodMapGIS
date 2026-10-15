using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Categories
{
    public record CategoryResponseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? IconUrl { get; init; }
    }
}
