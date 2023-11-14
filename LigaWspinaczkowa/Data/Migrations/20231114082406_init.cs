using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaWspinaczkowa.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Route1Holds = table.Column<int>(type: "int", nullable: false),
                    Route2Holds = table.Column<int>(type: "int", nullable: false),
                    Route3Lead = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Route1Points = table.Column<float>(type: "real", nullable: false),
                    Route2Points = table.Column<float>(type: "real", nullable: false),
                    RouteLead3Points = table.Column<float>(type: "real", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    UserStageUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStage_AspNetUsers_UserStageUserId",
                        column: x => x.UserStageUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStage_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStage_StageId",
                table: "UserStage",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStage_UserStageUserId",
                table: "UserStage",
                column: "UserStageUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStage");

            migrationBuilder.DropTable(
                name: "Stage");
        }
    }
}
