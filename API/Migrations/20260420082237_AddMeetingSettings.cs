using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetingSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstSessionOccurrenceRequiredManagementMembersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondSessionOccurrenceRequiredManagementMembersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ThirdSessionOccurrenceRequiredManagementMembersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstSessionOccurrenceRequiredMembersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondSessionOccurrenceRequiredMembersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ThirdSessionOccurrenceRequiredMembersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingSettings");
        }
    }
}
