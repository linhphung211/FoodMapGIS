using FoodMapGIS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Infrastructure.Database.Configurations
{
    public class ReviewConfiguration  : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            //UNIQUE: 1User chi review 1 quan
            builder.HasIndex(x => new { x.UserId, x.FoodPlaceId }).IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.FoodPlace)
                .WithMany(fp => fp.Reviews)
                .HasForeignKey(x => x.FoodPlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
