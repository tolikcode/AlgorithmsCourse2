using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.TasksImplementations;

namespace AlgorithmsCourse2
{
    class Program
    {
        static void Main(string[] args)
        {
            ScheduleJobs();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        /// <summary>
        /// Greedy algorithms from lecture for minimizing the weighted sum of completion times of a set of jobs.
        /// 
        /// File format:
        /// [number_of_jobs]
        /// [job_1_weight] [job_1_length]
        /// [job_2_weight] [job_2_length]
        /// </summary>
        public static void ScheduleJobs()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\jobs.txt");
            List<Job> jobs = new List<Job>();

            for (int i = 1; i < taskLines.Length; i++)
            {
                string[] lineValues = taskLines[i].Split(' ');
                jobs.Add(new Job(int.Parse(lineValues[0]), int.Parse(lineValues[1])));
            }
            
            Console.WriteLine("Initial ordering completion times sum: " + CalcWeightedCompletionTimesSum(jobs));

            List<Job> differeceSortedJobs = new List<Job>(jobs);
            differeceSortedJobs.Sort(new DifferenceWeightJobComparer());
            differeceSortedJobs.Reverse();
            Console.WriteLine("Completion times sum of jobs ordered by difference W(i) - L(i): " + CalcWeightedCompletionTimesSum(differeceSortedJobs));

            List<Job> ratioSortedJobs = new List<Job>(jobs);
            ratioSortedJobs.Sort((x, y) => x.RatioCoefficient.CompareTo(y.RatioCoefficient));
            ratioSortedJobs.Reverse();
            Console.WriteLine("Completion times sum of jobs ordered by ratio W(i)/L(i): " + CalcWeightedCompletionTimesSum(ratioSortedJobs));
        }

        private static long CalcWeightedCompletionTimesSum(IEnumerable<Job> jobs)
        {
            long weightedCompletionTimesSum = 0, 
                currentCompletionTime = 0;
            foreach (Job job in jobs)
            {
                currentCompletionTime += job.Length;
                weightedCompletionTimesSum += currentCompletionTime*job.Weight;
            }

            return weightedCompletionTimesSum;
        }

    }
}
