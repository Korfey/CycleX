using CycleX.Utility;
using System.Security.Cryptography;

namespace CycleX.Game
{
    internal class Move
    {
        public string PcMove { get; private set; }
        public int PcMoveIndex { get; private set; }
        
        public Move(ValidInput input)
        {
            PcMoveIndex = RandomNumberGenerator.GetInt32(input.GetCount());
            PcMove = input.GetMoveAt(PcMoveIndex);
        }

    }
}
