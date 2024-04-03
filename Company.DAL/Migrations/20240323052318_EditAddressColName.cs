using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.DAL.Migrations
{
    public partial class EditAddressColName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adrdess",
                table: "Employees",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Employees",
                newName: "Adrdess");
        }
    }
}
