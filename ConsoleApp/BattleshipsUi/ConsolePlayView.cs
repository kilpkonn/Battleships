using System;
using BattleshipsBoard;
using Renderer;
using Util;

namespace ConsoleBattleshipsUi
{
    public class ConsolePlayView: GamePlayUi
    {
        private readonly ConsoleRenderer _renderer = new(3, 2);
        public override void Step(GameBoard board)
        {
            ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            Console.WriteLine(board.WhiteToMove
                ? "White to move! Press any key to continue..."
                : "Black to move! Press any key to continue...");

            Console.ReadKey();
            var layer = board.WhiteToMove ? GameBoard.BoardType.WhiteHits : GameBoard.BoardType.BlackHits;
            bool whiteToMove = board.WhiteToMove;

            do
            {
                bool choosing = true;
                while (choosing) {
                    ConsoleUtil.WriteBlanks();
                    _renderer.RenderHits(board.Board[(int) layer]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nUser R to revert move");
                    Console.Write("Press Q to quit!");
                    var input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            _renderer.HighlightY = Math.Max(0, _renderer.HighlightY - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            _renderer.HighlightY = Math.Min(board.Height - 1,
                                _renderer.HighlightY + 1);
                            break;
                        case ConsoleKey.RightArrow:
                            _renderer.HighlightX = Math.Min(board.Width - 1,
                                _renderer.HighlightX + 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            _renderer.HighlightX = Math.Max(0, _renderer.HighlightX - 1);
                            break;
                        case ConsoleKey.Enter:
                            choosing = false;
                            break;
                        case ConsoleKey.Q:
                            ExitCallback?.Invoke();
                            return;
                        case ConsoleKey.R:
                            board.RevertMove();
                            if (!board.IsSetupComplete())
                            {
                                ToSetupCallback?.Invoke();
                            }
                            return;
                    }
                }

                bool? hit = DropBombCallback?.Invoke(_renderer.HighlightY, _renderer.HighlightX);
                ShowHitMiss(hit);
                
            } while (whiteToMove == board.WhiteToMove && board.GameResult() == null);

            _renderer.HighlightX = 0;
            _renderer.HighlightY = 0;
        }

        public override void GameOver(bool whiteWon)
        {
            ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(6, Console.WindowHeight / 2);
            if (whiteWon)
            {
                Console.WriteLine("WHITE WON!!!");
            }
            else
            {
                Console.WriteLine("BLACK WON!!!");
            }

            Console.ReadKey();
        }

        private void ShowHitMiss(bool? isHit)
        {
            if (isHit == null) return;
            
            ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(10, Console.WindowHeight / 2);
            if ((bool) isHit)
            {
                Console.WriteLine("HIT!");
            }
            else
            {
                Console.WriteLine("MISS");
            }

            Console.ReadKey();
        }
    }
}