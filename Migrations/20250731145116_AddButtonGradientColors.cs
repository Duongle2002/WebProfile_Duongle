using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebProfile.Migrations
{
    /// <inheritdoc />
    public partial class AddButtonGradientColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutlineButtonBorderColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OutlineButtonTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryButtonGradientEnd",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryButtonGradientStart",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryButtonGradientEnd",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryButtonGradientStart",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LiveDemoUrl",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUrl",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeployUrl",
                table: "Projects",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutlineButtonBorderColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "OutlineButtonTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "PrimaryButtonGradientEnd",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "PrimaryButtonGradientStart",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "SecondaryButtonGradientEnd",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "SecondaryButtonGradientStart",
                table: "ThemeSettings");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "VideoUrl",
                keyValue: null,
                column: "VideoUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Tags",
                keyValue: null,
                column: "Tags",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "LiveDemoUrl",
                keyValue: null,
                column: "LiveDemoUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "LiveDemoUrl",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ImageUrl",
                keyValue: null,
                column: "ImageUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "GitHubUrl",
                keyValue: null,
                column: "GitHubUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "GitHubUrl",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "DownloadUrl",
                keyValue: null,
                column: "DownloadUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "DeployUrl",
                keyValue: null,
                column: "DeployUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DeployUrl",
                table: "Projects",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
