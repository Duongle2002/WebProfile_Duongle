using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebProfile.Migrations
{
    /// <inheritdoc />
    public partial class AddDeployUrlToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeployUrl",
                table: "Projects",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeployUrl",
                table: "Projects");
        }
    }
}
