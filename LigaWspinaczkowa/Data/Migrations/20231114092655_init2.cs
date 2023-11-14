using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaWspinaczkowa.Data.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "Route1Holds",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "Route2Holds",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "Route3Lead",
                table: "Stage");

            migrationBuilder.AlterColumn<float>(
                name: "RouteLead3Points",
                table: "UserStage",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Route2Points",
                table: "UserStage",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Route1Points",
                table: "UserStage",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRoute1",
                table: "UserStage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRoute2",
                table: "UserStage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRoute3",
                table: "UserStage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptedRoute1",
                table: "UserStage",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptedRoute2",
                table: "UserStage",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptedRoute3",
                table: "UserStage",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRoute1",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "DateRoute2",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "DateRoute3",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "IsAcceptedRoute1",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "IsAcceptedRoute2",
                table: "UserStage");

            migrationBuilder.DropColumn(
                name: "IsAcceptedRoute3",
                table: "UserStage");

            migrationBuilder.AlterColumn<float>(
                name: "RouteLead3Points",
                table: "UserStage",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Route2Points",
                table: "UserStage",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Route1Points",
                table: "UserStage",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "UserStage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "UserStage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Route1Holds",
                table: "Stage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Route2Holds",
                table: "Stage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Route3Lead",
                table: "Stage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
