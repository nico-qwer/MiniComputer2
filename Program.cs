using System;
using static System.Console;

namespace MiniComputer2
{
    internal class Program
    {
        static void Main()
        {
            Title = "MiniComputer";

            SaveSystem.LoadPath();
            SaveSystem.Load();

            WriteLine("\n==================== Booted! ====================\n");

            WriteLine("Press any key to create save file...");
            ReadKey();
            SaveSystem.Dump("save1");

            ReadKey();
        }


        public static string FormatPath(Directory[] path)
        {
            string result = "";
            for (int i = 0; i < path.Length; i++)
            {
                result += path[i].name;
            }
            return result;
        }

        public static Directory[]? UnFormatPath(string path)
        {
            string[] splitedPath = path.Split("/");
            Directory[]? output = null;

            if (splitedPath.Length == 1 && splitedPath[0] != Globals.rootDirName)
            {
                Directory? directory = Globals.currentPath.Last().FindDirectoryInChildren(splitedPath[0]);
                if (directory != null)
                {
                    output = new Directory[directory.path.Count() + 1];

                    for (int i = 0; i < directory.path.Count(); i++)
                    {
                        output[i] = directory.path[i];
                    }

                    Directory? lastDir = Globals.currentPath.Last().FindDirectoryInChildren(splitedPath[0]);
                    if (lastDir != null)
                    {
                        output[output.Length - 1] = lastDir;
                    }
                }
                else Globals.WriteError("No such directory exists.");


            }
            else
            {
                output = Directory.FindPath(splitedPath);
            }

            return output;
        }
    }
}