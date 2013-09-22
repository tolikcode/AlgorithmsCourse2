﻿using System;
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
            ComputeMaxSpacingKClustering();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        /// <summary>
        /// Greedy algorithms from lecture for minimizing the weighted sum of completion times of a set of jobs.
        /// 
        /// Input file format:
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

        /// <summary>
        /// Prim's algorithm for finding minimum spanning tree (MST).
        /// 
        /// Input file format:
        /// [number_of_nodes] [number_of_edges]
        /// [one_node_of_edge_1] [other_node_of_edge_1] [edge_1_cost]
        /// [one_node_of_edge_2] [other_node_of_edge_2] [edge_2_cost]
        /// </summary>
        public static void CalculatePrimsMstCost()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\edges.txt");

            int numberOfVertices = int.Parse(taskLines[0].Split(' ')[0]);
            PrimsVertex[] primsVertices = new PrimsVertex[numberOfVertices + 1]; // position 0 in the array will always be empty
            for (int i = 1; i < primsVertices.Length; i++)
            {
                primsVertices[i] = new PrimsVertex(i);
            }

            for (int i = 1; i < taskLines.Length; i++) // the first line contains number of vertices and number of edges
            {
                string[] lineValues = taskLines[i].Split(' ');

                int firstVertexNumber = int.Parse(lineValues[0]);
                int secondVertexNumber = int.Parse(lineValues[1]);
                int cost = int.Parse(lineValues[2]);

                PrimsEdge edge = new PrimsEdge(primsVertices[firstVertexNumber], primsVertices[secondVertexNumber], cost);

                primsVertices[firstVertexNumber].Edges.Add(edge);
                primsVertices[secondVertexNumber].Edges.Add(edge);
            }

            PrimsMst primsMst = new PrimsMst();
            long mstCost = primsMst.CalculateMstCost(primsVertices);

            Console.WriteLine("Minimum spanning tree cost by Prism: " + mstCost);
        }

        /// <summary>
        /// Kruskal's algorithm for finding minimum spanning tree (MST).
        /// 
        /// Input file format:
        /// [number_of_nodes] [number_of_edges]
        /// [one_node_of_edge_1] [other_node_of_edge_1] [edge_1_cost]
        /// [one_node_of_edge_2] [other_node_of_edge_2] [edge_2_cost]
        /// </summary>
        public static void CalculateKruskalsMstCost()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\edges.txt");
            List<Edge> edges = new List<Edge>();

            for (int i = 1; i < taskLines.Length; i++) // the first line contains number of vertices and number of edges
            {
                string[] lineValues = taskLines[i].Split(' ');
                int firstVertexNumber = int.Parse(lineValues[0]);
                int secondVertexNumber = int.Parse(lineValues[1]);
                int cost = int.Parse(lineValues[2]);
                edges.Add(new Edge(firstVertexNumber, secondVertexNumber, cost));
            }

            KruskalsMst kruskalsMst = new KruskalsMst();
            long mstCost = kruskalsMst.CalculateMstCost(edges);

            Console.WriteLine("Minimum spanning tree cost by Kruskal: " + mstCost);
        }

        /// <summary>
        /// Clustering algorithm for computing a max-spacing k-clustering
        /// (maximal (over all possible k clusters) minimal space between k clusters).
        /// Single-link clustering is used.
        /// 
        /// Input must be a complete graph, because we're supposed to know the distance between every node.
        /// Input file format:
        /// [number_of_nodes]
        /// [edge 1 node 1] [edge 1 node 2] [edge 1 cost]
        /// [edge 2 node 1] [edge 2 node 2] [edge 2 cost]
        /// </summary>
        public static void ComputeMaxSpacingKClustering()
        {
            int targetNumberOfClusters = 4; // from the task

            string[] taskLines = File.ReadAllLines(@"TasksData\clustering1.txt");

            int numberOfNodes = int.Parse(taskLines[0]);

            List<Edge> distancesBetweenNodes = new List<Edge>();
            for (int i = 1; i < taskLines.Length; i++) // the first line contains number of nodes
            {
                string[] lineValues = taskLines[i].Split(' ');
                int firstVertexNumber = int.Parse(lineValues[0]);
                int secondVertexNumber = int.Parse(lineValues[1]);
                int cost = int.Parse(lineValues[2]);
                distancesBetweenNodes.Add(new Edge(firstVertexNumber, secondVertexNumber, cost));
            }

            SingleLinkClustering singleLinkClustering = new SingleLinkClustering();
            int maxMinSpacing = singleLinkClustering.ComputeMaxSpacingKClustering(distancesBetweenNodes, numberOfNodes,
                                                              targetNumberOfClusters);

            Console.WriteLine("Max spacing for {0} clusters: {1}", targetNumberOfClusters, maxMinSpacing);
        }
    }
}
