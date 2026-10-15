using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.DTOs.Admin;
using FoodMapGIS.Application.DTOs.Admin.Mapping;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using FoodMapGIS.Application.DTOs.FoodPlaces.Mapping;
using FoodMapGIS.Domain.Entities;
using FoodMapGIS.Domain.Enums;
using FoodMapGIS.Infrastructure.Database.Persistence;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FoodMapGIS.Infrastructure.Database.Repositories
{
    public class FoodPlaceRepository : GenericRepository<FoodPlace>, IFoodPlaceRepository
    {
        public FoodPlaceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<FoodPlace?> GetByIdWithDetailsAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<FoodPlace> query = _dbSet;
            if (asNoTracking) query = query.AsNoTracking();

            return await query
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<FoodPlace>> GetActivePlacesWithCategoryAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.Status == FoodPlaceStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FoodPlaceSummaryDto>> GetApprovedSummariesAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.Status == FoodPlaceStatus.Active)
                .OrderByDescending(p => p.AvgRating)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(FoodPlaceMappingExtensions.EnityToSummaryResponseDto)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FoodPlaceSummaryDto>> GetSummariesByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.OwnerId == ownerId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(FoodPlaceMappingExtensions.EnityToSummaryResponseDto)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FoodPlaceSummaryDto>> GetNearbySummariesAsync(double userLng, double userLat, double radiusInKm, CancellationToken cancellationToken = default)
        {
            var userLocation = new Point(userLng, userLat) { SRID = 4326 };
            double radiusInDegrees = radiusInKm / 111.32;

            return await _dbSet
                .AsNoTracking()
                .Where(p => p.Status == FoodPlaceStatus.Active)
                .Where(p => p.Geom.IsWithinDistance(userLocation, radiusInDegrees))
                .OrderBy(p => p.Geom.Distance(userLocation))
                .Select(FoodPlaceMappingExtensions.EnityToSummaryResponseDto)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PendingFoodPlaceDto>> GetPendingPlacesAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.Status == FoodPlaceStatus.Pending)
                .OrderBy(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(AdminMappingExtension.EntityToDtoExpression)
                .ToListAsync(cancellationToken);
        }
    }
}
