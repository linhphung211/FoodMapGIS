using FoodMapGIS.Application.Common.Interface.Repositories;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Menu;
using FoodMapGIS.Application.DTOs.Menu.Mapping;

namespace FoodMapGIS.Application.Services
{
    public class MenuService(IUnitOfWork unitOfWork) : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        // 1. Lấy toàn bộ thực đơn theo quán (gom nhóm)
        public async Task<List<MenuGroupDto>> GetMenuByPlaceIdAsync(Guid placeId, CancellationToken cancellationToken = default)
        {
            var placeExists = await _unitOfWork.FoodPlaces.ExistsAsync(p => p.Id == placeId, cancellationToken);
            if (!placeExists)
            {
                throw new Exception("Không tìm thấy thông tin quán ăn.");
            }

            var items = await _unitOfWork.MenuItems.FindAsync(
                m => m.FoodPlaceId == placeId,
                asNoTracking: true,
                cancellationToken: cancellationToken);

            return items.EntityListToGroupedMenuDto();
        }

        // 2. Xem chi tiết 1 món ăn trong quán
        public async Task<MenuItemDto?> GetMenuItemByIdAsync(Guid placeId, Guid itemId, CancellationToken cancellationToken = default)
        {
            var item = await _unitOfWork.MenuItems.FindOneAsync(
                m => m.Id == itemId && m.FoodPlaceId == placeId,
                asNoTracking: true,
                cancellationToken: cancellationToken);

            return item?.EntityToDto();
        }

        // 3. Thêm món mới vào quán
        public async Task<MenuItemDto> CreateMenuItemAsync(Guid placeId, Guid currentUserId, CreateMenuItemRequestDto request, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdAsync(placeId, cancellationToken);
            if (place == null)
            {
                throw new Exception("Quán ăn không tồn tại.");
            }

            if (place.OwnerId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thêm món ăn vào quán này.");
            }

            var menuItem = request.CreateDtoToEntity(placeId);

            await _unitOfWork.MenuItems.CreateAsync(menuItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return menuItem.EntityToDto();
        }

        // 4. Sửa món ăn trong quán
        public async Task<MenuItemDto> UpdateMenuItemAsync(Guid placeId, Guid itemId, Guid currentUserId, UpdateMenuItemRequestDto request, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdAsync(placeId, cancellationToken);
            if (place == null || place.OwnerId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền chỉnh sửa thực đơn của quán này.");
            }

            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(itemId, cancellationToken);
            if (menuItem == null || menuItem.FoodPlaceId != placeId)
            {
                throw new Exception("Món ăn không tồn tại trong quán ăn này.");
            }

            request.UpdateEntityFromDto(menuItem);

            _unitOfWork.MenuItems.Update(menuItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return menuItem.EntityToDto();
        }

        // 5. Xóa món ăn khỏi quán
        public async Task DeleteMenuItemAsync(Guid placeId, Guid itemId, Guid currentUserId, CancellationToken cancellationToken = default)
        {
            var place = await _unitOfWork.FoodPlaces.GetByIdAsync(placeId, cancellationToken);
            if (place == null || place.OwnerId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa món ăn của quán này.");
            }

            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(itemId, cancellationToken);
            if (menuItem == null || menuItem.FoodPlaceId != placeId)
            {
                throw new Exception("Món ăn không tồn tại trong quán ăn này.");
            }

            _unitOfWork.MenuItems.Delete(menuItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}