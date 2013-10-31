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
        private int clustersCount;
        private Dictionary<T, T> parentsDictionary = new Dictionary<T, T>();
        private Dictionary<T, int> depthDictionary = new Dictionary<T, int>();

        public int ElementsCount
        {
            get { return parentsDictionary.Count; }
        }

        /// <summary>
        /// Number of clusters (disjoint sets of elements) that are not interconnected.
        /// </summary>
        public int ClustersCount
        {
            get { return clustersCount; }
        }

        public void Add(T element)
        {
            parentsDictionary.Add(element, element);
            depthDictionary.Add(element, 0);
            clustersCount++;
        }

        public void Union(T element1, T element2)
        {
            if (!parentsDictionary.ContainsKey(element1))
                Add(element1);

            if (!parentsDictionary.ContainsKey(element2))
                Add(element2);

            T parent1 = FindRecursive(element1);
            T parent2 = FindRecursive(element2);

            if(!parent1.Equals(parent2))
                clustersCount--;

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
