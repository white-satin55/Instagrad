using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Instagrad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorUserData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_Users_IncomingFrendshipRequestsLogin",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_Users_OutgoingFrendshipRepuestsLogin",
                table: "UserUser");

            migrationBuilder.RenameColumn(
                name: "OutgoingFrendshipRepuestsLogin",
                table: "UserUser",
                newName: "OutgoingFriendshipRequestsLogin");

            migrationBuilder.RenameColumn(
                name: "IncomingFrendshipRequestsLogin",
                table: "UserUser",
                newName: "IncomingFriendshipRequestsLogin");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_OutgoingFrendshipRepuestsLogin",
                table: "UserUser",
                newName: "IX_UserUser_OutgoingFriendshipRequestsLogin");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_Users_IncomingFriendshipRequestsLogin",
                table: "UserUser",
                column: "IncomingFriendshipRequestsLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_Users_OutgoingFriendshipRequestsLogin",
                table: "UserUser",
                column: "OutgoingFriendshipRequestsLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_Users_IncomingFriendshipRequestsLogin",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_Users_OutgoingFriendshipRequestsLogin",
                table: "UserUser");

            migrationBuilder.RenameColumn(
                name: "OutgoingFriendshipRequestsLogin",
                table: "UserUser",
                newName: "OutgoingFrendshipRepuestsLogin");

            migrationBuilder.RenameColumn(
                name: "IncomingFriendshipRequestsLogin",
                table: "UserUser",
                newName: "IncomingFrendshipRequestsLogin");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_OutgoingFriendshipRequestsLogin",
                table: "UserUser",
                newName: "IX_UserUser_OutgoingFrendshipRepuestsLogin");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_Users_IncomingFrendshipRequestsLogin",
                table: "UserUser",
                column: "IncomingFrendshipRequestsLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_Users_OutgoingFrendshipRepuestsLogin",
                table: "UserUser",
                column: "OutgoingFrendshipRepuestsLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
