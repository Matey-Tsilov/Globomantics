using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Globomantics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeUsersLazy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_AssignedToId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Todo",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_AssignedToId",
                table: "Todo",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_AssignedToId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Todo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_AssignedToId",
                table: "Todo",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
