using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ecobooksi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    PriceFifty = table.Column<double>(type: "float", nullable: false),
                    PriceHundred = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Author", "Description", "ISBN", "ListPrice", "Price", "PriceFifty", "PriceHundred", "Title" },
                values: new object[,]
                {
                    { 1, "John Doe", "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "1234567890", 99.0, 90.0, 85.0, 80.0, "Book One" },
                    { 2, "Jane Doe", "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "1234567891", 120.0, 100.0, 90.0, 80.0, "Book Two" },
                    { 3, "John Smith", "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "1234567892", 150.0, 130.0, 120.0, 100.0, "Book Three" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
