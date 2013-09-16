using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse2.TasksImplementations
{
    internal struct Job
    {
        private readonly int weight;
        private readonly int length;

        public Job(int weight, int length)
        {
            this.weight = weight;
            this.length = length;
        }

        public int Weight                        // Importance(priority) of a job
        {
            get { return weight; }
        }

        public int Length                        // Time required to do a job
        {
            get { return length; }
        }  
        
        public int DifferenceCoefficient         // Jobs with bigger coefficient must be done earilier
        {
            get { return weight - length; }
        }

        public double RatioCoefficient              // Jobs with bigger coefficient must be done earilier
        {
            get { return (double)weight/(double)length; }
        }
    }

    /// <summary>
    /// Compares by DifferenceCoefficient and then by Weight for equal DifferenceCoefficients
    /// </summary> 
    internal class DifferenceWeightJobComparer : IComparer<Job>
    {
        public int Compare(Job x, Job y)
        {
            return x.DifferenceCoefficient == y.DifferenceCoefficient 
                ? x.Weight.CompareTo(y.Weight) 
                : x.DifferenceCoefficient.CompareTo(y.DifferenceCoefficient);
        }
    }
}
