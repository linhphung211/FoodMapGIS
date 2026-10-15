using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FoodMapGIS.Domain.Entities;

namespace FoodMapGIS.Application.Common.Interface.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Category> Categories { get; }
        DbSet<FoodPlace> FoodPlaces { get; }
        DbSet<FoodPlaceImage> FoodPlaceImages { get; }
        DbSet<Review> Reviews { get; }
        DbSet<ReviewReact> ReviewReacts { get; }
        DbSet<Bookmark> Bookmarks { get; }
        DbSet<Notification> Notifications { get; }
        DbSet<ReviewReply> ReviewReplies { get; }
        DbSet<ReviewImage> ReviewImages { get; }
        DbSet<MenuItem> MenuItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
