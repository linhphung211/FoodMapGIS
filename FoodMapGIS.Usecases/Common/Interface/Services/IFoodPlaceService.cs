using FoodMapGIS.Application.DTOs.FoodPlaces;


namespace FoodMapGIS.Application.Common.Interface.Services
{
    public interface IFoodPlaceService
    {
        // Lấy danh sach quan an
        Task<List<FoodPlaceSummaryDto>> GetApprovedPlacesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        // Xem chi tiet 1 quan an
        Task<FoodPlaceDetailDto?> GetPlaceDetailAsync(Guid id, CancellationToken cancellationToken = default);
        // Xuat du lieu chuan GeoJSON FeatureCollection cho Leaflet 
        Task<GeoJsonFeatureCollectionDto> GetPlacesGeoJsonAsync(CancellationToken cancellationToken = default);

        // Merchant
        Task<FoodPlaceDetailDto> CreatePlaceAsync(Guid ownerId, CreateFoodPlaceRequestDto request, CancellationToken cancellationToken = default);
        Task<List<FoodPlaceSummaryDto>> GetMyPlacesAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<FoodPlaceDetailDto> UpdatePlaceAsync(Guid id, Guid ownerId, UpdateFoodPlaceRequestDto request, CancellationToken cancellationToken = default);
        Task DeletePlaceAsync(Guid id, Guid ownerId, CancellationToken cancellationToken = default);

        //Ham tim kiem quan an trong ban kinh ... km
        Task<List<FoodPlaceSummaryDto>> GetNearbyPlacesAsync(double userLng, double userLat, double radiusInKm, CancellationToken cancellationToken = default);
    }
}
