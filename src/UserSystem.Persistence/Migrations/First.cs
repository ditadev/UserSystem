#nullable disable

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Persistence.Migrations;

public partial class First : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "Roles",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Roles", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmailAddress = table.Column<string>("varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>("longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>("longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>("longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>("longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VerifiedAt = table.Column<DateTime>("datetime(6)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "RoleUser",
                table => new
                {
                    RolesId = table.Column<int>("int", nullable: false),
                    UsersId = table.Column<long>("bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        "FK_RoleUser_Roles_RolesId",
                        x => x.RolesId,
                        "Roles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RoleUser_Users_UsersId",
                        x => x.UsersId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            "Roles",
            "Id",
            1);

        migrationBuilder.InsertData(
            "Roles",
            "Id",
            2);

        migrationBuilder.CreateIndex(
            "IX_RoleUser_UsersId",
            "RoleUser",
            "UsersId");

        migrationBuilder.CreateIndex(
            "IX_Users_EmailAddress",
            "Users",
            "EmailAddress",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "RoleUser");

        migrationBuilder.DropTable(
            "Roles");

        migrationBuilder.DropTable(
            "Users");
    }
}