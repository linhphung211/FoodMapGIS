using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodMapGIS.Domain.Entities;

namespace FoodMapGIS.Infrastructure.Database.Configurations
{
    public class FoodPlaceConfiguration : IEntityTypeConfiguration<FoodPlace>
    {
        public void Configure(EntityTypeBuilder<FoodPlace> builder)
        {
            // Thiet lap cac conditions cho cac truong trong entity FoodPlace
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.MinPrice).HasPrecision(18, 2);
            builder.Property(x => x.MaxPrice).HasPrecision(18, 2);

            // Cau hinh du lieu khong gian
            builder.Property(x => x.Geom).HasColumnType("geometry (Point, 4326)"); // Cau hinh du lieu khong gian voi he toa do WGS 84 (EPSG:4326)

            builder.HasIndex(x => x.Geom).HasMethod("GIST"); // Tao chi muc GIST cho truong Geom de toi uu hoa truy van khong gian

            builder.HasOne(x => x.Owner)
                .WithMany(u => u.OwnedPlaces)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.FoodPlaces)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
