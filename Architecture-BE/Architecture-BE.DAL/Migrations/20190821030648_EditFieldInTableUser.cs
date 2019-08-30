using Microsoft.EntityFrameworkCore.Migrations;

namespace Architecture_BE.DAL.Migrations
{
    public partial class EditFieldInTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}
