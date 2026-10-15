using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class ReviewImage : BaseEntity
{
    public Guid ReviewId { get; set; }
    public Review? Review { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}

