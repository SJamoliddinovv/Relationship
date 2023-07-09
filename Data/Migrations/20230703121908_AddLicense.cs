using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace relationships.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLicense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driverLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Serial = table.Column<string>(type: "text", nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driverLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_driverLicenses_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_driverLicenses_UserId",
                table: "driverLicenses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driverLicenses");
        }
    }
}
