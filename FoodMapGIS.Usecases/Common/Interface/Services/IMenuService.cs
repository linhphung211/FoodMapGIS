using FoodMapGIS.Application.DTOs.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Interface.Services
{
    public interface IMenuService
    {
        // Khách & User xem thực đơn
        Task<List<MenuGroupDto>> GetMenuByPlaceIdAsync(Guid placeId, CancellationToken cancellationToken = default);
        Task<MenuItemDto?> GetMenuItemByIdAsync(Guid placeId, Guid itemId, CancellationToken cancellationToken = default);
        // Merchant quản lý thực đơn của quán mình
        Task<MenuItemDto> CreateMenuItemAsync(Guid placeId, Guid currentUserId, CreateMenuItemRequestDto request, CancellationToken cancellationToken = default);
        Task<MenuItemDto> UpdateMenuItemAsync(Guid placeId, Guid itemId, Guid currentUserId, UpdateMenuItemRequestDto request, CancellationToken cancellationToken = default);
        Task DeleteMenuItemAsync(Guid placeId, Guid itemId, Guid currentUserId, CancellationToken cancellationToken = default);
    }
}
