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
                name: "BoardState",
                columns: table => new
                {
                    BoardStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhiteToMove = table.Column<bool>(nullable: false),
                    GameSessionId = table.Column<int>(nullable: false),
                    GameSessionId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardState", x => x.BoardStateId);
                    table.ForeignKey(
                        name: "FK_BoardState_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardState_GameSessions_GameSessionId1",
                        column: x => x.GameSessionId1,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "BoardTile",
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
                    TileBlackHits = table.Column<int>(nullable: false),
                    BoardStateId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardTile", x => x.BoardTileId);
                    table.ForeignKey(
                        name: "FK_BoardTile_BoardState_BoardStateId",
                        column: x => x.BoardStateId,
                        principalTable: "BoardState",
                        principalColumn: "BoardStateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardTile_BoardState_BoardStateId1",
                        column: x => x.BoardStateId1,
                        principalTable: "BoardState",
                        principalColumn: "BoardStateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardState_GameSessionId",
                table: "BoardState",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardState_GameSessionId1",
                table: "BoardState",
                column: "GameSessionId1");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTile_BoardStateId",
                table: "BoardTile",
                column: "BoardStateId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTile_BoardStateId1",
                table: "BoardTile",
                column: "BoardStateId1");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTile_CoordX",
                table: "BoardTile",
                column: "CoordX");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTile_CoordY",
                table: "BoardTile",
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
                name: "BoardTile");

            migrationBuilder.DropTable(
                name: "Boats");

            migrationBuilder.DropTable(
                name: "BoardState");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
