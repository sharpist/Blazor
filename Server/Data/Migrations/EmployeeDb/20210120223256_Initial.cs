using Microsoft.EntityFrameworkCore.Migrations;

namespace Blazor.Server.Data.Migrations.EmployeeDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    City = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}
