using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text;

namespace CycleX.Utility
{
    internal class OutputManager
    {
        private const string numberColor = "seagreen1";
        private const string hmacColor = "208"; // darkorange
        private const string hmacKeyColor = "107"; // darkolivegreen3
        private const string helpColor = "magenta1";
        private const string exitColor = "red1";
        private const string panelColor = "36"; // darkcyan

        public const string identicalMovesMessage = "There were several moves with identical names!\nUse distinct names for moves.\nExample of [bold red]INVALID[/] arguments: Apple Apple 12 Baobab 12\nExample of [bold green]VALID[/] arguments: Apple Argentina 12 Baobab Sirius";

        public const string evenMovesMessage = "The arguments contained an even number of moves!\nMake sure you use an odd number of moves in arguments.\nExample of [bold red]INVALID[/] arguments: Beatles RollingStones AC/DC Queen\nExample of [bold green]VALID[/] arguments: Beatles RollingStones AC/DC Queen Metallica";

        public const string fewMovesMessage = "The arguments had too few moves!\nArguments must include at least three moves.\nExample of [bold red]INVALID[/] arguments: 1 2\nExample of [bold green]VALID[/] arguments: 1 2 3";

        public const string greetingMessage = $"You have started playing the non-transitive one-move game [bold]CycleX[/]!\nYour goal is to select one of the moves listed in the numbered menu below and input the corresponding [{numberColor} bold]number[/] or [{helpColor} bold]?[/]. To ensure fairness, you will receive the [bold {hmacColor}]HMAC[/] of the computer's move that has already been made. Once you have made your move, you will receive a [bold {hmacKeyColor}]HMAC KEY[/]. You can use it to calculate the [bold {hmacColor}]HMAC[/] and verify that the computer's move has not been altered. If you input your move incorrectly, the menu will appear again. Best of luck!";

        public const string win = "[green bold]You win![/]";
        public const string lose = "[red bold]Computer win![/]";
        public const string draw = "[yellow bold]It is a draw![/]";

        private static int prompted = 0;

        private OutputManager() { }

        private static void Print(IRenderable value)
        {
            AnsiConsole.Write(value);
        }

        public static void PrintStartMessage()
        {
            string header = "Welcome to CycleX!";
            var panel = CreatePanel(header, greetingMessage, ConvertFromString(panelColor));
            Console.Title = "CycleX";
            AnsiConsole.Clear();
            Print(panel);
        }

        public static void PrintErrorMessage(string message)
        {
            int headerEndIndex = message.IndexOf('\n');
            string header = message[..headerEndIndex];
            string content = message[(headerEndIndex + 1)..];
            var panel = CreatePanel(header, content, Color.Orange1);
            Print(panel);
        }

        private static Color ConvertFromString(string colorCode) => Color.FromInt32(int.Parse(colorCode));

        public static void PrintHmac(string hmac) 
        {
            var panel = CreatePanel("HMAC", hmac, ConvertFromString(hmacColor));
            Print(panel); 
        }

        public static void PrintHmacKey(string hmacKey)
        {
            var panel = CreatePanel("HMAC KEY", hmacKey, ConvertFromString(hmacKeyColor));
            Print(panel);
        }

        private static void PrintMenu(ValidInput validInput)
        {
            StringBuilder menu = BuildMenu(validInput);
            string specialItems = $"[{exitColor}]0[/] - exit\n[{helpColor}]?[/] - help";
            menu.Append(specialItems);
            var panel = CreatePanel("Menu", menu.ToString(), ConvertFromString(panelColor));
            Print(panel);
        }

        public static string AskUserMove(ValidInput validInput)
        {
            PrintMenu(validInput);
            var move = AnsiConsole.Ask<string>("Enter your move: ");
            return validInput.IsMoveValid(move) ? move : AskUserMove(validInput);
        }

        public static void PrintResult(string userMove, string pcMove, string result)
        {
            string moves = $"Your move: [bold yellow]{userMove}[/]\nComputer move: [bold yellow]{pcMove}[/]\n";

            var panel = CreatePanel("Result", moves + result, ConvertFromString(panelColor));
            Print(panel);
        }

        private static StringBuilder BuildMenu(ValidInput validInput)
        {
            StringBuilder menu = new("[bold]Available moves:[/]\n");
            for (int i = 1; i <= validInput.GetCount(); i++)
            {
                string menuItem = $"[{numberColor}]{i}[/] - {validInput.GetMoveAt(i - 1)}";
                menu.AppendLine(menuItem);
            }
            return menu;
        }

        public static void PrintTable(Table table)
        {
            var panel = new Panel(table)
            {
                Header = new PanelHeader("Help table")
            };
            ModifyPanelBorder(panel, ConvertFromString(panelColor));
            Print(panel);
        }

        private static Panel CreatePanel(string header, string content, Color color)
        {
            var panel = new Panel(content)
            {
                Header = new PanelHeader(header)
            };
            ModifyPanelBorder(panel, color);
            return panel;
        }

        private static void ModifyPanelBorder(Panel panel, Color color)
        {
            panel.Border = new RoundedBoxBorder();
            panel.BorderColor(color);
        }
    }
}

