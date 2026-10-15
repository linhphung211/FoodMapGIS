using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.FoodPlaces
{
    public record FoodPlaceSummaryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string CategoryName { get; init; } = string.Empty;
        public double Longitude { get; init; } // Kinh độ X
        public double Latitude { get; init; }  // Vĩ độ Y
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public double AvgRating { get; init; }
        public int TotalReviews { get; init; }
        public string? PrimaryImageUrl { get; init; }
    }
}
