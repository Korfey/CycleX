using CycleX.Utility;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleX.Game
{
    internal class HelpTableGenerator
    {
        private const string userColor = "seagreen1";
        private const string pcColor = "orange1";

        public Table Table { get; init; }
        private readonly ValidInput validInput;
        public HelpTableGenerator(ValidInput validInput)
        {
            this.validInput = validInput;
            this.Table = GenerateTable();
        }

        public Table GenerateTable()
        {
            var table = GetStartingTable();
            CreateColumnsAndRows(table);
            FillTableCells(table);
            table.Caption = BuildCaption(table);
            return table;
        }

        private TableTitle BuildCaption(Table table)
        {
            string[] moves = GetExampleMoves(); 

            string caption = $"[white]For instance, if the [bold {userColor}]USER[/] picks [bold {userColor}]{moves[1]}[/] and the [bold {pcColor}]COMPUTER[/] selects [bold {pcColor}]{moves[0]}[/], the [bold {userColor}]USER wins[/]\nIf the [bold {pcColor}]COMPUTER[/] chooses [bold {pcColor}]{moves[0]}[/] and [bold {userColor}]YOU[/] choose [bold {userColor}]{moves[2]}[/] - the [bold {pcColor}]COMPUTER wins[/][/]";

            return new TableTitle(caption);
        }

        private string[] GetExampleMoves()
        {
            int count = validInput.GetCount() - 1;
            string pcMove = validInput.GetMoveAt(count);
            string userMove1 = validInput.GetMoveAt(0);
            string userMove2 = validInput.GetMoveAt(count - 1);
            return new string[] { pcMove, userMove1, userMove2 };
        }

        private Table GetStartingTable()
        {
            var table = new Table()
                .RoundedBorder();
            table.AddColumn($"[bold {pcColor}]PC[/] \\ [bold {userColor}]USER[/]", c => c.Alignment(Justify.Center));
            return table;
        }

        private void CreateColumnsAndRows(Table table)
        {
            foreach (var move in validInput.GetMoves())
            {
                table.AddColumn($"[bold {userColor}]{move}[/]", c => c.Alignment(Justify.Center).NoWrap());
            }
            foreach (var move in validInput.GetMoves())
            {
                table.AddRow($"[bold {pcColor}]{move}[/]");
            }
        }

        private void FillTableCells(Table table)
        {
            int count = validInput.GetCount();
            for (var i = 0; i < count; i++)
            {
                for (int j = 1; j < count + 1; j++)
                {
                    GetGameOutcome(table, i, j, count);   
                }
            }
        }

        private void GetGameOutcome(Table table, int i, int j, int count)
        {
            string cellContent = i == j - 1 ? "[yellow]Draw[/]" : GetResult(i, j, count);
            table.UpdateCell(i, j, cellContent);
        }

        private string GetResult(int i, int j, int count)
        {
            return (j - i - 1 + count) % count <= count / 2 
                ? $"[{userColor}]Win[/]" : $"[{pcColor}]Lose[/]";
        }
        //Fix colors of the table, footer, header, add example and submit this piece of really good code!)
    }
}
