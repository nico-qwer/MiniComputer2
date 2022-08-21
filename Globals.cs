using System;
using static System.Console;

namespace MiniComputer2
{
    class Globals
    {
        public static string rootDirName = "Main";
        public static Directory rootDirectory = new Directory(rootDirName, new Directory[0], File.currentID);
        public static Directory[] currentPath = new Directory[1] { Globals.rootDirectory };
        public static File? openFile;

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
    }
}