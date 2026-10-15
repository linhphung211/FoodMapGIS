using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.FoodPlaces
{
    public record FoodPlaceDetailDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public string? Phone { get; init; }
        public string? OpeningTime { get; init; }
        public string? ClosingTime { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public double AvgRating { get; init; }
        public int TotalReviews { get; init; }
        public string? Description { get; init; }
        public string Status { get; init; } = string.Empty;
        public List<string> ImageUrls { get; init; } = new();
    }
}
