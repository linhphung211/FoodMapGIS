using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.FoodPlaces
{
    public record UpdateFoodPlaceRequestDto
    {
        public string Name { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public Guid CategoryId { get; init; }
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public string? Phone { get; init; }
        public TimeOnly? OpeningTime { get; init; }
        public TimeOnly? ClosingTime { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? Description { get; init; }
    }
}
