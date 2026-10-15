using FoodMapGIS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Infrastructure.Database.Configurations
{
    internal class ReviewReplyConfiguration : IEntityTypeConfiguration<ReviewReply>
    {
        public void Configure(EntityTypeBuilder<ReviewReply> builder)
        {
            builder.HasIndex(x => x.ReviewId).IsUnique();
            builder.HasOne(x => x.Review)
                .WithOne(x => x.Reply)
                .HasForeignKey<ReviewReply>(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
