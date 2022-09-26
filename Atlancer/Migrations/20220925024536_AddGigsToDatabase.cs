using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlancer.Migrations
{
    public partial class AddGigsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gig",
                columns: table => new
                {
                    GigId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FreelancerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GigImageName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GigImageName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GigImageName3 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gig", x => x.GigId);
                    table.ForeignKey(
                        name: "FK_Gig_Freelancer_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "Freelancer",
                        principalColumn: "FreelancerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gig_FreelancerId",
                table: "Gig",
                column: "FreelancerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gig");
        }
    }
}
