using FoodMapGIS.Application.DTOs.Admin;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using FoodMapGIS.Domain.Entities;

namespace FoodMapGIS.Application.Common.Interface.Repositories
{
    public interface IFoodPlaceRepository : IGenericRepository<FoodPlace>
    {
        // Lấy Entity kèm Category & Images
        Task<FoodPlace?> GetByIdWithDetailsAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

        // Lấy danh sách quán Active kèm Category cho bản đồ GeoJSON
        Task<List<FoodPlace>> GetActivePlacesWithCategoryAsync(CancellationToken cancellationToken = default);

        // Truy vấn danh sách quán dạng Summary DTO
        Task<List<FoodPlaceSummaryDto>> GetApprovedSummariesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<List<FoodPlaceSummaryDto>> GetSummariesByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<List<FoodPlaceSummaryDto>> GetNearbySummariesAsync(double userLng, double userLat, double radiusInKm, CancellationToken cancellationToken = default);

        // Truy vấn danh sách quán chờ duyệt cho Admin
        Task<List<PendingFoodPlaceDto>> GetPendingPlacesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}