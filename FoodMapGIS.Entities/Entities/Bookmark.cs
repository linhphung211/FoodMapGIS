using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class Bookmark : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid FoodPlaceId { get; set; }
    public FoodPlace? FoodPlace { get; set; }
}

