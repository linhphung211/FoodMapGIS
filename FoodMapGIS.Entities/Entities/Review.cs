using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class Review : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid FoodPlaceId { get; set; }
    public FoodPlace? FoodPlace { get; set; }

    /// <summary>
    /// Rating from 1 to 5 stars.
    /// </summary>
    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();
    public ReviewReply? Reply { get; set; }
    public ICollection<ReviewReact> Reacts { get; set; } = new List<ReviewReact>();
}

