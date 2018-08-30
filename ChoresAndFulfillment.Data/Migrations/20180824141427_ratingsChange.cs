using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChoresAndFulfillment.Migrations
{
    public partial class ratingsChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_MessageReceiverId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_MessageSenderId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "WorkerAccounts");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "EmployerAccounts");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_MessageSenderId",
                table: "Messages",
                newName: "IX_Messages_MessageSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_MessageReceiverId",
                table: "Messages",
                newName: "IX_Messages_MessageReceiverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(nullable: false),
                    UserGivingRatingId = table.Column<int>(nullable: false),
                    UserGivingRatingId1 = table.Column<string>(nullable: true),
                    UserReceivingRatingId = table.Column<int>(nullable: false),
                    UserReceivingRatingId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_UserGivingRatingId1",
                        column: x => x.UserGivingRatingId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_UserReceivingRatingId1",
                        column: x => x.UserReceivingRatingId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserGivingRatingId1",
                table: "Ratings",
                column: "UserGivingRatingId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserReceivingRatingId1",
                table: "Ratings",
                column: "UserReceivingRatingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_MessageReceiverId",
                table: "Messages",
                column: "MessageReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_MessageSenderId",
                table: "Messages",
                column: "MessageSenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_MessageReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_MessageSenderId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageSenderId",
                table: "Comments",
                newName: "IX_Comments_MessageSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageReceiverId",
                table: "Comments",
                newName: "IX_Comments_MessageReceiverId");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "WorkerAccounts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "EmployerAccounts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_MessageReceiverId",
                table: "Comments",
                column: "MessageReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_MessageSenderId",
                table: "Comments",
                column: "MessageSenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
