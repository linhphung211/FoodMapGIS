using FoodMapGIS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Infrastructure.Database.Configurations
{
    internal class ReviewReactConfiguration : IEntityTypeConfiguration<ReviewReact>
    {
        public void Configure(EntityTypeBuilder<ReviewReact> builder)
        {
            builder.HasIndex(x => new { x.UserId, x.ReviewId }).IsUnique();
        }
    }
}
