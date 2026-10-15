using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMapGIS.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsEmailVerifiedFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_email_verified",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_email_verified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
