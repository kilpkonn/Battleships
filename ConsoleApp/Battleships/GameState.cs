using System;
using ConsoleBattleshipsUi;
using ConsoleGame;

namespace Battleships
{
    public class GameState : BaseState
    {
        private readonly Game _game;
        private readonly GamePlayUi _ui;

        public GameState(Game game, GamePlayUi ui)
        {
            _game = game;
            _ui = ui;
            _ui.DropBombCallback = DropBomb;
            _ui.ExitCallback = OnExit;
            _ui.ToSetupCallback = ToSetup;
        }

        public void Step()
        {
            if (_game.GameBoard == null) return;

            bool? gameResult = _game.GameBoard.GameResult();
            if (gameResult != null)
            {
                _ui.GameOver((bool) gameResult);
                _game.PopState();
                _game.PushState(Game.GameState.Menu);
                return;
            }

            _ui.Step(_game.GameBoard);
        }

        public bool? DropBomb(int y, int x)
        {
            return _game.GameBoard?.DropBomb(y, x);
        }

        public void ToSetup()
        {
            _game.PushState(Game.GameState.Setup);
        }

        public void OnExit()
        {
            _game.PopState();
        }
    }
}