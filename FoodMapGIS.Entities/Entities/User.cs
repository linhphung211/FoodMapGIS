using FoodMapGIS.Domain.Common;
using FoodMapGIS.Domain.Enums;

namespace FoodMapGIS.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLogin { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public ICollection<FoodPlace> OwnedPlaces { get; set; } = new List<FoodPlace>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<ReviewReact> ReviewReacts { get; set; } = new List<ReviewReact>();
    public ICollection<ReviewReply> MerchantReplies { get; set; } = new List<ReviewReply>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

