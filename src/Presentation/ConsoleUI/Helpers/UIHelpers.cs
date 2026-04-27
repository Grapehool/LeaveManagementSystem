namespace ConsoleUI.Helpers
{
    internal static class UIHelpers
    {
        private const int TotalWidth = 100;
        private const ConsoleColor warningColor = ConsoleColor.Red;
        private const ConsoleColor successColor = ConsoleColor.Green;
        public static void PrintLine()
        {
            Console.WriteLine(new string('=', TotalWidth));
        }
        public static void PrintLine(int length)
        {
            Console.WriteLine(new string('=', length));
        }
        public static void PrintTitle(string title)
        {
            if (title.Length >= TotalWidth)
            {
                // If title is too long, just print it as-is
                Console.WriteLine(title);
                return;
            }

            int padding = TotalWidth - title.Length;
            int left = padding / 2;
            int right = padding - left;

            Console.WriteLine(
                new string('=', left) +
                title +
                new string('=', right)
            );
        }
        public static void PrintTitle(string title, int length)
        {
            if (title.Length >= length)
            {
                // If title is too long, just print it as-is
                Console.WriteLine(title);
                return;
            }

            int padding = length - title.Length;
            int left = padding / 2;
            int right = padding - left;

            Console.WriteLine(
                new string('=', left) +
                title +
                new string('=', right)
            );
        }
        public static void PrintTitle(string title, ConsoleColor color)
        {
            if (title.Length >= TotalWidth)
            {
                // If title is too long, just print it as-is
                Console.WriteLine(title);
                return;
            }

            int padding = TotalWidth - title.Length;
            int left = padding / 2;
            int right = padding - left;

            Console.ForegroundColor = color;
            Console.WriteLine(
                new string('=', left) +
                title +
                new string('=', right)
            );
            Console.ResetColor();
        }
        public static void PrintTitle(string title, char character)
        {
            if (title.Length >= TotalWidth)
            {
                // If title is too long, just print it as-is
                Console.WriteLine(title);
                return;
            }

            int padding = TotalWidth - title.Length;
            int left = padding / 2;
            int right = padding - left;

            Console.WriteLine(
                new string(character, left) +
                title +
                new string(character, right)
            );
        }
        public static void PrintTitle(string title, char character, ConsoleColor color)
        {
            if (title.Length >= TotalWidth)
            {
                // If title is too long, just print it as-is
                Console.WriteLine(title);
                return;
            }

            int padding = TotalWidth - title.Length;
            int left = padding / 2;
            int right = padding - left;

            Console.ForegroundColor = color;
            Console.WriteLine(
                new string(character, left) +
                title +
                new string(character, right)
            );
            Console.ResetColor();
        }
        public static void WriteColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void WriteWarning(string text)
        {
            WriteColor(text, warningColor);
        }
        public static void WriteSuccess(string text)
        {
            WriteColor(text, successColor);
        }
    }
}
