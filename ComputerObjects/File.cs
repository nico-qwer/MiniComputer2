
namespace MiniComputer2
{
    class File : Item
    {
        public static List<File> allFiles = new List<File>();

        public List<string> content = new List<string>() { "" };
        public string type;

        public static File? CreateFile(string fileName, Directory[] newPath)
        {
            File newFile = new File(fileName, newPath);
            return newFile;
        }

        public File(string newName, Directory[] newPath)
        {
            this.Rename(newName);
            path = newPath;

            if (name == null) { name = Globals.RandomString(10); }
            if (type == null) { type = "txt"; }

            allFiles.Add(this);
        }

        public void Rename(string newName)
        {
            if (Globals.currentPath.Last().FindFileInChildren(newName) != null) { Globals.WriteError("Name is already used."); return; }
            if (Globals.currentPath.Last().FindDirectoryInChildren(newName) != null) { Globals.WriteError("Name is already used."); return; }

            string[] splitName = newName.Split('.');
            if (splitName.Length > 2) { Globals.WriteError("Cannot have multiple file types."); return; }
            if (splitName.Length == 2)
            {
                if (splitName[0] == null || splitName[0] == " ") { Globals.WriteError("Cannot rename to nothing."); return; }
                if (splitName[1] == null || splitName[1].Length != 3) { Globals.WriteError("File type must be 3 characters."); return; }

                if (splitName[0].Contains("/") || splitName[0].Contains(" ")) { Globals.WriteError("File name cannot include / or space."); return; }
                if (splitName[1].Contains("/") || splitName[1].Contains(" ")) { Globals.WriteError("File name cannot include / or space."); return; }

                name = splitName[0];
                type = splitName[1];
                return;
            }
            if (splitName.Length == 1)
            {
                if (splitName[0] == null || splitName[0] == " ") { Globals.WriteError("Cannot rename to nothing."); return; }
                if (splitName[0].Contains("/") || splitName[0].Contains(" ")) { Globals.WriteError("File name cannot include / or space."); return; }

                name = splitName[0];
                return;
            }
        }

        public void Delete()
        {
            allFiles.Remove(this);
        }
    }
}