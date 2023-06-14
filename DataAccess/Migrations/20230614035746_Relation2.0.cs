using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Relation20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectsEmployees",
                table: "ProjectsEmployees");

            migrationBuilder.DropIndex(
                name: "IX_ProjectsEmployees_EmployeeId",
                table: "ProjectsEmployees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectsEmployees",
                table: "ProjectsEmployees",
                columns: new[] { "EmployeeId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsEmployees_ProjectId",
                table: "ProjectsEmployees",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectsEmployees",
                table: "ProjectsEmployees");

            migrationBuilder.DropIndex(
                name: "IX_ProjectsEmployees_ProjectId",
                table: "ProjectsEmployees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectsEmployees",
                table: "ProjectsEmployees",
                columns: new[] { "ProjectId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsEmployees_EmployeeId",
                table: "ProjectsEmployees",
                column: "EmployeeId");
        }
    }
}
