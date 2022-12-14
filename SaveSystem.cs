using System;
using static System.Console;

namespace MiniComputer2
{
    class SaveSystem
    {
        public static string? appPath;
        public static string? filesPath;
        public static string? saveFilePath;
        public static bool loading = false;

        //Saves a file to the current filesPath
        static List<string> FormatFile(File file)
        {
            List<string> toWrite = new List<string>();

            toWrite.Add("{");

            toWrite.Add(file.name + "." + file.type);
            toWrite.Add(Program.FormatPath(file.path));
            toWrite.Add("c======");
            toWrite.AddRange(file.content);

            //Indentation
            for (int i = 1; i < toWrite.Count(); i++)
            {
                toWrite[i] = "    " + toWrite[i];
            }

            toWrite.Add("END}");

            return toWrite;
        }

        static List<string> FormatDirectory(Directory directory)
        {
            List<string> toWrite = new List<string>();
            toWrite.Add("{");

            toWrite.Add("    " + directory.name);
            toWrite.Add("    " + Program.FormatPath(directory.path));

            toWrite.Add("END}");

            return toWrite;
        }

        static void NewSaveFile()
        {
            Write("Creating new save file, type new name: ");

            string? newSaveName = ReadLine();
            if (string.IsNullOrWhiteSpace(newSaveName)) { newSaveName = Globals.RandomString(15); }

            Dump(newSaveName);
        }

