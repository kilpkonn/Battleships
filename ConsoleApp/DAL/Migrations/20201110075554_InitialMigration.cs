using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    GameSessionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    TouchMode = table.Column<int>(nullable: false),
                    BackToBackMovesOnHit = table.Column<bool>(nullable: false),
                    BoardWidth = table.Column<int>(nullable: false),
                    BoardHeight = table.Column<int>(nullable: false),
                    PlayerWhiteId = table.Column<int>(nullable: false),
                    PlayerBlackId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.GameSessionId);
                    table.ForeignKey(
                        name: "FK_GameSessions_Players_PlayerBlackId",
                        column: x => x.PlayerBlackId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameSessions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameSessions_Players_PlayerWhiteId",
                        column: x => x.PlayerWhiteId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardStates",
                columns: table => new
                {
                    BoardStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhiteToMove = table.Column<bool>(nullable: false),
                    GameSessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardStates", x => x.BoardStateId);
                    table.ForeignKey(
                        name: "FK_BoardStates_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Boats",
                columns: table => new
                {
                    BoatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lenght = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    GameSessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boats", x => x.BoatId);
                    table.ForeignKey(
                        name: "FK_Boats_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardTiles",
                columns: table => new
                {
                    BoardTileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardStateId = table.Column<int>(nullable: false),
                    CoordX = table.Column<int>(nullable: false),
                    CoordY = table.Column<int>(nullable: false),
                    TileWhiteShips = table.Column<int>(nullable: false),
                    TileBlackShips = table.Column<int>(nullable: false),
                    TileWhiteHits = table.Column<int>(nullable: false),
                    TileBlackHits = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardTiles", x => x.BoardTileId);
                    table.ForeignKey(
                        name: "FK_BoardTiles_BoardStates_BoardStateId",
                        column: x => x.BoardStateId,
                        principalTable: "BoardStates",
                        principalColumn: "BoardStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardStates_GameSessionId",
                table: "BoardStates",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTiles_BoardStateId",
                table: "BoardTiles",
                column: "BoardStateId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTiles_CoordX",
                table: "BoardTiles",
                column: "CoordX");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTiles_CoordY",
                table: "BoardTiles",
                column: "CoordY");

            migrationBuilder.CreateIndex(
                name: "IX_Boats_GameSessionId",
                table: "Boats",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_PlayerBlackId",
                table: "GameSessions",
                column: "PlayerBlackId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_PlayerId",
                table: "GameSessions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_PlayerWhiteId",
                table: "GameSessions",
                column: "PlayerWhiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardTiles");

            migrationBuilder.DropTable(
                name: "Boats");

            migrationBuilder.DropTable(
                name: "BoardStates");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
