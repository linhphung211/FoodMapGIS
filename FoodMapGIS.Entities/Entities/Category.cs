using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }

    // Navigation properties
    public ICollection<FoodPlace> FoodPlaces { get; set; } = new List<FoodPlace>();
}

