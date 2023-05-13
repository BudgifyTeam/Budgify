using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgifyDal.Migrations
{
    /// <inheritdoc />
    public partial class addIncomesExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Users_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    PublicAccount = table.Column<bool>(type: "boolean", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Users_id);
                });

            migrationBuilder.CreateTable(
                name: "budget",
                columns: table => new
                {
                    Budget_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", maxLength: 8, nullable: false),
                    Users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget", x => x.Budget_id);
                    table.ForeignKey(
                        name: "FK_budget_users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "users",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Category_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Category_id);
                    table.ForeignKey(
                        name: "FK_categories_users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "users",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incomes",
                columns: table => new
                {
                    Income_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", maxLength: 8, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incomes", x => x.Income_id);
                    table.ForeignKey(
                        name: "FK_incomes_users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "users",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pockets",
                columns: table => new
                {
                    Pocket_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Total = table.Column<double>(type: "double precision", maxLength: 8, nullable: false),
                    Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Goal = table.Column<double>(type: "double precision", maxLength: 8, nullable: false),
                    Users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pockets", x => x.Pocket_id);
                    table.ForeignKey(
                        name: "FK_pockets_users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "users",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                columns: table => new
                {
                    Wallet_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Total = table.Column<double>(type: "double precision", maxLength: 8, nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Users_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallets", x => x.Wallet_id);
                    table.ForeignKey(
                        name: "FK_wallets_users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "users",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    Expense_id = table.Column<int>(type: "integer", maxLength: 10, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", maxLength: 8, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Users_id = table.Column<int>(type: "integer", nullable: false),
                    Pocket_id = table.Column<int>(type: "integer", nullable: false),
                    Wallet_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.Expense_id);
                    table.ForeignKey(
                        name: "FK_expenses_pockets_Pocket_id",
                        column: x => x.Pocket_id,
                        principalTable: "pockets",
                        principalColumn: "Pocket_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expenses_pockets_Wallet_id",
                        column: x => x.Wallet_id,
                        principalTable: "pockets",
                        principalColumn: "Pocket_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expenses_users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "users",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_budget_Users_id",
                table: "budget",
                column: "Users_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_Users_id",
                table: "categories",
                column: "Users_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_Pocket_id",
                table: "expenses",
                column: "Pocket_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_Users_id",
                table: "expenses",
                column: "Users_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_Wallet_id",
                table: "expenses",
                column: "Wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_incomes_Users_id",
                table: "incomes",
                column: "Users_id");

            migrationBuilder.CreateIndex(
                name: "IX_pockets_Users_id",
                table: "pockets",
                column: "Users_id");

            migrationBuilder.CreateIndex(
                name: "IX_wallets_Users_id",
                table: "wallets",
                column: "Users_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "budget");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "incomes");

            migrationBuilder.DropTable(
                name: "wallets");

            migrationBuilder.DropTable(
                name: "pockets");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
