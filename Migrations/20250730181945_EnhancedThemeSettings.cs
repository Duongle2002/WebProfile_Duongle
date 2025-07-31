using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebProfile.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedThemeSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BorderColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BorderStyle",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BorderWidth",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonOutlineColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonOutlineHoverColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonOutlineHoverTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonOutlineTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonPrimaryColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonPrimaryHoverColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonPrimaryTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonSecondaryColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonSecondaryHoverColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonSecondaryTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardPadding",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ContainerMaxWidth",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CustomCSS",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EnableHoverEffects",
                table: "ThemeSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableScrollAnimations",
                table: "ThemeSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GradientDirection",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GradientEndColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GradientMiddleColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GradientPosition",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GradientStartColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GradientType",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeadingFontFamily",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeadingFontWeight",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SectionPadding",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SpacingUnit",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "UseGradientBackground",
                table: "ThemeSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorderColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "BorderStyle",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "BorderWidth",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonOutlineColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonOutlineHoverColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonOutlineHoverTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonOutlineTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonPrimaryColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonPrimaryHoverColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonPrimaryTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonSecondaryColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonSecondaryHoverColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonSecondaryTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "CardPadding",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ContainerMaxWidth",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "CustomCSS",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "EnableHoverEffects",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "EnableScrollAnimations",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "GradientDirection",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "GradientEndColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "GradientMiddleColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "GradientPosition",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "GradientStartColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "GradientType",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "HeadingFontFamily",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "HeadingFontWeight",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "SectionPadding",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "SpacingUnit",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "UseGradientBackground",
                table: "ThemeSettings");
        }
    }
}
