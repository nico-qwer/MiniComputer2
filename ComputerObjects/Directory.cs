using System;
using static System.Console;

namespace MiniComputer2
{
    class Directory : Item
    {
        public static List<Directory> allDirectories = new List<Directory>();

        public List<Directory> directories = new List<Directory>();
        public List<File> files = new List<File>();

        public int ID;

        public static Directory? CreateDirectory(string dirName, Directory[] newPath, int newID = -1)
        {
            if (dirName == null || dirName == "") { Globals.WriteError("Cannot create directory with no name."); return null; }
            if (dirName.Contains("+")) { Globals.WriteError("Invalid directory name."); return null; }

            if (Globals.currentPath.Last().FindFileInChildren(dirName) != null) { Globals.WriteError("Name is already used."); return null; }
            if (Globals.currentPath.Last().FindDirectoryInChildren(dirName) != null) { Globals.WriteError("Name is already used."); return null; }

            Directory newDir = new Directory(dirName, Globals.currentPath, newID);

            return newDir;
        }

        public Directory(string newName, Directory[] newPath, int newID)
        {
            Rename(newName);
            path = newPath;

            if (newID == -1)
            {
                ID = Item.currentID;
                Item.currentID++;
            }
            else
            {
                ID = newID;
            }

            if (newName == Globals.rootDirName) return;

            newPath.Last().directories.Add(this);
            allDirectories.Add(this);
        }

        public void Rename(string newName)
        {
            if (newName == null || newName == " ") { Globals.WriteError("Cannot rename to nothing."); return; }
            if (newName.Contains("/") || newName.Contains(" ")) { Globals.WriteError("Directory name cannot include / or space."); return; }
            name = newName;
        }

        public Directory? FindDirectoryInChildren(string nameToFind)
        {
            for (int i = 0; i < directories.Count(); i++)
            {
                if (directories[i].name == nameToFind) { return directories[i]; }
            }

            return null;
        }

        public File? FindFileInChildren(string nameToFind)
        {
            for (int i = 0; i < files.Count(); i++)
            {
                if (files[i].name == nameToFind) { return files[i]; }
            }

            return null;
        }

        public void Delete()
        {
            for (int i = 0; i < directories.Count(); i++)
            {
                directories[i].Delete();
            }
            for (int i = 0; i < files.Count(); i++)
            {
                files[i].Delete();
            }

            allDirectories.Remove(this);
        }

        public static Directory[]? FindPath(string[] path)
        {
            List<Directory> newPath = new List<Directory>();
            newPath.Add(Globals.rootDirectory);

            for (int i = 1; i < path.Length; i++)
            {
                Directory? foundDir = newPath.Last().FindDirectoryInChildren(path[i]);
                if (foundDir == null) { Globals.WriteError($"Could not find directory {path[i]} inside of {newPath.Last().name}"); return null; }
                newPath.Add(foundDir);
            }

            return newPath.ToArray();
        }
    }
}