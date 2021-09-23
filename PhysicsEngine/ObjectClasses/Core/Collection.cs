using System.Collections.Generic;

namespace PhysicsEngine.ObjectClasses.Core
{
    public class Collection<T>
    {
        public Collection()
        {

        }

        private readonly List<T> CollectionT = new List<T>();

        private readonly List<string> Names = new List<string>();

        public void Add(T item, string name)
        {
            CollectionT.Add(item);
            Names.Add(name);
        }

        public T Get(string name)
        {
            int index = Names.FindIndex(0, n => n == name);

            return CollectionT[index];
        }

        public void Remove(string name)
        {
            int index = Names.FindIndex(0, n => n == name);

            Names.RemoveAt(index);
            CollectionT.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            CollectionT.RemoveAt(index);
            Names.RemoveAt(index);
        }
    }
}
