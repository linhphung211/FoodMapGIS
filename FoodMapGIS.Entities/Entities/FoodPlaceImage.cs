using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class FoodPlaceImage : BaseEntity
{
    public Guid FoodPlaceId { get; set; }
    public FoodPlace? FoodPlace { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
}

