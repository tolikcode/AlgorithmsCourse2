using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.DataStructures;

namespace AlgorithmsCourse2.TasksImplementations
{
    /// <summary>
    /// In this case there are no explicit distances provided between points.
    /// We take hamming distance as distance between two elements.
    /// </summary>
    class ImplicitDistancesClustering
    {
        public int ComputeNumberOfClustersForDistance(int[] inputElements, int numberOfBits)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>(); // key - element itself (bit array as integer)
                                                                          // value - element's number
            UnionFind<int> unionFind = new UnionFind<int>(); // UnionFind containing all elements' numbers
            for (int i = 0; i < inputElements.Length; i++)
            {
                int elementsNumber = i + 1;
                unionFind.Add(elementsNumber);
                if (!dictionary.ContainsKey(inputElements[i]))
                {
                    dictionary.Add(inputElements[i], elementsNumber);
                }
                else
                {
                    int duplicateElementNumber = dictionary[inputElements[i]];
                    unionFind.Union(elementsNumber, duplicateElementNumber);
                }
            }

            int[] masks = GetMasksDifferentBy2BitsMax(numberOfBits).ToArray();
            
            for (int i = 0; i < inputElements.Length; i++)
            {
                int elementsNumber = i + 1;

                IEnumerable<int> bitsSimularElements = masks.Select(mask => inputElements[i] ^ mask); // elements different no more but just by two bits

                foreach (int simularElement in bitsSimularElements)
                {
                    if (dictionary.ContainsKey(simularElement))
                    {
                        int simularElementNumber = dictionary[simularElement];
                        unionFind.Union(elementsNumber, simularElementNumber);
                    }
                }
            }

            return unionFind.ClustersCount;
        }

        // By XORing integers with these masks we'll get integers that are diffent no more than in two bits
        private IEnumerable<int> GetMasksDifferentBy2BitsMax(int numberOfBits)
        {
            for (int i = 0; i < numberOfBits; i++)
            {
                BitArray oneBitDifferent = new BitArray(numberOfBits, false);
                oneBitDifferent[i] = !oneBitDifferent[i];
                yield return oneBitDifferent.ToInt32();

                for (int j = i + 1; j < numberOfBits; j++)
                {
                    BitArray twoBitsDifferent = new BitArray(oneBitDifferent);
                    twoBitsDifferent[j] = !twoBitsDifferent[j];
                    yield return twoBitsDifferent.ToInt32();
                }
            }
        }

    }

    public static class BinaryConverter
    {
        public static BitArray ToBitArray(this int integer)
        {
            return new BitArray(new[] { integer });
        }

        public static int ToInt32(this BitArray bitArray)
        {
            if (bitArray == null)
                throw new ArgumentNullException("bitArray");
            if (bitArray.Length > 32)
                throw new ArgumentException("Must be at most 32 bits long");

            var result = new int[1];
            bitArray.CopyTo(result, 0);
            return result[0];
        }
    }
}
