using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmsCourse2.TasksImplementations
{
    /// <summary>
    /// Algorithm for solving Traveling Salesman Problem.
    /// <remarks>
    /// Performance results for the test task (25 cities)
    /// on Intel Core i5-2520M, 4Gb Ram, rather swamped Windows 8:
    /// Average running time (not debug mode): 52 sec
    /// Peak memory usage: ~ 1550 Mb
    /// 
    /// People tell it can work a lot faster (and using less memory).
    /// A way to significantly decrease memory usage and speed it up:
    /// Use arrays instead of hash tables to store subproblems' solutions.
    /// To find then a needed solution you possibly can use Combinatorial number system.
    /// </remarks>
    /// </summary>
    class TravelingSalesman
    {
        private double[,] distances; // distances between all the cities

        /// <summary>
        /// Computes the optimal path length for Traveling Salesman problem.
        /// </summary>
        /// <param name="coordinates">
        /// Cities coordinates: double[i, 2]
        /// Dimension 0: cities
        /// Dimension 1: coordinates (x, y) = 2
        /// </param>
        /// <returns></returns>
        public double ComputeOptimalPathLength(double[,] coordinates)
        {
            InitDistancesBetweenCities(coordinates);

            int numberOfCities = coordinates.GetLength(0);
            int usedNumberOfCities = numberOfCities - 1; // we don't explicitly include city 0 in our subprolems,
                                                         // as it must be in every subproblem anyway

            ConcurrentDictionary<int, double[]> previousSolutions = new ConcurrentDictionary<int, double[]>(); // subproblems of a smaller size (key)
                                                                                                               // and there solutions (value)

            int[] size1Subproblems = GetNumbersWithBitsSet(usedNumberOfCities, 1);
            for (int i = 0; i < size1Subproblems.Length; i++)
            {
                double[] solution = new double[usedNumberOfCities];
                for (int j = 0; j < solution.Length; j++)
                {
                    solution[j] = i == j ? distances[0, j + 1] : double.PositiveInfinity;
                }
                previousSolutions[size1Subproblems[i]] = solution;
            }

            ConcurrentDictionary<int, double[]> currentSolutions = new ConcurrentDictionary<int, double[]>(); // subproblems of current size (key)
                                                                                                              // and there solutions (value)

            for (int subproblemSize = 2; subproblemSize <= usedNumberOfCities; subproblemSize++)
            {
                currentSolutions = new ConcurrentDictionary<int, double[]>();

                Console.WriteLine("Current subproblem size: " + subproblemSize);

                int[] subproblems = GetNumbersWithBitsSet(usedNumberOfCities, subproblemSize);
                Parallel.ForEach(subproblems, subproblem =>
                    {
                        double[] currentSolution = new double[usedNumberOfCities];
                        for (int indexOfCurrent = 0; indexOfCurrent < currentSolution.Length; indexOfCurrent++)
                        {
                            if (CheckBitSet(subproblem, indexOfCurrent))
                            {
                                int previousSubproblem = SwitchBit(subproblem, indexOfCurrent); // problem of a smaller size that is different by one bit
                                double[] previousSolution = previousSolutions[previousSubproblem];

                                double smallestValue = double.PositiveInfinity;
                                for (int indexOfSmaller = 0; indexOfSmaller < previousSolution.Length; indexOfSmaller++)
                                {
                                    if (indexOfSmaller != indexOfCurrent)
                                    {
                                        // I lookup distances[indexOfSmaller + 1, indexOfCurrent + 1] instead of distances[indexOfSmaller, indexOfCurrent]
                                        // because in distances array City 0 is taken into account, while it's not in the current computations
                                        double possiblySmallerValue = previousSolution[indexOfSmaller] + distances[indexOfSmaller + 1, indexOfCurrent + 1];
                                        if (possiblySmallerValue < smallestValue)
                                            smallestValue = possiblySmallerValue;
                                    }
                                }

                                currentSolution[indexOfCurrent] = smallestValue;
                            }
                            else
                            {
                                currentSolution[indexOfCurrent] = double.PositiveInfinity;
                            }
                        }

                        currentSolutions[subproblem] = currentSolution;
                    });
                
                previousSolutions = currentSolutions;
            }

            int[] lastSubproblem = GetNumbersWithBitsSet(usedNumberOfCities, usedNumberOfCities);
            double[] lastSolution = currentSolutions[lastSubproblem[0]];

            double minValue = double.PositiveInfinity;
            for (int i = 0; i < lastSolution.Length; i++)
            {
                double possiblyNewMinValue = lastSolution[i] + distances[0, i + 1];
                if (possiblyNewMinValue < minValue)
                    minValue = possiblyNewMinValue;
            }

            return minValue;
        }

        private void InitDistancesBetweenCities(double[,] coordinates)
        {
            int numberOfCities = coordinates.GetLength(0);

            distances = new double[numberOfCities, numberOfCities];
            for (int i = 0; i < numberOfCities; i++)
            {
                for (int j = i + 1; j < numberOfCities; j++)
                {
                    double distance = Math.Sqrt(Math.Pow(coordinates[i, 0] - coordinates[j, 0], 2) +
                                                Math.Pow(coordinates[i, 1] - coordinates[j, 1], 2)); // Euclidean distance formula
                    distances[i, j] = distance;
                    distances[j, i] = distance;
                }
            }
        }

        #region Bit operations

        /// <summary>
        /// Returns all numbers with the same number of bits set
        /// </summary>
        /// <param name="totalNumberOfBits">Usable number of bits (that can possibly be set)</param>
        /// <param name="numberOfSetBits">Number of bits that are allowed to be simultaneously set</param>
        /// <returns></returns>
        private int[] GetNumbersWithBitsSet(int totalNumberOfBits, int numberOfSetBits)
        {
            if(totalNumberOfBits > 32)
                throw new ArgumentOutOfRangeException("totalNumberOfBits", "Cannot represent bit array of size more than 32 bits with Int32 data structure.");

            if(numberOfSetBits > totalNumberOfBits)
                throw new ArgumentException("Number of bits to be set is bigger than number of bits allowed to use.");

            int numberOfSuchNumbers = GetNumberOfCombinations(totalNumberOfBits, numberOfSetBits);
            int[] numbers = new int[numberOfSuchNumbers];

            int seedNumber = 0;
            for (int i = 0; i < numberOfSetBits; i++)
            {
                seedNumber = seedNumber | (int)Math.Pow(2, i);
            }
            numbers[0] = seedNumber;

            for (int i = 1; i < numberOfSuchNumbers; i++)
            {
                seedNumber = GetNextSimularNumber(seedNumber);
                numbers[i] = seedNumber;
            }

            return numbers;
        }

        /// <summary>
        /// Returns another integer with the same number of bits set
        /// </summary>
        private int GetNextSimularNumber(int number)
        {
            // Gosper's Hack
            int a = number & -number;
            int b = number + a;
            return (((b ^ number) >> 2) / a) | b;
        }

        /// <summary>
        /// Check whether a certain bit is set
        /// </summary>
        private bool CheckBitSet(int bitArray, int bitNumber)
        {
            if (bitNumber > 32)
                throw new ArgumentOutOfRangeException("bitNumber", "Number of bit to check is bigger than number of bits in Int32.");

            int checkNumber = bitArray | (int)Math.Pow(2, bitNumber);
            return bitArray == checkNumber;
        }

        /// <summary>
        /// Changes the value of a certain bit to an opposite value
        /// </summary>
        private int SwitchBit(int bitArray, int bitNumber)
        {
            if (bitNumber > 32)
                throw new ArgumentOutOfRangeException("bitNumber", "Number of bit to switch is bigger than number of bits in Int32.");

            return bitArray ^ (int)Math.Pow(2, bitNumber);
        }

        #endregion Bit operations

        /// <summary>
        /// Recursive computation of the number of computation.
        /// <remarks>
        /// A formula from school can be used to compute number of combinations for a small numbers only.
        /// </remarks>
        /// </summary>
        private int GetNumberOfCombinations(int totalNumber, int numberToSelect)
        {
            if (numberToSelect == 0 || totalNumber == numberToSelect)
                return 1;

            return GetNumberOfCombinations(totalNumber - 1, numberToSelect - 1) +
                   GetNumberOfCombinations(totalNumber - 1, numberToSelect);
        }
    }
}
