using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForMig.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Taskss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GunlukGorev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HaftalikHedefler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AylikHedefler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YillikHedefler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TamamlandiMi = table.Column<bool>(type: "bit", nullable: false),
                    SelectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taskss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Userss",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Userss", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Friendss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<int>(type: "int", nullable: false),
                    UserId2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendss_Userss_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Userss",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendss_Userss_UserId2",
                        column: x => x.UserId2,
                        principalTable: "Userss",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messagess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<int>(type: "int", nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messagess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messagess_Userss_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Userss",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messagess_Userss_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Userss",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendss_UserId1",
                table: "Friendss",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friendss_UserId2",
                table: "Friendss",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_Messagess_FromUserId",
                table: "Messagess",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messagess_ToUserId",
                table: "Messagess",
                column: "ToUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendss");

            migrationBuilder.DropTable(
                name: "Messagess");

            migrationBuilder.DropTable(
                name: "Taskss");

            migrationBuilder.DropTable(
                name: "Userss");
        }
    }
}
