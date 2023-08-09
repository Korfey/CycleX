using CycleX.Game;
using System.Security.Principal;

namespace CycleX.Utility
{
    internal class ValidInput
    {
        private HashSet<string> args;

        public ValidInput()
        {
            args = new HashSet<string>();
        }

        public void Validate(string[] args)
        {
            try
            {
                CheckArgs(args);
            }
            catch (ArgumentException ex)
            {
                OutputManager.PrintErrorMessage(ex.Message);
                Environment.Exit(1);
            }
        }

        private void CheckArgs(string[] args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));
            this.args = args.ToHashSet();
            CheckUniqueness(args);
            CheckLowCount();
            CheckOddCount();
        }

        private void CheckUniqueness(string[] args)
        {
            if (args.Length != GetCount())
                throw new ArgumentException(OutputManager.identicalMovesMessage);
        }

        private void CheckOddCount()
        {
            if (GetCount() % 2 != 1)
                throw new ArgumentException(OutputManager.evenMovesMessage);
        }

        private void CheckLowCount()
        {
            if (GetCount() < 3)
                throw new ArgumentException(OutputManager.fewMovesMessage);
        }

        public bool IsMoveValid(string move)
        {
            return !string.IsNullOrEmpty(move) && (move == "?" || IsNumberInRange(move));
        }

        private bool IsNumberInRange(string move)
        {
            return int.TryParse(move, out int x) && 0 <= x && x <= GetCount();
        }

        public IEnumerable<string> GetMoves() => args.AsEnumerable();

        public string GetMoveAt(int index) => GetMoves().ElementAt(index);

        public int GetCount() => args.Count;
    }
}
