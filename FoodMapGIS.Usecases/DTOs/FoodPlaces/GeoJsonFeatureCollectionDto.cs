using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.DTOs.FoodPlaces
{
    public record GeoJsonFeatureCollectionDto
    {
        public string Type { get; init; } = "FeatureCollection";
        public List<GeoJsonFeatureDto> Features { get; init; } = new();
    }
    public record GeoJsonFeatureDto
    {
        public string Type { get; init; } = "Feature";
        public GeoJsonGeometryDto Geometry { get; init; } = new();
        public Dictionary<string, object?> Properties { get; init; } = new();
    }
    public record GeoJsonGeometryDto
    {
        public string Type { get; init; } = "Point";
        // [Kinh độ Longitude, Vĩ độ Latitude] chuẩn GeoJSON:
        public List<double> Coordinates { get; init; } = new();
    }
}
