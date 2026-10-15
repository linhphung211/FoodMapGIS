using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Admin
{
    public record PendingFoodPlaceDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string CategoryName { get; init; } = string.Empty;

        // Thong tin chu quan de Admin doi chieu
        public Guid OwnerId { get; init; }
        public string OwnerUsername { get; init; } = string.Empty;
        public string OwnerEmail { get; init; } = string.Empty;

        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
