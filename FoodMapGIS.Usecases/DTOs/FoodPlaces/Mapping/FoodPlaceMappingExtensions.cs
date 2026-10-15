using FoodMapGIS.Domain.Entities;
using FoodMapGIS.Domain.Enums;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;

namespace FoodMapGIS.Application.DTOs.FoodPlaces.Mapping
{
    public static class FoodPlaceMappingExtensions
    {
        public static FoodPlaceDetailDto EnityToDetailResponseDto(this FoodPlace place)
        {
            return new FoodPlaceDetailDto
            {
                Id = place.Id,
                Name = place.Name,
                Address = place.Address,
                CategoryId = place.CategoryId,
                CategoryName = place.Category?.Name ?? string.Empty,
                Longitude = place.Geom.X,
                Latitude = place.Geom.Y,
                Phone = place.Phone,
                OpeningTime = place.OpeningTime?.ToString("HH:mm"),
                ClosingTime = place.ClosingTime?.ToString("HH:mm"),
                MinPrice = place.MinPrice,
                MaxPrice = place.MaxPrice,
                AvgRating = place.AvgRating,
                TotalReviews = place.TotalReviews,
                Description = place.Description,
                Status = place.Status.ToString(),
                ImageUrls = place.Images.Select(img => img.ImageUrl).ToList()
            };
        }

        public static Expression<Func<FoodPlace, FoodPlaceSummaryDto>> EnityToSummaryResponseDto =>

            p => new FoodPlaceSummaryDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                Longitude = p.Geom.X,
                Latitude = p.Geom.Y,
                MinPrice = p.MinPrice,
                MaxPrice = p.MaxPrice,
                AvgRating = p.AvgRating,
                TotalReviews = p.TotalReviews,
                PrimaryImageUrl = p.Images.Where(img => img.IsPrimary).Select(img => img.ImageUrl).FirstOrDefault()
                               ?? p.Images.Select(img => img.ImageUrl).FirstOrDefault()
            };


        // 1. DTO -> Entity mới (Cho Create)
        public static FoodPlace CreateRequestDtoToEntity(this CreateFoodPlaceRequestDto dto, Guid ownerId, Point location)
        {
            return new FoodPlace
            {
                Name = dto.Name,
                Address = dto.Address,
                CategoryId = dto.CategoryId,
                OwnerId = ownerId,
                Geom = location,
                Status = FoodPlaceStatus.Pending,
                Phone = dto.Phone,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
                MinPrice = dto.MinPrice,
                MaxPrice = dto.MaxPrice,
                Description = dto.Description
            };
        }
        // 2. DTO -> Entity có sẵn (Cho Update)
        public static void UpdateResponseDtoToEntity(this UpdateFoodPlaceRequestDto dto, FoodPlace entity)
        {
            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.CategoryId = dto.CategoryId;
            entity.Geom = new Point(dto.Longitude, dto.Latitude) { SRID = 4326 };
            entity.Phone = dto.Phone;
            entity.OpeningTime = dto.OpeningTime;
            entity.ClosingTime = dto.ClosingTime;
            entity.MinPrice = dto.MinPrice;
            entity.MaxPrice = dto.MaxPrice;
            entity.Description = dto.Description;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        // 1. Map từng quán ăn sang 1 Feature GeoJSON
        public static GeoJsonFeatureDto ToGeoJsonFeature(this FoodPlace place)
        {
            return new GeoJsonFeatureDto
            {
                Type = "Feature",
                Geometry = new GeoJsonGeometryDto
                {
                    Type = "Point",
                    Coordinates = new List<double> { place.Geom.X, place.Geom.Y } // [Lng, Lat]
                },
                Properties = new Dictionary<string, object?>
                {
                    { "id", place.Id },
                    { "name", place.Name },
                    { "address", place.Address },
                    { "category", place.Category?.Name },
                    { "avgRating", place.AvgRating },
                    { "totalReviews", place.TotalReviews },
                    { "minPrice", place.MinPrice },
                    { "maxPrice", place.MaxPrice }
                }
            };
        }
        // 2. Map cả danh sách quán sang 1 GeoJsonFeatureCollectionDto
        public static GeoJsonFeatureCollectionDto ToGeoJsonCollection(this IEnumerable<FoodPlace> places)
        {
            return new GeoJsonFeatureCollectionDto
            {
                Type = "FeatureCollection",
                Features = places.Select(p => p.ToGeoJsonFeature()).ToList()
            };
        }
    }
}
