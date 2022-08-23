using System;
using static System.Console;

namespace MiniComputer2
{
    class Globals
    {
        public static string rootDirName = "Main";
        public static string programsDirName = "Programs";
        public static Directory rootDirectory = new Directory(rootDirName, new Directory[0]);
        public static Directory programsDirectory = new Directory(programsDirName, new Directory[1] { rootDirectory });
        public static Directory[] currentPath = new Directory[1] { rootDirectory };

        public static Random RNG = new Random();

        public static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            Write("Error: ");
            WriteLine(text);
            ResetColor();
        }

        public static void WriteWithColor(string text, ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor txtColor = ConsoleColor.White)
        {
            BackgroundColor = bgColor;
            ForegroundColor = txtColor;
            WriteLine(text);
            ResetColor();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[RNG.Next(s.Length)]).ToArray());
        }
    }
}