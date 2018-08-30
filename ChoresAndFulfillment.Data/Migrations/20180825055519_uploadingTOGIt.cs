using Microsoft.EntityFrameworkCore.Migrations;

namespace ChoresAndFulfillment.Migrations
{
    public partial class uploadingTOGIt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserGivingRatingId1",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserReceivingRatingId1",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserGivingRatingId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "UserReceivingRatingId1",
                table: "Ratings",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "UserReceivingRatingId",
                table: "Ratings",
                newName: "JobAndWorkerId");

            migrationBuilder.RenameColumn(
                name: "UserGivingRatingId1",
                table: "Ratings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserReceivingRatingId1",
                table: "Ratings",
                newName: "IX_Ratings_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserGivingRatingId1",
                table: "Ratings",
                newName: "IX_Ratings_UserId");

            migrationBuilder.AddColumn<int>(
                name: "JobAndWorkerJobId",
                table: "Ratings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobAndWorkerWorkerAccountId",
                table: "Ratings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_JobAndWorkerWorkerAccountId_JobAndWorkerJobId",
                table: "Ratings",
                columns: new[] { "JobAndWorkerWorkerAccountId", "JobAndWorkerJobId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId1",
                table: "Ratings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_WorkerAccountApplications_JobAndWorkerWorkerAccountId_JobAndWorkerJobId",
                table: "Ratings",
                columns: new[] { "JobAndWorkerWorkerAccountId", "JobAndWorkerJobId" },
                principalTable: "WorkerAccountApplications",
                principalColumns: new[] { "WorkerAccountId", "JobId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId1",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_WorkerAccountApplications_JobAndWorkerWorkerAccountId_JobAndWorkerJobId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_JobAndWorkerWorkerAccountId_JobAndWorkerJobId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "JobAndWorkerJobId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "JobAndWorkerWorkerAccountId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Ratings",
                newName: "UserReceivingRatingId1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ratings",
                newName: "UserGivingRatingId1");

            migrationBuilder.RenameColumn(
                name: "JobAndWorkerId",
                table: "Ratings",
                newName: "UserReceivingRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserId1",
                table: "Ratings",
                newName: "IX_Ratings_UserReceivingRatingId1");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                newName: "IX_Ratings_UserGivingRatingId1");

            migrationBuilder.AddColumn<int>(
                name: "UserGivingRatingId",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserGivingRatingId1",
                table: "Ratings",
                column: "UserGivingRatingId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserReceivingRatingId1",
                table: "Ratings",
                column: "UserReceivingRatingId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
