using FoodMapGIS.Application.Common.Constants;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Menu;
using FoodMapGIS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodMapGIS.Controllers
{
    [Route(ApiRoutes.Menu)]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>
        /// Xem toàn bộ thực đơn của quán (Gom nhóm theo Category món)
        /// GET /api/food-places/{placeId}/menu
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<MenuGroupDto>>> GetMenu(Guid placeId, CancellationToken cancellationToken)
        {
            try
            {
                var menu = await _menuService.GetMenuByPlaceIdAsync(placeId, cancellationToken);
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Xem chi tiết một món ăn của quán
        /// GET /api/food-places/{placeId}/menu/{itemId}
        /// </summary>
        [HttpGet("{itemId:guid}")]
        public async Task<ActionResult<MenuItemDto>> GetMenuItem(Guid placeId, Guid itemId, CancellationToken cancellationToken)
        {
            var item = await _menuService.GetMenuItemByIdAsync(placeId, itemId, cancellationToken);
            if (item == null)
            {
                return NotFound(new { message = "Không tìm thấy món ăn này trong thực đơn của quán." });
            }
            return Ok(item);
        }
        /// <summary>
        /// Merchant thêm món ăn mới vào thực đơn quán
        /// POST /api/food-places/{placeId}/menu
        /// </summary>
        [Authorize(Roles = "Merchant")]
        [HttpPost]
        public async Task<ActionResult<MenuItemDto>> CreateMenuItem(Guid placeId, [FromBody] CreateMenuItemRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _menuService.CreateMenuItemAsync(placeId, currentUserId, request, cancellationToken);
                return CreatedAtAction(nameof(GetMenuItem), new { placeId, itemId = result.Id }, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Merchant chỉnh sửa món ăn trong thực đơn
        /// PUT /api/food-places/{placeId}/menu/{itemId}
        /// </summary>
        [Authorize(Roles = "Merchant")]
        [HttpPut("{itemId:guid}")]
        public async Task<ActionResult<MenuItemDto>> UpdateMenuItem(Guid placeId, Guid itemId, [FromBody] UpdateMenuItemRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _menuService.UpdateMenuItemAsync(placeId, itemId, currentUserId, request, cancellationToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Merchant xóa một món ăn khỏi thực đơn
        /// DELETE /api/food-places/{placeId}/menu/{itemId}
        /// </summary>
        [Authorize(Roles = "Merchant")]
        [HttpDelete("{itemId:guid}")]
        public async Task<IActionResult> DeleteMenuItem(Guid placeId, Guid itemId, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                await _menuService.DeleteMenuItemAsync(placeId, itemId, currentUserId, cancellationToken);
                return Ok(new { message = "Xóa món ăn thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        private Guid GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                throw new Exception("Không xác định được danh tính người dùng.");
            }
            return userId;
        }
    }
}
