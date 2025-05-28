using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudExchange.EntitiyFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Descriptors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Path = table.Column<string>(type: "longtext", nullable: true),
                    Weight = table.Column<long>(type: "bigint", nullable: false),
                    Uploaded = table.Column<long>(type: "bigint", nullable: false),
                    Lifetime = table.Column<int>(type: "int", nullable: false),
                    Download = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    Root = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descriptors", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Descriptors");
        }
    }
}
