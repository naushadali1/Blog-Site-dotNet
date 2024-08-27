using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Migrations
{
    /// <inheritdoc />
    public partial class updateProfileData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FatherName",
                table: "tbl_Profiles",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "tbl_Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "tbl_Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "tbl_Profiles");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "tbl_Profiles");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "tbl_Profiles",
                newName: "FatherName");
        }
    }
}
