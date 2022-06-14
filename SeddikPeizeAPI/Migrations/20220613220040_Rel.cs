using Microsoft.EntityFrameworkCore.Migrations;

namespace SeddikPeizeAPI.Migrations
{
    public partial class Rel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompId",
                table: "Projects",
                column: "CompId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_compRegs_CompId",
                table: "Projects",
                column: "CompId",
                principalTable: "compRegs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_compRegs_CompId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CompId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CompId",
                table: "Projects");
        }
    }
}
