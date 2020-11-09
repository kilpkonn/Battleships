using System.IO;
using System.Linq;
using BattleshipsBoard;
using ConsoleBattleshipsUi;
using ConsoleGame;
using Domain;

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
                _game.Database.SaveChanges();
            }
        }

        private bool LoadGame(string file)
        {
            string jsonStr = File.ReadAllText(file);
            var jsonState = GameJsonDeserializer.FromJson(jsonStr).Deserialize();
            _game.GameBoard = GameBoard.FromJsonState(jsonState);

            // TODO: Load from db

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