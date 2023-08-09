using CycleX.Game;
using CycleX.Security;
using CycleX.Utility;

namespace CycleX
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            ValidInput validInput = new();
            validInput.Validate(args);

            Move move = new(validInput);

            HmacGenerator hmacGenerator = new(move);

            var gameManager = new GameManager(validInput, hmacGenerator, move);
            gameManager.ProcessUserMove();
            gameManager.GetWinner();
        }
    }
}