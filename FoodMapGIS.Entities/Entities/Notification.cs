using FoodMapGIS.Domain.Common;
using FoodMapGIS.Domain.Enums;

namespace FoodMapGIS.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    /// Optional reference to related entity (e.g., FoodPlace ID, Review ID).
    public string? ReferenceId { get; set; }
}

