using FoodMapGIS.Application.Common.Interface.Infrastructure;
using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Admin;
using FoodMapGIS.Application.DTOs.Admin.Mapping;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using FoodMapGIS.Application.DTOs.FoodPlaces.Mapping;
using FoodMapGIS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodMapGIS.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PendingFoodPlaceDto>> GetPendingPlacesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.FoodPlaces.GetPendingPlacesAsync(page, pageSize, cancellationToken);
        }
        public async Task<FoodPlaceDetailDto> ApprovePlaceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdWithDetailsAsync(id, cancellationToken: cancellationToken)
                ?? throw new Exception("Không tìm thấy quán ăn.");
            if (place.Status != FoodPlaceStatus.Pending)
            {
                throw new Exception($"Chỉ có thể duyệt quán đang ở trạng thái Chờ duyệt (Pending). Trạng thái hiện tại: {place.Status}");
            }
            place.Status = FoodPlaceStatus.Active;
            place.RejectionReason = null;
            place.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return place.EnityToDetailResponseDto();
        }
        public async Task<FoodPlaceDetailDto> RejectPlaceAsync(Guid id, RejectPlaceRequestDto request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.Reason))
            {
                throw new Exception("Vui lòng cung cấp lý do từ chối duyệt quán.");
            }
            var place = await _unitOfWork.FoodPlaces.GetByIdWithDetailsAsync(id, cancellationToken: cancellationToken)
                ?? throw new Exception("Không tìm thấy quán ăn.");
            if (place.Status != FoodPlaceStatus.Pending)
            {
                throw new Exception($"Chỉ có thể từ chối quán đang ở trạng thái Chờ duyệt (Pending). Trạng thái hiện tại: {place.Status}");
            }
            place.Status = FoodPlaceStatus.Rejected;
            place.RejectionReason = request.Reason;
            place.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return place.EnityToDetailResponseDto();
        }
        public async Task<FoodPlaceDetailDto> SuspendPlaceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdWithDetailsAsync(id, cancellationToken: cancellationToken)
                ?? throw new Exception("Không tìm thấy quán ăn.");
            place.Status = FoodPlaceStatus.Suspended;
            place.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return place.EnityToDetailResponseDto();
        }
        public async Task<FoodPlaceDetailDto> UnsuspendPlaceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdWithDetailsAsync(id, cancellationToken: cancellationToken)
                ?? throw new Exception("Không tìm thấy quán ăn.");
            if (place.Status != FoodPlaceStatus.Suspended)
            {
                throw new Exception("Quán ăn này không ở trạng thái bị khóa.");
            }
            place.Status = FoodPlaceStatus.Active;
            place.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return place.EnityToDetailResponseDto();
        }
        public async Task DeleteViolatingPlaceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdAsync(id, cancellationToken)
                ?? throw new Exception("Không tìm thấy quán ăn.");
            _unitOfWork.FoodPlaces.Delete(place);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
