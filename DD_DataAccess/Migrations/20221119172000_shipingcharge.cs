using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DD_DataAccess.Migrations
{
    public partial class shipingcharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ItemTotal",
                table: "OrderHeaders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShippingCharge",
                table: "OrderHeaders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemTotal",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ShippingCharge",
                table: "OrderHeaders");
        }
    }
}
