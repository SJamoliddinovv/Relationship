using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace relationships.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserLicenseRealationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_driverLicenses_users_UserId",
                table: "driverLicenses");

            migrationBuilder.DropIndex(
                name: "IX_driverLicenses_UserId",
                table: "driverLicenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "driverLicenses");

            migrationBuilder.AddForeignKey(
                name: "FK_driverLicenses_users_Id",
                table: "driverLicenses",
                column: "Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_driverLicenses_users_Id",
                table: "driverLicenses");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "driverLicenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_driverLicenses_UserId",
                table: "driverLicenses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_driverLicenses_users_UserId",
                table: "driverLicenses",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
