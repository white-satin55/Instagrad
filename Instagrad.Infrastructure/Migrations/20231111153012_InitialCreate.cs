using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Instagrad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(type: "text", nullable: false),
                    UserLogin = table.Column<string>(type: "text", nullable: true),
                    _password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Login);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "Login");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PublisherLogin = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Users_PublisherLogin",
                        column: x => x.PublisherLogin,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    IncomingFrendshipRequestsLogin = table.Column<string>(type: "text", nullable: false),
                    OutgoingFrendshipRepuestsLogin = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => new { x.IncomingFrendshipRequestsLogin, x.OutgoingFrendshipRepuestsLogin });
                    table.ForeignKey(
                        name: "FK_UserUser_Users_IncomingFrendshipRequestsLogin",
                        column: x => x.IncomingFrendshipRequestsLogin,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUser_Users_OutgoingFrendshipRepuestsLogin",
                        column: x => x.OutgoingFrendshipRepuestsLogin,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_PublisherLogin",
                table: "Images",
                column: "PublisherLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserLogin",
                table: "Users",
                column: "UserLogin");

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_OutgoingFrendshipRepuestsLogin",
                table: "UserUser",
                column: "OutgoingFrendshipRepuestsLogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "UserUser");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
