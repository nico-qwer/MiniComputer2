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

            while (true)
            {
                Write($"<{FormatPath(Globals.currentPath)}>: ");
                string? input = ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string command = input.Split(" ")[0];
                string[] arguments = input.Split(" ").Skip(1).ToArray();
                if (arguments == null)
                {
                    arguments = new string[0];
                }

                if (command == "quit") break;

                SearchProgram(command, arguments);
            }
        }

        static void SearchProgram(string name, string[] arguments)
        {
            for (int i = 0; i < Globals.programsDirectory.files.Count(); i++)
            {
                if (Globals.programsDirectory.files[i].name == name) continue;
                if (Globals.programsDirectory.files[i].type != "exe")
                {
                    int ErrorCode = Interpreter.Run(Globals.programsDirectory.files[i], arguments);
                    if (ErrorCode != 0) { Globals.WriteError($"Program exited with error code of {ErrorCode.ToString()}."); }
                }

                return;
            }
            Directory currentDir = Globals.currentPath.Last();
            for (int i = 0; i < currentDir.files.Count(); i++)
            {
                if (currentDir.files[i].name == name) continue;
                if (currentDir.files[i].type != "exe")
                {
                    int ErrorCode = Interpreter.Run(currentDir.files[i], arguments);
                    if (ErrorCode != 0) { Globals.WriteError($"Program exited with error code of {ErrorCode.ToString()}."); }
                }

                return;
            }
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