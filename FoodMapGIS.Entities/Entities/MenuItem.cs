using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class MenuItem : BaseEntity
{
    public Guid FoodPlaceId { get; set; }
    public FoodPlace? FoodPlace { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    /// Food group name (e.g. "Món chính", "Món phụ", "Đồ uống", "Tráng miệng", etc.).
    public string? Group { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; } = true;
}

