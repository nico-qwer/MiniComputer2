using System;
using static System.Console;

namespace MiniComputer2
{
    class Interpreter
    {
        public static void Run(File toRun, string[] arguments)
        {
            string[]? code = PreProcess(toRun.content);
            if (code == null) return;

            Function[]? functions = DetectFunctions(code);
            if (functions == null) return;

            List<Variable> variables = new List<Variable>();

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i].StartsWith("var"))
                {
                    string[] tokens = code[i].Split(" ");

                    if (tokens.Length < 5) { Globals.WriteError($"Line {i.ToString()}: Variable declaration incorrect."); return; }

                    string name = tokens[2];

                    if (Enum.TryParse(tokens[1], out varType newType) == false)
                    {
                        Globals.WriteError($"Line {i.ToString()}: Variable type incorrect.");
                        return;
                    }

                    if (name.Contains("/") | name.Contains("(") | name.Contains(")") | name.Contains(".") | name.Contains(","))
                    {
                        Globals.WriteError($"Line {i.ToString()}: Variable name cannot contain /().,");
                        return;
                    }

                    if (tokens[3] != "=") { Globals.WriteError($"Line {i.ToString()}: Variable declaration incorrect."); return; }

                    string value = "";

                    if (tokens[4].Contains("("))
                    {
                        string funcName = tokens[4].Split("(")[0];
                        for (int j = 0; j < functions.Length; j++)
                        {

                        }
                    }

                    if (tokens[4].StartsWith("\"") && tokens.Last().EndsWith("\"") && newType == varType.stri) // String
                    {
                        tokens[4].Remove(0, 1);
                        tokens.Last().Remove(tokens.Last().Length - 2, 1);

                        for (int j = 4; j < tokens.Length - 5; j++)
                        {
                            value += tokens[j];
                        }
                    }
                    else if (newType == varType.floa && float.TryParse(tokens[4], out _)) // Float
                    {
                        value = tokens[4];
                    }
                    else if (newType == varType.inte && int.TryParse(tokens[4], out _)) // Int
                    {
                        value = tokens[4];
                    }
                    else
                    {
                        Globals.WriteError($"Line {i.ToString()}: Variable assignment does not match specified type.");
                        return;
                    }

                    variables.Add(new Variable(name, newType, value));
                }
                else if (code[i].StartsWith("Out"))
                {
                    string tokens = code[i].Split("(")[0];

                }
                else
                {

                }
            }
        }

        static string[]? PreProcess(List<string> unProcessedCode)
        {
            if (unProcessedCode[0] == "#add<STDIO>")
            {
                List<string>? STDIO = GetSTDIO();
                if (STDIO == null) return null;

                List<string> userCode = unProcessedCode;

                unProcessedCode = STDIO;
                unProcessedCode.AddRange(userCode);
            }
            for (int i = 0; i < unProcessedCode.Count(); i++)
            {
                for (int j = 0; j < unProcessedCode[i].Length; j++)
                {
                    if (unProcessedCode[i][j] == '/' && unProcessedCode[i][j + 1] == '/')
                    {
                        unProcessedCode[i] = unProcessedCode[i].Remove(i);
                    }
                }

                if (string.IsNullOrWhiteSpace(unProcessedCode[i]))
                {
                    unProcessedCode.RemoveAt(i);
                    i--;
                }

                unProcessedCode[i].TrimStart();

            }

            return unProcessedCode.ToArray();
        }

        static Function[]? DetectFunctions(string[] code)
        {
            List<Function> functions = new List<Function>();

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i].StartsWith("func"))
                {
                    string[] splitLine = code[i].Split(" ");

                    string returnType = splitLine[1];

                    string name = "";
                    int index = splitLine[2].IndexOf("(");
                    if (index >= 0) { name = splitLine[2].Substring(0, index); }
                    else { Globals.WriteError($"Line {i.ToString()}: Function declaration incorrect."); return null; }

                    if (name.Contains("/") | name.Contains(")") | name.Contains(".") | name.Contains(","))
                    {
                        Globals.WriteError($"Line {i.ToString()}: Function name cannot contain /).,");
                        return null;
                    }

                    for (int j = 0; j < functions.Count(); j++)
                    {
                        if (functions[j].name == name) { Globals.WriteError($"Line {i.ToString()}: Function name already taken."); return null; }
                    }

                    int start = i;
                    int lenght = 0;
                    for (int j = i; j < code.Length; j++)
                    {
                        if (code[j] == "func END")
                        {
                            lenght = j;
                            i = j;
                            break;
                        }
                    }
                    if (lenght == 0) { Globals.WriteError($"Line {i.ToString()}: Function never closed."); return null; }

                    functions.Add(new Function(name, returnType, start, lenght));
                }
            }

            return functions.ToArray();
        }

        static List<string>? GetSTDIO()
        {
            File? STDIOFile = Globals.rootDirectory.FindFileInChildren("STDIO.cod");
            if (STDIOFile == null) { Globals.WriteError("\"STDIO.cod\" is missing at root directory."); return null; }
            return STDIOFile.content;
        }
    }
}