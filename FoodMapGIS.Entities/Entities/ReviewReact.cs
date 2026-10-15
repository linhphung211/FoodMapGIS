using FoodMapGIS.Domain.Common;
using FoodMapGIS.Domain.Enums;

namespace FoodMapGIS.Domain.Entities;

public class ReviewReact : BaseEntity
{
    public Guid ReviewId { get; set; }
    public Review? Review { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public ReactionType Type { get; set; }
}

