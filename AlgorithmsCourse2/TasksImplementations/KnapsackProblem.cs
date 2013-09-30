﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse2.TasksImplementations
{
    struct  KnapsackItem
    {
        public readonly int Value;
        public readonly int Size;

        public KnapsackItem(int value, int size)
        {
            this.Value = value;
            this.Size = size;
        }
    }

    /// <summary>
    /// Characteristics of knapsack subproblem - problem with a smaller number of items and/or smaller knapsack size.
    /// </summary>
    struct SubproblemCharacteristics
    {
        public readonly int NumberOfItems;
        public readonly int KnapsackSize;

        public SubproblemCharacteristics(int numberOfItems, int knapsackSize)
        {
            this.NumberOfItems = numberOfItems;
            this.KnapsackSize = knapsackSize;
        }
    }

    class KnapsackProblem
    {
        #region Naive implementation of a Knapsack algorithm (Bottom-up approach)

        public int CalcOptimalSolutionValue(IList<KnapsackItem> items, int knapsackSize)
        {
            int[,] solutionArray = new int[items.Count + 1, knapsackSize + 1]; // +1 for cases itemsCount = 0 and knapsackSize = 0
            
            for (int sizeIndex = 0; sizeIndex <= knapsackSize; sizeIndex++) // initialize with zeros for items count = 0
                solutionArray[0, sizeIndex] = 0;

            for (int itemIndex = 1; itemIndex <= items.Count; itemIndex++)
            {
                for (int sizeIndex = 0; sizeIndex <= knapsackSize; sizeIndex++)
                {
                    KnapsackItem currentItem = items[itemIndex - 1];

                    if (sizeIndex >= currentItem.Size)
                    {
                        solutionArray[itemIndex, sizeIndex] = Math.Max(solutionArray[itemIndex - 1, sizeIndex],
                                                                           solutionArray[itemIndex - 1, sizeIndex - currentItem.Size] + currentItem.Value);
                    }
                    else
                    {
                        solutionArray[itemIndex, sizeIndex] = solutionArray[itemIndex - 1, sizeIndex];
                    }
                }
            }

            // Going backwards to find out what items are in the found solution
            int currentSizeIndex = knapsackSize;
            int knapsackUsedSize = 0;
            for (int itemIndex = items.Count; itemIndex > 0; itemIndex--)
            {
                KnapsackItem currentItem = items[itemIndex - 1];

                if (solutionArray[itemIndex, currentSizeIndex] == solutionArray[itemIndex - 1, currentSizeIndex])
                    continue;

                if (solutionArray[itemIndex, currentSizeIndex] == solutionArray[itemIndex - 1, currentSizeIndex - currentItem.Size] + currentItem.Value)
                {
                    currentSizeIndex -= currentItem.Size;
                    knapsackUsedSize += currentItem.Size;
                    Console.WriteLine("Solution includes item {0}. Value: {1}. Size: {2}.", itemIndex, currentItem.Value, currentItem.Size);
                    continue;
                }

                throw new Exception("The solution array is incorrect.");
            }
            Console.WriteLine("Knapsack size used: " + knapsackUsedSize);
            return solutionArray[items.Count, knapsackSize];
        }

        #endregion Naive implementation of a Knapsack algorithm (Bottom-up approach)

        #region Recursive implementation (Top-down approach)

        private IList<KnapsackItem> _items;
        private Dictionary<SubproblemCharacteristics, int> _subproblemsOptimalValues; 

        public int CalcRecurciveOptimalSolutionValue(IList<KnapsackItem> items, int knapsackSize)
        {
            _items = items;
            _subproblemsOptimalValues = new Dictionary<SubproblemCharacteristics, int>();
            
            int optimalSolutionValue = GetSubproblemSolutionValue(items.Count, knapsackSize);
            Console.WriteLine("Total number of subproblems: " + (long)_items.Count*knapsackSize);
            Console.WriteLine("Number of subproblems were solved: " + _subproblemsOptimalValues.Count);
            return optimalSolutionValue;
        }

        private int GetSubproblemSolutionValue(int numberOfItems, int knapsackSize)
        {
            if (numberOfItems == 0)
                return 0;

            SubproblemCharacteristics currentSubproblemCharacteristics = new SubproblemCharacteristics(numberOfItems, knapsackSize);

            if (_subproblemsOptimalValues.ContainsKey(currentSubproblemCharacteristics))
                return _subproblemsOptimalValues[currentSubproblemCharacteristics];

            KnapsackItem currentItem = _items[numberOfItems - 1];

            int optimalValue;
            if (knapsackSize > currentItem.Size)
            {
                optimalValue = Math.Max(GetSubproblemSolutionValue(numberOfItems - 1, knapsackSize),
                                                                           GetSubproblemSolutionValue(numberOfItems - 1, knapsackSize - currentItem.Size) + currentItem.Value);
            }
            else
            {
                optimalValue = GetSubproblemSolutionValue(numberOfItems - 1, knapsackSize);
            }

            _subproblemsOptimalValues.Add(currentSubproblemCharacteristics, optimalValue);
            return optimalValue;
        }

        #endregion Recursive implementation (top-down approach)
    }
}
