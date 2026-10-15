using FoodMapGIS.Application.DTOs.Admin;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Interface.Services
{
    public interface IAdminService
    {
        // 1. Xem danh sách quán đang chờ duyệt
        Task<List<PendingFoodPlaceDto>> GetPendingPlacesAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        // 2. Duyệt quán ăn (Pending -> Active)
        Task<FoodPlaceDetailDto> ApprovePlaceAsync(Guid id, CancellationToken cancellationToken = default);
        // 3. Từ chối quán ăn (Pending -> Rejected kèm lý do)
        Task<FoodPlaceDetailDto> RejectPlaceAsync(Guid id, RejectPlaceRequestDto request, CancellationToken cancellationToken = default);
        // 4. Tạm khóa quán ăn vi phạm (Active -> Suspended)
        Task<FoodPlaceDetailDto> SuspendPlaceAsync(Guid id, CancellationToken cancellationToken = default);
        // 5. Mở khóa quán ăn (Suspended -> Active)
        Task<FoodPlaceDetailDto> UnsuspendPlaceAsync(Guid id, CancellationToken cancellationToken = default);
        // 6. Xóa vĩnh viễn quán rác / vi phạm nghiêm trọng
        Task DeleteViolatingPlaceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
