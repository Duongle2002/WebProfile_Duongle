using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebProfile.Migrations
{
    /// <inheritdoc />
    public partial class AddPageSectionHoverLoaderSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AboutTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonHoverShadow",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ButtonHoverTransform",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardHoverShadow",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardHoverTransform",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ContactBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ContactTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EnablePageLoader",
                table: "ThemeSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExperienceTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FooterBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FooterTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeaderBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeaderTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HeroTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageHoverShadow",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageHoverTransform",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LinkHoverColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LoaderAnimationDuration",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LoaderBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LoaderColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LoaderFadeOutDuration",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "LoaderShowOnAjax",
                table: "ThemeSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoaderShowOnNavigation",
                table: "ThemeSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LoaderSize",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LoaderType",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProjectsBackgroundColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProjectsTextColor",
                table: "ThemeSettings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "AboutTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonHoverShadow",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ButtonHoverTransform",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "CardHoverShadow",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "CardHoverTransform",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ContactBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ContactTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "EnablePageLoader",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ExperienceBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ExperienceTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "FooterBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "FooterTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "HeaderBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "HeaderTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "HeroBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "HeroTextColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ImageHoverShadow",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ImageHoverTransform",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LinkHoverColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderAnimationDuration",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderFadeOutDuration",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderShowOnAjax",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderShowOnNavigation",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderSize",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "LoaderType",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ProjectsBackgroundColor",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ProjectsTextColor",
                table: "ThemeSettings");
        }
    }
}
