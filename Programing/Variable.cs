
namespace MiniComputer2
{
    class Variable { }

    class Variable<T> : Variable
    {
        public string name;
        public T value;

        public Variable(string newName, T newValue)
        {
            name = newName;
            value = newValue;
        }
    }
}