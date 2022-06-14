using Microsoft.EntityFrameworkCore.Migrations;

namespace SeddikPeizeAPI.Migrations.ApplicationDb
{
    public partial class Acss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsJudge",
                table: "AspNetUsers",
                newName: "IsAccepted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "AspNetUsers",
                newName: "IsJudge");
        }
    }
}
