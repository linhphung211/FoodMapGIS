using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Menu
{
    public record MenuItemDto
    {
        public Guid Id { get; init; }
        public Guid FoodPlaceId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string? ImageUrl { get; init; }
        public string? Group { get; init; }
        public string? Description { get; init; }
        public bool IsAvailable { get; init; }
    }
}
