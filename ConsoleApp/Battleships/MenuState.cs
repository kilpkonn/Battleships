using System.IO;
using Battleships.Serializer;
using ConsoleBattleships;
using ConsoleGame;

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

        private void StartGame(int width, int height)
        {
            _game.GameBoard = new GameBoard(width, height);
            _game.PushState(Game.GameState.Setup);
        }

        private void SaveGame(string filename)
        {
            GameJsonSerializer serializer = GameJsonSerializer.FromGame(_game);
            serializer.SaveToFile(filename);
        }

        private bool LoadGame(string file)
        {
            string jsonStr = File.ReadAllText(file);
            var jsonState = GameJsonDeserializer.FromJson(jsonStr).Deserialize();
            _game.GameBoard = GameBoard.FromJsonState(jsonState);
            if (_game.GameBoard == null)
            {
                return false;
            }

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