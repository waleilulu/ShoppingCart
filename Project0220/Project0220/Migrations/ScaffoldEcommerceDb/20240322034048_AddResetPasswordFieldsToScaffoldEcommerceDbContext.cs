using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project0220.Migrations.ScaffoldEcommerceDb
{
    /// <inheritdoc />
    public partial class AddResetPasswordFieldsToScaffoldEcommerceDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiration",
                table: "Customers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiration",
                table: "Customers");
        }
    }
}
