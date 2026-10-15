using FoodMapGIS.Application.DTOs.FoodPlaces;
using FoodMapGIS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FoodMapGIS.Application.DTOs.Admin.Mapping
{
    public static class AdminMappingExtension
    {
        public static Expression<Func<FoodPlace, PendingFoodPlaceDto>> EntityToDtoExpression =>
            p => new PendingFoodPlaceDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                OwnerId = p.OwnerId,
                OwnerUsername = p.Owner != null ? p.Owner.Username : string.Empty,
                OwnerEmail = p.Owner != null ? p.Owner.Email : string.Empty,
                Longitude = p.Geom.X,
                Latitude = p.Geom.Y,
                CreatedAt = p.CreatedAt
            };
    }
}
