namespace MiniComputer2
{
    enum varType
    {
        stri,
        inte,
        floa,
        none
    }

    class Variable
    {
        public string name;
        public varType type;
        public string value;

        public Variable(string newName, varType newType, string newValue)
        {
            name = newName;
            type = newType;
            value = newValue;
        }
    }

    class Function
    {
        public string name;
        public string returnType;
        public int startLine;
        public int lenght;

        public Function(string newName, string newReturnType, int newLine, int newLenght)
        {
            name = newName;
            returnType = newReturnType;
            startLine = newLine;
            lenght = newLenght;
        }
    }
}