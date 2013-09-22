using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse2.DataStructures
{
    /// <summary>
    /// Implementation of a UnionFind data structure
    /// <remarks>
    /// Another name: Disjoint set
    /// </remarks>
    /// </summary>
    class UnionFind<T>
    {
        private Dictionary<T, T> parentsDictionary = new Dictionary<T, T>();
        private Dictionary<T, int> depthDictionary = new Dictionary<T, int>();

        public void Union(T element1, T element2)
        {
            if (!parentsDictionary.ContainsKey(element1))
            {
                parentsDictionary.Add(element1, element1);
                depthDictionary.Add(element1, 0);
            }

            if (!parentsDictionary.ContainsKey(element2))
            {
                parentsDictionary.Add(element2, element2);
                depthDictionary.Add(element2, 0);
            }

            T parent1 = FindRecursive(element1);
            T parent2 = FindRecursive(element2);

            int parent1Depth = depthDictionary[parent1];
            int parent2Depth = depthDictionary[parent2];

            if (parent1Depth >= parent2Depth)
            {
                parentsDictionary[parent2] = parent1;

                if (parent1Depth == parent2Depth)
                    depthDictionary[parent1]++;
            }
            else
            {
                parentsDictionary[parent1] = parent2;
            }
        }

        public T Find(T element)
        {
            if (!parentsDictionary.ContainsKey(element))
                return element;

            return FindRecursive(element);
        }

        private T FindRecursive(T element)
        {
            T parent = parentsDictionary[element];

            if (parent.Equals(element))
                return parent;

            return FindRecursive(parent);
        }

        public bool CheckConnected(T element1, T element2)
        {
            return Find(element1).Equals(Find(element2));
        }
    }
}
