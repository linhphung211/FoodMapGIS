using FoodMapGIS.Domain.Common;
using FoodMapGIS.Domain.Enums;
using NetTopologySuite.Geometries;

namespace FoodMapGIS.Domain.Entities;

public class FoodPlace : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    /// PostGIS spatial point (SRID = 4326, WGS84: X = Longitude, Y = Latitude).
    public Point Geom { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public FoodPlaceStatus Status { get; set; } = FoodPlaceStatus.Draft;
    public string? RejectionReason { get; set; }
    public string? Phone { get; set; }
    public TimeOnly? OpeningTime { get; set; }
    public TimeOnly? ClosingTime { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double AvgRating { get; set; } = 0.0;
    public int TotalReviews { get; set; } = 0;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<FoodPlaceImage> Images { get; set; } = new List<FoodPlaceImage>();
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
}

