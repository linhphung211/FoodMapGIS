using FoodMapGIS.Domain.Common;

namespace FoodMapGIS.Domain.Entities;

public class ReviewReply : BaseEntity
{
    public Guid ReviewId { get; set; }
    public Review? Review { get; set; }

    public Guid MerchantId { get; set; }
    public User? Merchant { get; set; }

    public string Content { get; set; } = string.Empty;
}

