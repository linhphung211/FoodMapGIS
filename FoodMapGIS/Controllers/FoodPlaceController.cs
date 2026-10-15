using FoodMapGIS.Application.Common.Constants;
using FoodMapGIS.Application.Common.Interface.Services;
using FoodMapGIS.Application.DTOs.FoodPlaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodMapGIS.Controllers
{
    [Route(ApiRoutes.FoodPlaces)]
    [ApiController]
    public class FoodPlaceController : ControllerBase
    {
        private readonly IFoodPlaceService _foodPlaceService;

        public FoodPlaceController(IFoodPlaceService foodPlaceService)
        {
            _foodPlaceService = foodPlaceService;
        }

        //Lay danh sach quan an da duyet
        [HttpGet]
        public async Task<ActionResult<List<FoodPlaceSummaryDto>>> GetApprovedPlaces(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
        {
            var result = await _foodPlaceService.GetApprovedPlacesAsync(page, pageSize, cancellationToken);
            return Ok(result);
        }

        //Trich xuat danh sach quan an dinh dang chuan GeoJson cho ban cho leaflet
        [HttpGet("geojson")]
        public async Task<ActionResult<GeoJsonFeatureCollectionDto>> GetPlacesGeoJson(CancellationToken cancellationToken)
        {
            var geoJson = await _foodPlaceService.GetPlacesGeoJsonAsync(cancellationToken);
            return Ok(geoJson);
        }

        //Xem chi tiet 1 quan an
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FoodPlaceDetailDto>> GetPlaceDetail(Guid id, CancellationToken cancellationToken)
        {
            var place = await _foodPlaceService.GetPlaceDetailAsync(id, cancellationToken);
            if (place == null)
            {
                return NotFound(new { message = "Không tìm thấy quán ăn này." });
            }
            return Ok(place);
        }

        //Tim kiem quan an theo bán kính (km) quanh vi tri nguoi dung
        [HttpGet("nearby")]
        public async Task<ActionResult<List<FoodPlaceSummaryDto>>> GetNearByPlaces(
            [FromQuery] double lng,
            [FromQuery] double lat,
            [FromQuery] double radiusInKm = 3.0,
            CancellationToken cancellationToken = default)
        {
            var places = await _foodPlaceService.GetNearbyPlacesAsync(lng, lat, radiusInKm, cancellationToken);
            return Ok(places);
        }

        //Merchant dang ki quan moi (Trang thai ban dau la pending)
        [Authorize(Roles = "Merchant")]
        [HttpPost]
        public async Task<ActionResult<FoodPlaceDetailDto>> CreatePlace(
            [FromBody] CreateFoodPlaceRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var ownerId = GetCurrentUserId();
                var newPlace = await _foodPlaceService.CreatePlaceAsync(ownerId, request, cancellationToken);
                return CreatedAtAction(nameof(GetPlaceDetail), new { id = newPlace.Id }, newPlace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Merchant xem danh sach hien thi thong tin quan minh
        [Authorize(Roles = "Merchant")]
        [HttpGet("my-places")]
        public async Task<ActionResult<List<FoodPlaceSummaryDto>>> GetMyPlaces(CancellationToken cancellationToken)
        {
            var ownerId = GetCurrentUserId();
            var places = await _foodPlaceService.GetMyPlacesAsync(ownerId, cancellationToken);
            return Ok(places);
        }

        // Merchant cap nhat thong tin quan minh
        [Authorize(Roles = "Merchant")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FoodPlaceDetailDto>> UpdatePlace(
            Guid id,
            [FromBody] UpdateFoodPlaceRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var ownerId = GetCurrentUserId();
                var updatedPlace = await _foodPlaceService.UpdatePlaceAsync(id, ownerId, request, cancellationToken);
                return Ok(updatedPlace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Merchant xoa quan an cua minh
        [Authorize(Roles = "Merchant")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePlace(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var ownerId = GetCurrentUserId();
                await _foodPlaceService.DeletePlaceAsync(id, ownerId, cancellationToken);
                return Ok(new { message = "Xóa quán ăn thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Ham phu tro lay ID tu JWT
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
