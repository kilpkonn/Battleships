using ConsoleBattleshipsUi;
using ConsoleGame;

namespace Battleships
{
    public class SetupState : BaseState
    {
        private readonly Game _game;
        private readonly GameSetupUi _ui;

        public SetupState(Game game, GameSetupUi ui)
        {
            _ui = ui;
            _game = game;

            _ui.ExitCallback = OnExit;
            _ui.PlaceShipCallback = PlaceShip;
        }

        public void Step()
        {
            if (_game.GameBoard == null) return;
            if (_game.GameBoard.IsSetupComplete())
            {
                _game.PushState(Game.GameState.Game);
                return;
            }
            _ui.Step(_game.GameBoard);
        }

        public bool PlaceShip(int y, int x, int length, bool horizontal)
        {
            return _game.GameBoard?.PlaceShip(y, x, length, horizontal) ?? false;
        }
        
        public void OnExit()
        {
            _game.PopState();
        }
    }
}