using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Forename = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    EntityName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Forename", "IsActive", "Surname" },
                values: new object[,]
                {
                    { 1L, new DateTime(1992, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ploew@example.com", "Peter", true, "Loew" },
                    { 2L, new DateTime(1995, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "bfgates@example.com", "Benjamin Franklin", true, "Gates" },
                    { 3L, new DateTime(1997, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ctroy@example.com", "Castor", false, "Troy" },
                    { 4L, new DateTime(1998, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "mraines@example.com", "Memphis", true, "Raines" },
                    { 5L, new DateTime(1999, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgodspeed@example.com", "Stanley", true, "Goodspeed" },
                    { 6L, new DateTime(1996, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "himcdunnough@example.com", "H.I.", true, "McDunnough" },
                    { 7L, new DateTime(1997, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpoe@example.com", "Cameron", false, "Poe" },
                    { 8L, new DateTime(1993, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "emalus@example.com", "Edward", false, "Malus" },
                    { 9L, new DateTime(1995, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "dmacready@example.com", "Damon", false, "Macready" },
                    { 10L, new DateTime(1999, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "jblaze@example.com", "Johnny", true, "Blaze" },
                    { 11L, new DateTime(1999, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "rfeld@example.com", "Robin", true, "Feld" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
