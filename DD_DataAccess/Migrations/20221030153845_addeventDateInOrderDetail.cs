using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DD_DataAccess.Migrations
{
    public partial class addeventDateInOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "OrderDetails",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "OrderDetails");
        }
    }
}
