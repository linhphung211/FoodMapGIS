using FoodMapGIS.Application.Common.Interface;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.Admin;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodMapGIS.Controllers;

[ApiController]
[Route("api/admin/food-places")]
[Authorize(Roles = "Admin")] // KHÓA TOÀN BỘ CONTROLLER: Chỉ Admin mới có quyền vào!
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Xem danh sách các quán ăn đang chờ duyệt (Pending) - FR-ADMIN-01
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<List<PendingFoodPlaceDto>>> GetPendingPlaces(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var places = await _adminService.GetPendingPlacesAsync(page, pageSize, cancellationToken);
        return Ok(places);
    }

    /// <summary>
    /// Duyệt quán ăn -> Chuyển sang Active (hiển thị lên bản đồ) - FR-ADMIN-01
    /// </summary>
    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<FoodPlaceDetailDto>> ApprovePlace(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adminService.ApprovePlaceAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Từ chối duyệt quán ăn kèm lý do -> Chuyển sang Rejected - FR-ADMIN-01
    /// </summary>
    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<FoodPlaceDetailDto>> RejectPlace(
        Guid id,
        [FromBody] RejectPlaceRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adminService.RejectPlaceAsync(id, request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Tạm khóa quán ăn vi phạm quy định (Active -> Suspended) - FR-ADMIN-04
    /// </summary>
    [HttpPut("{id:guid}/suspend")]
    public async Task<ActionResult<FoodPlaceDetailDto>> SuspendPlace(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adminService.SuspendPlaceAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Mở khóa quán ăn (Suspended -> Active) - FR-ADMIN-04
    /// </summary>
    [HttpPut("{id:guid}/unsuspend")]
    public async Task<ActionResult<FoodPlaceDetailDto>> UnsuspendPlace(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adminService.UnsuspendPlaceAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Xóa vĩnh viễn quán ăn vi phạm quy định - FR-ADMIN-04
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteViolatingPlace(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _adminService.DeleteViolatingPlaceAsync(id, cancellationToken);
            return Ok(new { message = "Xóa quán ăn vi phạm thành công." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
