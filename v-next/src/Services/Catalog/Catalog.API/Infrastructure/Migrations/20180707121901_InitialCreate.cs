using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "beverage_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "beverage_style_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "beverage_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "manufacturer_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "BeverageType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeverageStyle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    BeverageTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeverageStyle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeverageStyle_BeverageType_BeverageTypeId",
                        column: x => x.BeverageTypeId,
                        principalTable: "BeverageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeverageStyle_BeverageStyle_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BeverageStyle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Beverage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AlcoholPercent = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    Volume = table.Column<int>(nullable: true),
                    Url = table.Column<string>(maxLength: 200, nullable: true),
                    BeverageStyleId = table.Column<int>(nullable: false),
                    BeverageTypeId = table.Column<int>(nullable: false),
                    ManufacturerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beverage_BeverageStyle_BeverageStyleId",
                        column: x => x.BeverageStyleId,
                        principalTable: "BeverageStyle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Beverage_BeverageType_BeverageTypeId",
                        column: x => x.BeverageTypeId,
                        principalTable: "BeverageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Beverage_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beverage_BeverageStyleId",
                table: "Beverage",
                column: "BeverageStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Beverage_BeverageTypeId",
                table: "Beverage",
                column: "BeverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Beverage_ManufacturerId",
                table: "Beverage",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_BeverageStyle_BeverageTypeId",
                table: "BeverageStyle",
                column: "BeverageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BeverageStyle_ParentId",
                table: "BeverageStyle",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beverage");

            migrationBuilder.DropTable(
                name: "BeverageStyle");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropTable(
                name: "BeverageType");

            migrationBuilder.DropSequence(
                name: "beverage_hilo");

            migrationBuilder.DropSequence(
                name: "beverage_style_hilo");

            migrationBuilder.DropSequence(
                name: "beverage_type_hilo");

            migrationBuilder.DropSequence(
                name: "manufacturer_hilo");
        }
    }
}
