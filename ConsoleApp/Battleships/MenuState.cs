using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleshipsBoard;
using ConsoleBattleshipsUi;
using ConsoleGame;
using Domain;
using Microsoft.EntityFrameworkCore;
using BoardState = Domain.BoardState;

namespace Battleships
{
    public class MenuState : BaseState
    {
        private readonly Game _game;
        private readonly GameMenuUi _menuUi;

        public MenuState(Game game, GameMenuUi menuUi)
        {
            _game = game;
            _menuUi = menuUi;
            _menuUi.StartGameCallback = StartGame;
            _menuUi.SaveCallback = SaveGame;
            _menuUi.LoadGameCallback = LoadGame;
            _menuUi.ExitCallback = Exit;
        }

        public void Step()
        {
            _menuUi.Step();
        }

        private void StartGame(Configuration.Configuration config)
        {
            _game.GameBoard = new GameBoard(
                config.BoardWidth,
                config.BoardHeight,
                config.ShipCounts,
                config.TouchMode,
                config.BackToBackMovesOnHit
            );
            _game.PushState(Game.GameState.Setup);
        }

        private void SaveGame(string filename)
        {
            if (_game.GameBoard == null) return;

            if (filename.EndsWith(".json"))
            {
                GameJsonSerializer serializer = GameJsonSerializer.FromGameBoard(_game.GameBoard);
                serializer.SaveToFile(filename);
            }
            else
            {
                Player playerWhite = _game.Database.Players.First(x => x.Name == "Player White");
                Player playerBlack = _game.Database.Players.First(x => x.Name == "Player Black");
                GameSession gameSession = new GameSession(
                    filename,
                    _game.GameBoard.TouchMode,
                    _game.GameBoard.BackToBackHits,
                    _game.GameBoard.Width,
                    _game.GameBoard.Height,
                    playerWhite,
                    playerBlack
                );
                List<Boat> boats = _game.GameBoard.ShipCounts
                    .Select(x => new Boat(x.Key, x.Value, gameSession))
                    .ToList();

                List<BoardState> boardStates = new List<BoardState>();
                IEnumerable<BoardTile> tiles = _game.GameBoard.BoardHistory
                    .SelectMany(s =>
                    {
                        var state = new BoardState(gameSession, s.WhiteToMove);
                        boardStates.Add(state);
                        List<BoardTile> boardTiles = new List<BoardTile>();
                        for (int y = 0; y < s.Board[0].GetLength(0); y++)
                        {
                            for (int x = 0; x < s.Board[0].GetLength(1); x++)
                            {
                                boardTiles.Add(
                                    new BoardTile(state, x, y,
                                        s.Board[(int) GameBoard.BoardType.WhiteShips][y, x],
                                        s.Board[(int) GameBoard.BoardType.BlackShips][y, x],
                                        s.Board[(int) GameBoard.BoardType.WhiteHits][y, x],
                                        s.Board[(int) GameBoard.BoardType.BlackHits][y, x]));
                            }
                        }

                        return boardTiles;
                    });

                _game.Database.GameSessions.Add(gameSession);
                _game.Database.Boats.AddRange(boats);
                _game.Database.BoardStates.AddRange(boardStates);
                _game.Database.BoardTiles.AddRange(tiles);
                _game.Database.SaveChanges();
            }
        }

        private bool LoadGame(string file)
        {
            if (file.EndsWith(".json"))
            {
                string jsonStr = File.ReadAllText(file);
                var jsonState = GameJsonDeserializer.FromJson(jsonStr).Deserialize();
                _game.GameBoard = GameBoard.FromJsonState(jsonState);
            }
            else
            {
                var session = _game.Database.GameSessions
                    .Where(x => x.Name == file)
                    .Include(x => x.Boats)
                    .Include(x => x.BoardStates)
                        .ThenInclude(s => s.BoardTiles)
                    .Include(x => x.PlayerWhite)
                    .Include(x => x.PlayerBlack)
                    .First();
                _game.GameBoard = GameBoard.FromGameSession(session);
            }

            if (_game.GameBoard == null) return false;

            if (_game.GameBoard.IsSetup) _game.PushState(Game.GameState.Setup);
            else _game.PushState(Game.GameState.Game);
            return true;
        }

        private void Exit()
        {
            _game.Exit();
        }
    }
}