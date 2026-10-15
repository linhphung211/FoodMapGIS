using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FoodMapGIS.Application.Common.Interface.Infrastructure;
using FoodMapGIS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodMapGIS.Infrastructure.Database.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<FoodPlace> FoodPlaces => Set<FoodPlace>();
        public DbSet<FoodPlaceImage> FoodPlaceImages => Set<FoodPlaceImage>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ReviewReact> ReviewReacts => Set<ReviewReact>();
        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<ReviewReply> ReviewReplies => Set<ReviewReply>();
        public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Kich hoat postGIS
            modelBuilder.HasPostgresExtension("postgis");

            //Tu dong quet va ap dung tat ca cac ke thua IEntityTypeConfiguration trong project Infrastructure
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
