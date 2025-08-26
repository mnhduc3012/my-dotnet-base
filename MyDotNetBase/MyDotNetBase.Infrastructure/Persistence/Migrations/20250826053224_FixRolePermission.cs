using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDotNetBase.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRolePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permission",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "ix_role_permission_role_id",
                table: "role_permission");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "role_permission",
                newName: "permission");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permission",
                table: "role_permission",
                columns: new[] { "role_id", "permission" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permission",
                table: "role_permission");

            migrationBuilder.RenameColumn(
                name: "permission",
                table: "role_permission",
                newName: "value");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permission",
                table: "role_permission",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "ix_role_permission_role_id",
                table: "role_permission",
                column: "role_id");
        }
    }
}