        public static void Dump(string saveName)
        {
            if (filesPath == null!) { return; }

            if (saveName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            {
                Globals.WriteError(@$"File name '{saveName}' is not valid. File names cannot include these characters: "" * < > \ | / :");
                return;
            }
            else if (saveName.EndsWith('.') || saveName.EndsWith(' '))
            {
                Globals.WriteError($"File name '{saveName}' is not valid. File names cannot end with a space or a period.");
                return;
            }

            List<string> toWrite = new List<string>();

            toWrite.Add("DIRECTORIES:");
            for (int i = 0; i < Directory.allDirectories.Count(); i++)
            {
                if (Directory.allDirectories[i].name == Globals.rootDirName) continue;
                if (Directory.allDirectories[i].name == Globals.programsDirName) continue;
                toWrite.AddRange(FormatDirectory(Directory.allDirectories[i]));
            }

            toWrite.Add("FILES:");
            for (int i = 0; i < File.allFiles.Count(); i++)
            {
                toWrite.AddRange(FormatFile(File.allFiles[i]));
            }

            if (saveName != null) { saveFilePath = System.IO.Path.Combine(filesPath, saveName) + ".txt"; }

            if (saveFilePath == null) return;

            WriteLine("Writing to save file, do not shut down");
            System.IO.File.WriteAllLinesAsync(saveFilePath, toWrite);
            Clear();
        }

        public static void Load()
        {
            if (filesPath == null) return;
            string[] saveFiles = System.IO.Directory.GetFiles(filesPath);

            if (saveFiles.Length == 0)
            {
                WriteLine("No save file recorded.");
                NewSaveFile();
                return;
            }
            else
            {
                int selectedFile = 0;
                bool hasBeenSelected = false;

                while (hasBeenSelected == false)
                {
                    Clear();
                    WriteLine("Save file to load:");

                    for (int i = 0; i < saveFiles.Length + 1; i++)
                    {
                        string toDisplay = i == saveFiles.Length ? "none" : saveFiles[i].Split(@"\").Last();
                        if (selectedFile == i) Globals.WriteWithColor(toDisplay, ConsoleColor.White, ConsoleColor.Black);
                        else WriteLine(toDisplay);
                    }

                    ConsoleKeyInfo keyInfo = ReadKey(true);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (selectedFile <= 0) break;
                            selectedFile--;
                            break;

                        case ConsoleKey.DownArrow:
                            if (selectedFile >= saveFiles.Length) break;
                            selectedFile++;
                            break;

                        case ConsoleKey.Enter:
                            if (selectedFile == saveFiles.Length) { Clear(); NewSaveFile(); return; }
                            saveFilePath = saveFiles[selectedFile];
                            hasBeenSelected = true;
                            break;
                    }
                }

                Clear();
            }

            if (saveFilePath == null) return;
            string[] lines = System.IO.File.ReadAllLines(saveFilePath);

            if (lines.Length < 2) return;

            if (lines[0] != "DIRECTORIES:") { Globals.WriteError($"Save file invalid at line 0:{lines[0]}"); return; }

            string currentlyLoading = "Directories";

            //Item loading
            int recordStart = 0;
            //bool foundDirectory = false;
            string newName = "";
            Directory[]? newPath = new Directory[1] { Globals.rootDirectory };

            //File loading
            List<string> newFileContent = new List<string>();

            //Directory loading
            bool isLoadingDirs = true;
            List<File> newFileChildren = new List<File>();
            List<Directory> newDirChildren = new List<Directory>();
            Directory currentDirectory = Globals.rootDirectory;

            WriteLine("loading directories...");
            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i];
                if (currentLine.StartsWith("    ")) { currentLine = currentLine.Remove(0, 4); }

                if (currentlyLoading == "Directories")
                {
                    if (currentLine == "FILES:")
                    {
                        currentlyLoading = "Files";
                        WriteLine("loading files...");
                        continue;
                    }

                    //Detects begenning of directory
                    if (currentLine == "{")
                    {
                        recordStart = i;
                        newName = "";
                        newFileChildren = new List<File>();
                        newDirChildren = new List<Directory>();
                        continue;
                    }
                    //Detects directory name
                    if (i == recordStart + 1)
                    {
                        newName = currentLine;
                    }
                    //Detects directory path
                    if (i == recordStart + 2)
                    {
                        newPath = Program.UnFormatPath(currentLine);
                        if (newPath == null) { Globals.WriteError($"Save file invalid at line {i + 1}: {currentLine}"); return; }
                    }
                    //Detects end of directory creation
                    if (currentLine == "END}")
                    {
                        if (newPath == null) { Globals.WriteError($"Save file invalid at line {i + 1}: {currentLine}"); return; }
                        Directory? newDirectory = Directory.CreateDirectory(newName, newPath);
                        if (newDirectory == null) return;

                        newDirectory.files = newFileChildren;
                        newDirectory.directories = newDirChildren;
                    }
                    //Detects directories
                    if (i >= recordStart + 5 && isLoadingDirs == true)
                    {
                        newFileContent.Add(currentLine);
                        continue;
                    }

                }
                else if (currentlyLoading == "Files")
                {
                    //Detects begenning of file
                    if (currentLine == "{")
                    {
                        recordStart = i;
                        newName = "";
                        newFileContent = new List<string>();
                        continue;
                    }
                    //Detects file name and type
                    if (i == recordStart + 1)
                    {
                        newName = currentLine;
                        continue;
                    }
                    //Detects file path
                    if (i == recordStart + 2)
                    {
                        newPath = Program.UnFormatPath(currentLine);
                        continue;
                    }
                    //Detects delimitation between info and content
                    if (i == recordStart + 3 && currentLine != "c======")
                    {
                        Globals.WriteError($"Save file invalid at line {i + 1}: {currentLine}");
                        return;
                    }
                    //Detects end of file creation
                    if (currentLine == "END}")
                    {
                        if (newPath == null) { Globals.WriteError($"Save file invalid at line {i + 1}: {currentLine}"); return; }
                        File? newFile = File.CreateFile(newName, newPath);

                        if (newFile == null) return;

                        newFile.content = newFileContent;
                        continue;
                    }
                    //Detects content
                    if (i >= recordStart + 5)
                    {
                        newFileContent.Add(currentLine);
                        continue;
                    }
                }
                else
                {
                    Globals.WriteError($"Save file invalid at line {i + 1}: {currentLine}");
                    return;
                }
            }

            WriteLine($"Loaded save file <{saveFilePath}>");
        }

        public static void LoadPath()
        {
            appPath = System.IO.Directory.GetCurrentDirectory();

            if (appPath == null) { Globals.WriteError($"exe file path not found."); return; }

            filesPath = System.IO.Path.Combine(appPath, "StoredFiles");

            if (System.IO.Directory.Exists(filesPath) == false)
            {
                System.IO.Directory.CreateDirectory(filesPath);
            }
        }
    }
}