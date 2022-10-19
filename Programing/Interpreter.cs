using System;
using static System.Console;

namespace MiniComputer2
{
    class Interpreter
    {
        public static int Run(File code, string[] arguments)
        {
            string[] formattedFile = FormatCode(code);

            List<Variable> variables = new List<Variable>();

            for (int i = 0; i < formattedFile.Length; i++)
            {
                string[] tokens = formattedFile[i].Split(" ");

                switch (tokens[0])
                {
                    case "var":

                        if (tokens[3] != "=" || tokens.Length < 5) { Globals.WriteError($"\"{formattedFile[i]}\" at line {i}: variable assignment not valid."); return 1; }

                        Variable? newVar = CreateVariable(tokens);
                        if (newVar == null) { Globals.WriteError($"\"{formattedFile[i]}\" at line {i}: variable assignment not valid."); return 1; }

                        variables.Add(newVar);
                        break;

                    default:

                        break;
                }
            }

            return 0;
        }

        static string[] FormatCode(File code)
        {
            List<string> noWhiteSpaceFile = new List<string>();

            for (int i = 0; i < code.content.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(code.content[i])) continue; //Removes empty lines
                if (code.content[i].Trim().StartsWith("//")) continue;    //Removes one line comments

                // Removes end of line comments
                string toAdd = code.content[i].Trim();
                int indexOfComment = toAdd.IndexOf("//");
                if (indexOfComment != -1) { toAdd = toAdd.Remove(indexOfComment); }

                noWhiteSpaceFile.Add(toAdd);
            }

            return noWhiteSpaceFile.ToArray();
        }
        static Variable? CreateVariable(string[] tokens)
        {
            string name = tokens[2];
            string type = tokens[1];

            // Concatenates value
            string[] valueI = tokens.Skip(4).ToArray();
            string? value;
            if (valueI.Length > 1)
            {
                value = string.Join(" ", valueI);
            }
            else if (valueI.Length < 1) return null;
            else
            {
                value = valueI[0];
            }

            Variable? newVar = null;

            // Determines type and creates variable
            switch (type)
            {
                case "string":
                    value = ParseString(value);
                    if (value == null) return null;

                    newVar = new Variable<string>(name, value);
                    break;

                case "int":
                    if (!Int32.TryParse(value, out int intValue)) return null;

                    newVar = new Variable<int>(name, intValue);

                    break;

                case "float":
                    if (!float.TryParse(value, out float floatValue)) return null;

                    newVar = new Variable<float>(name, floatValue);

                    break;

                case "bool":
                    if (!ParseBool(value, out bool boolValue)) return null;

                    newVar = new Variable<bool>(name, boolValue);

                    break;


                default: return null;
            }

            return newVar;
        }

        #region parsing
        static string? ParseString(string input)
        {
            if (!input.StartsWith("\"") || !input.EndsWith("\"")) { Globals.WriteError("Variable assignment invalid."); return null; }
            if (input.Substring(1, input.Length - 2).Contains("\"")) { Globals.WriteError("Variable assignment invalid."); return null; }

            return input.Substring(1, input.Length - 2);
        }

        static bool ParseBool(string input, out bool result)
        {
            if (input == "true")
            {
                result = false;
                return true;
            }
            else if (input == "false")
            {
                result = false;
                return true;
            }
            else
            {
                result = false;
                return false;
            }
        }
        #endregion parsing
    }
}