using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curb.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "patronymic",
                table: "users",
                newName: "photo_url");

            migrationBuilder.AddColumn<DateTime>(
                name: "auth_date",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "auth_date",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "photo_url",
                table: "users",
                newName: "patronymic");
        }
    }
}
