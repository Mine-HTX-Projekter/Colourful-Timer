using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Timers.Timer;

namespace StopWatchFinal {
    static class Program {
        static void Main() {
            // Prompt user
            LoggerPrimary("How long do you want to time for?");
            LoggerInfo("Example 2m 25s");

            // Instantiate TimerInput to interpret the user input
            int toTime = new TimerInput(Console.ReadLine() ?? "none").Evaluate();

            Timer timer = new Timer(toTime);
            timer.Elapsed += TimeIsOut;
            timer.Start();

            // Loop - rruns once every second
            for (int i = 0; i < toTime; i += 1000) {
                int timeLeft = toTime - i;
                Console.Clear();

                int minutes = timeLeft / 60 / 1000;
                int seconds = (timeLeft / 1000) - (minutes * 60);

                // Print time with a format and random colours
                if (minutes != 0) LoggerRandom($"{minutes}:");
                LoggerRandom(seconds.ToString().Length == 1 ? $"0{seconds}" : seconds.ToString());

                Thread.Sleep(1000);
            }

            Console.Clear();
            timer.Stop();
        }

        // Function to run once the Timer#Stop is run
        static void TimeIsOut(object? _, ElapsedEventArgs _1) {
            LoggerInfo("Time is out!");
        }

        // Set Console#ForeGroundColor to ConsoleColour#Yellow, print, then reset
        static void LoggerInfo(string toPrint) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(toPrint);
            Console.ResetColor();
        }

        // Set Console#ForeGroundColor to ConsoleColour#DarkCyan, print, then reset
        static void LoggerPrimary(string toPrint) {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(toPrint);
            Console.ResetColor();
        }

        // Set Console#ForeGroundColor to a random ConsoleColour, print, then reset
        static void LoggerRandom(string toPrint) {
            ConsoleColor[] colors = {
                ConsoleColor.Blue,
                ConsoleColor.Magenta,
                ConsoleColor.DarkRed,
                ConsoleColor.White,
                ConsoleColor.Yellow,
                ConsoleColor.Cyan
            };

            Console.ForegroundColor = colors[new Random().Next(0, colors.Length)];
            Console.Write(toPrint);
            Console.ResetColor();
        }
    }
    
    // Handle user input
    class TimerInput {
        readonly Regex _timeFormat = new(@"([0-9]+\s?[a-z]+)");
        private readonly string _toInterpret;

        public TimerInput(string toInterpret) {
            this._toInterpret = toInterpret;

            // Check if the wanted input syntax has been provided
            if (!this._timeFormat.IsMatch(this._toInterpret))
                throw new Exception("You must provide a proper time format!");
        }

        // Interpret whether minutes or seconds have been provided, then return accordingly
        private int Interpret(string input) {
            int inputVal = int.Parse(new Regex(@"[a-z]+", RegexOptions.IgnoreCase).Replace(input, String.Empty).Trim());

            string unit = new Regex(@"[0-9]+").Replace(input, String.Empty).Trim();

            if (unit.StartsWith("m")) return inputVal * 60 * 1000;
            if (unit.StartsWith("s")) return inputVal * 1000;

            throw new Exception("I can only take minutes and seconds!");
        }

        // Return the user input in milliseconds
        // Example: "2 min 15 secs" => 135,000
        public int Evaluate() {
            int toReturn = 0;

            foreach (Match match in this._timeFormat.Matches(this._toInterpret)) {
                Console.WriteLine(match.Value);
                toReturn += this.Interpret(match.Value);
            }

            return toReturn;
        }
    }
}