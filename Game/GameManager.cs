using CycleX.Security;
using CycleX.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CycleX.Game
{
    internal class GameManager
    {
        private readonly HelpTableGenerator helpTable;
        private readonly ValidInput validInput;
        private int userMove;
        private readonly Move move;
        private readonly string key;

        public GameManager(ValidInput validInput, HmacGenerator hmac, Move move)
        {
            this.helpTable = new HelpTableGenerator(validInput);
            this.validInput = validInput;
            this.move = move;
            this.key = hmac.GetKey();
            OutputManager.PrintStartMessage();
            OutputManager.PrintHmac(hmac.GetHmac());
        }

        public void ProcessUserMove()
        {
            string userInput = OutputManager.AskUserMove(validInput);
            if (userInput == "?")
            {
                OutputManager.PrintTable(this.helpTable.Table);
                ProcessUserMove();
            }
            else if (userInput == "0")
            {
                Environment.Exit(0);
            }
            else
            {
                this.userMove = int.Parse(userInput) - 1;
            }
        }

        public void GetWinner()
        {
            string pc = move.PcMove;
            string user = validInput.GetMoveAt(userMove);
            string result = GetResult();
            OutputManager.PrintResult(user, pc, result);
            OutputManager.PrintHmacKey(key);
        }

        public string GetResult() 
        {
            int count = validInput.GetCount();
            int difference = (userMove - move.PcMoveIndex + count) % count;

            if (difference == 0)
            {
                return OutputManager.draw;
            }
            else if (difference <= count / 2)
            {
                return OutputManager.win;
            }
            else
            {
                return OutputManager.lose;
            }
        }
    }
}

