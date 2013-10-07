using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.DataStructures;
using AlgorithmsCourse2.TasksImplementations;

namespace AlgorithmsCourse2
{
    class Program
    {
        static void Main(string[] args)
        {
            FindShortestShortestPath();
            
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

        /// <summary>
        /// Solving clustering problem for a large input.
        /// All distances are provides implicitly. The distance between two nodes u and v in this problem is defined as
        /// the Hamming distance--- the number of differing bits --- between the two nodes' labels.
        /// The question is: what is the largest value of k such that there is a k-clustering with spacing at least 3?
        /// That is, how many clusters are needed to ensure that no pair of nodes with all but 2 bits in common get split into different clusters?
        /// 
        /// Input file format:
        /// [# of nodes] [# of bits for each node's label]
        /// [first bit of node 1] ... [last bit of node 1]
        /// [first bit of node 2] ... [last bit of node 2]
        /// </summary>
        public static void ComputeMaxSpacingKClusteringBig()
        {
            StreamReader streamReader = new StreamReader(@"TasksData\clustering_big.txt");

            string firstLine = streamReader.ReadLine();
            int numberOfNodes = int.Parse(firstLine.Split(' ')[0]);
            int numberOfBits = int.Parse(firstLine.Split(' ')[1]);

            // elements in the input file are represented as bit arrays,
            // we're going work with them as with intergers
            int[] elements = new int[numberOfNodes];
            for (int i = 0; i < numberOfNodes; i++)
            {
                string line = streamReader.ReadLine();
                string[] lineValues = line.Split(' ');
                bool[] boolArray = new bool[numberOfBits];
                for (int j = 0; j < numberOfBits; j++)
                    boolArray[j] = lineValues[j] == "1";
                elements[i] = new BitArray(boolArray).ToInt32();
            }

            ImplicitDistancesClustering implicitDistancesClustering = new ImplicitDistancesClustering();
            int numberOfClusters = implicitDistancesClustering.ComputeNumberOfClustersForDistance(elements, numberOfBits);

            Console.WriteLine("Number of clusters with spacing at least 3: {0}", numberOfClusters);
        }

        /// <summary>
        /// Finds the solution for Knapsack problem.
        /// 
        /// Input file format:
        /// [knapsack_size][number_of_items]
        /// [value_1] [weight_1]
        /// [value_2] [weight_2]
        /// </summary>
        public static void SolveKnapsackProblem()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\knapsack1.txt");

            int knapsakSize = int.Parse(taskLines[0].Split(' ')[0]);

            List<KnapsackItem> items = new List<KnapsackItem>();
            for (int i = 1; i < taskLines.Length; i++)
            {
                string[] lineValues = taskLines[i].Split(' ');
                items.Add(new KnapsackItem(int.Parse(lineValues[0]), int.Parse(lineValues[1])));
            }

            KnapsackProblem knapsackProblem = new KnapsackProblem();
            int optimalSolutionValue = knapsackProblem.CalcOptimalSolutionValue(items, knapsakSize);

            Console.WriteLine("The optimal solution value: {0}", optimalSolutionValue);
        }

        /// <summary>
        /// Finds the solution for Knapsack problem (works with a large input data size).
        /// 
        /// Input file format:
        /// [knapsack_size][number_of_items]
        /// [value_1] [weight_1]
        /// [value_2] [weight_2]
        /// </summary>
        public static void SolveKnapsackProblemBig()
        {
            string[] taskLines = File.ReadAllLines(@"TasksData\knapsack_big.txt");

            int knapsakSize = int.Parse(taskLines[0].Split(' ')[0]);

            List<KnapsackItem> items = new List<KnapsackItem>();
            for (int i = 1; i < taskLines.Length; i++)
            {
                string[] lineValues = taskLines[i].Split(' ');
                items.Add(new KnapsackItem(int.Parse(lineValues[0]), int.Parse(lineValues[1])));
            }

            KnapsackProblem knapsackProblem = new KnapsackProblem();
            
            Console.WriteLine("1. Recursive implementation (top-down approach)");
            int recursiveSolutionValue = knapsackProblem.CalcRecursiveOptimalSolutionValue(items, knapsakSize);
            Console.WriteLine("The optimal solution value with recursive top-down approach: {0}", recursiveSolutionValue);

            Console.WriteLine("1. Naive implementation with 2 arrays (bottom-up aproach)");
            int twoArraySolutionsValue = knapsackProblem.CaclOptimalSolutionValueWith2Arrays(items, knapsakSize);
            Console.WriteLine("The optimal solution value with naive implementation: {0}", twoArraySolutionsValue);
        }

        public static void FindShortestShortestPath()
        {
            ComputeShortestPath(@"TasksData\g1.txt");
            ComputeShortestPath(@"TasksData\g2.txt");
            ComputeShortestPath(@"TasksData\g3.txt");
        }

        private static void ComputeShortestPath(string inputFilePath)
        {
            FloydWarshall floydWarshall = new FloydWarshall();

            string[] taskLines = File.ReadAllLines(inputFilePath);
            int numberOfVertices = int.Parse(taskLines[0].Split(' ')[0]);
            List<Edge> edges = new List<Edge>();
            for (int i = 1; i < taskLines.Length; i++) // the first line contains number of vertices and number of edges
            {
                string[] lineValues = taskLines[i].Split(' ');
                edges.Add(new Edge(int.Parse(lineValues[0]), int.Parse(lineValues[1]), int.Parse(lineValues[2])));
            }

            try
            {
                Console.WriteLine("Computing shortest path for " + inputFilePath);

                int[,] asps1 = floydWarshall.FindApsp(numberOfVertices, edges.ToArray());

                int shortestPath = asps1[1, 1];
                for (int i = 1; i <= numberOfVertices; i++)
                {
                    for (int j = 1; j <= numberOfVertices; j++)
                    {
                        if (asps1[i, j] < shortestPath)
                            shortestPath = asps1[i, j];
                    }
                }

                Console.WriteLine("The shortest path in {0}: {1}", inputFilePath, shortestPath);
            }
            catch (NegativeCycleException)
            {
                Console.WriteLine(inputFilePath + " has a negative cost cycle.");
            }
        }
    }

}
