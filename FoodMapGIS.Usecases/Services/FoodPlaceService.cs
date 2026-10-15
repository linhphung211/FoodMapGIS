using FoodMapGIS.Application.Common.Interface.Infrastructure;
using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using FoodMapGIS.Application.DTOs.FoodPlaces.Mapping;
using FoodMapGIS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Services
{
    //public class FoodPlaceService : IFoodPlaceService
    //{
    //    private readonly IApplicationDbContext _context;

    //    public FoodPlaceService(IApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<List<FoodPlaceSummaryDto>> GetApprovedPlacesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    //    {
    //        return await _context.FoodPlaces
    //            .AsNoTracking()
    //            .Where(p => p.Status == FoodPlaceStatus.Active)
    //            .OrderByDescending(p => p.AvgRating)
    //            .Skip((page - 1) * pageSize)
    //            .Take(pageSize)
    //            .Select(FoodPlaceMappingExtensions.EnityToSummaryResponseDto)
    //            .ToListAsync(cancellationToken);
    //    }



    //    public async Task<FoodPlaceDetailDto?> GetPlaceDetailAsync(Guid id, CancellationToken cancellationToken = default)
    //    {
    //        var place = await _context.FoodPlaces
    //            .AsNoTracking()
    //            .Include(p => p.Category)
    //            .Include(p => p.Images)
    //            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    //        if (place == null) return null;

    //        return place.EnityToDetailResponseDto();
    //    }

    //    public async Task<GeoJsonFeatureCollectionDto> GetPlacesGeoJsonAsync(CancellationToken cancellationToken = default)
    //    {
    //        var places = await _context.FoodPlaces
    //            .AsNoTracking()
    //            .Include(p => p.Category)
    //            .Where(p => p.Status == FoodPlaceStatus.Active)
    //            .ToListAsync(cancellationToken);
    //        return places.ToGeoJsonCollection();

    //    }

    //    public async Task<FoodPlaceDetailDto> CreatePlaceAsync(Guid ownerId, CreateFoodPlaceRequestDto request, CancellationToken cancellationToken = default)
    //    {
    //        //Kiem tra xem danh muc có ton tai ko
    //        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
    //        if (!categoryExists)
    //        {
    //            throw new Exception("Danh mục món ăn không hợp lệ.");
    //        }

    //        //Doi tuong khong gian voi kieu du lieu Point thu vien NetTopologySuit (SRID:4326)
    //        var location = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

    //        var foodPlace = request.CreateRequestDtoToEntity(ownerId, location);

    //        await _context.FoodPlaces.AddAsync(foodPlace, cancellationToken);
    //        await _context.SaveChangesAsync(cancellationToken);

    //        return (await GetPlaceDetailAsync(foodPlace.Id, cancellationToken))!;
    //    }

    //    public async Task<FoodPlaceDetailDto> UpdatePlaceAsync(Guid id, Guid ownerId, UpdateFoodPlaceRequestDto request, CancellationToken cancellationToken = default)
    //    {
    //        var place = await _context.FoodPlaces.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    //        if (place == null)
    //        {
    //            throw new Exception("Không tìm thấy quán ăn.");
    //        }
    //        if (place.OwnerId != ownerId)
    //        {
    //            throw new Exception("Bạn không có quyền chỉnh sửa quán ăn này.");
    //        }

    //        request.UpdateResponseDtoToEntity(place);
    //        // Nếu quán từng bị Rejected, sửa xong chuyển lại thành Pending để Admin duyệt lại (Theo SRS Q1):
    //        if (place.Status == FoodPlaceStatus.Rejected)
    //        {
    //            place.Status = FoodPlaceStatus.Pending;
    //            place.RejectionReason = null;
    //        }
    //        await _context.SaveChangesAsync(cancellationToken);
    //        return (await GetPlaceDetailAsync(place.Id, cancellationToken))!;
    //    }


    //    public async Task DeletePlaceAsync(Guid id, Guid ownerId, CancellationToken cancellationToken = default)
    //    {
    //        var place = await _context.FoodPlaces.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    //        if (place == null)
    //        {
    //            throw new Exception("Không tìm thấy quán ăn.");
    //        }
    //        if (place.OwnerId != ownerId)
    //        {
    //            throw new Exception("Bạn không có quyền xóa quán ăn này.");
    //        }
    //        _context.FoodPlaces.Remove(place);
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }

    //    public async Task<List<FoodPlaceSummaryDto>> GetMyPlacesAsync(Guid ownerId, CancellationToken cancellationToken = default)
    //    {
    //        return await _context.FoodPlaces
    //        .AsNoTracking()
    //        .Where(p => p.OwnerId == ownerId)
    //        .OrderByDescending(p => p.CreatedAt)
    //        .Select(FoodPlaceMappingExtensions.EnityToSummaryResponseDto)
    //        .ToListAsync(cancellationToken);
    //    }

    //    public async Task<List<FoodPlaceSummaryDto>> GetNearbyPlacesAsync(double userLng, double userLat, double radiusInKm, CancellationToken cancellationToken = default)
    //    {
    //        var userLocation = new Point(userLng, userLat) { SRID = 4326 };
    //        // doi km sang do: 1 độ ≈ 111.32 km
    //        double radiusInDegrees = radiusInKm / 111.32;
    //        return await _context.FoodPlaces
    //            .AsNoTracking()
    //            .Where(p => p.Status == FoodPlaceStatus.Active)
    //            // Ham ko gian PostGIS ST_DWithin tan dung chi muc GiST
    //            .Where(p => p.Geom.IsWithinDistance(userLocation, radiusInDegrees))
    //            .OrderBy(p => p.Geom.Distance(userLocation)) // Xếp quán gần nhất lên đầu
    //            .Select(FoodPlaceMappingExtensions.EnityToSummaryResponseDto)
    //            .ToListAsync(cancellationToken);
    //    }
    //}


    public class FoodPlaceService : IFoodPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FoodPlaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<FoodPlaceSummaryDto>> GetApprovedPlacesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.FoodPlaces.GetApprovedSummariesAsync(page, pageSize, cancellationToken);
        }
        public async Task<FoodPlaceDetailDto?> GetPlaceDetailAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdWithDetailsAsync(id, asNoTracking: true, cancellationToken: cancellationToken);
            return place?.EnityToDetailResponseDto();
        }
        public async Task<GeoJsonFeatureCollectionDto> GetPlacesGeoJsonAsync(CancellationToken cancellationToken = default)
        {
            var places = await _unitOfWork.FoodPlaces.GetActivePlacesWithCategoryAsync(cancellationToken);
            return places.ToGeoJsonCollection();
        }
        public async Task<FoodPlaceDetailDto> CreatePlaceAsync(Guid ownerId, CreateFoodPlaceRequestDto request, CancellationToken cancellationToken = default)
        {
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                throw new Exception("Danh mục món ăn không hợp lệ.");
            }
            var location = new Point(request.Longitude, request.Latitude) { SRID = 4326 };
            var foodPlace = request.CreateRequestDtoToEntity(ownerId, location);
            await _unitOfWork.FoodPlaces.CreateAsync(foodPlace, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return (await GetPlaceDetailAsync(foodPlace.Id, cancellationToken))!;
        }
        public async Task<FoodPlaceDetailDto> UpdatePlaceAsync(Guid id, Guid ownerId, UpdateFoodPlaceRequestDto request, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdAsync(id, cancellationToken);
            if (place == null)
            {
                throw new Exception("Không tìm thấy quán ăn.");
            }
            if (place.OwnerId != ownerId)
            {
                throw new Exception("Bạn không có quyền chỉnh sửa quán ăn này.");
            }
            request.UpdateResponseDtoToEntity(place);
            if (place.Status == FoodPlaceStatus.Rejected)
            {
                place.Status = FoodPlaceStatus.Pending;
                place.RejectionReason = null;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return (await GetPlaceDetailAsync(place.Id, cancellationToken))!;
        }
        public async Task DeletePlaceAsync(Guid id, Guid ownerId, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdAsync(id, cancellationToken);
            if (place == null)
            {
                throw new Exception("Không tìm thấy quán ăn.");
            }
            if (place.OwnerId != ownerId)
            {
                throw new Exception("Bạn không có quyền xóa quán ăn này.");
            }
            _unitOfWork.FoodPlaces.Delete(place);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<FoodPlaceSummaryDto>> GetMyPlacesAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.FoodPlaces.GetSummariesByOwnerAsync(ownerId, cancellationToken);
        }
        public async Task<List<FoodPlaceSummaryDto>> GetNearbyPlacesAsync(double userLng, double userLat, double radiusInKm, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.FoodPlaces.GetNearbySummariesAsync(userLng, userLat, radiusInKm, cancellationToken);
        }
    }
}
