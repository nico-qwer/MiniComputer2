using System;
using static System.Console;

namespace MiniComputer2
{
    class Item
    {
        public static int currentID = 0;
        public string name = "Unnamed";
        public Directory[] path = default!;

        public static void DeleteItem(string _name, Directory[] _path)
        {
            for (int i = 0; i < _path.Last().directories.Count(); i++)
            {
                if (_path.Last().directories[i].name != _name) continue;
                _path.Last().directories.RemoveAt(i);
                Directory.allDirectories.Remove(_path.Last().directories[i]);
                WriteLine($"Successfuly deleted {_name} and it's contents from {_path.Last().name}.");
                return;
            }
            for (int i = 0; i < _path.Last().files.Count(); i++)
            {
                if (_path.Last().files[i].name != _name) continue;
                _path.Last().files.RemoveAt(i);
                File.allFiles.Remove(_path.Last().files[i]);
                WriteLine($"Successfuly deleted {_name} from {_path.Last().name}.");
                return;
            }
            Globals.WriteError("No such file or directory exists.");
        }
    }
}