using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse2.TasksImplementations
{
    /// <summary>
    /// Exception is thrown when a graph has a negative cost cycle.
    /// </summary>
    public class NegativeCycleException : Exception
    {
    }

    /// <summary>
    /// Implementation of a Floyd-Warshall algorithm (with two 2-dimensional arrays instead of one 3-dimensional).
    /// </summary>
    class FloydWarshall
    {
        // values considered large/small enough for my input data
        private const int PositiveInfinity = int.MaxValue/2 -1; 
        private const int NegativeInfinity = int.MinValue/2 + 1;

        /// <summary>
        /// Finds ASPS (All Pairs Shortest Path) of a graph.
        /// <remarks>
        /// Returned values in [i, 0] and [0, i] are always 0, because we start numbering of vertices from 1.
        /// </remarks>
        /// </summary>
        public int[,] FindApsp(int numberOfVertices, Edge[] edges)
        {
            int[,] previousSolutions = new int[numberOfVertices + 1, numberOfVertices + 1];
            int[,] currentSolutions = new int[numberOfVertices + 1, numberOfVertices + 1];

            // Distance from vertex to itself = 0. Distance from vertex to other vertices = infinity if we have no intermediate vertices/edges.
            for (int i = 0; i <= numberOfVertices; i++)
            {
                for (int j = 0; j <= numberOfVertices; j++)
                {
                    
                    previousSolutions[i, j] = i == j ? 0 : PositiveInfinity;
                }
            }
            // Distance (without intermediate vertices) between two vertices = edge length, if there is one.
            foreach (Edge edge in edges)
            {
                previousSolutions[edge.Vertex1, edge.Vertex2] = edge.Cost;
            }


            for (int maxAllowedV = 1; maxAllowedV < numberOfVertices; maxAllowedV++)
            {
                currentSolutions = new int[numberOfVertices + 1, numberOfVertices + 1];
                for (int sourceV = 1; sourceV <= numberOfVertices; sourceV++)
                {
                    for (int targetV = 1; targetV <= numberOfVertices; targetV++)
                    {
                        int a = previousSolutions[sourceV, targetV];
                        int b = previousSolutions[sourceV, maxAllowedV] + previousSolutions[maxAllowedV, targetV];

                        if(b > PositiveInfinity)
                            b = PositiveInfinity;

                        if (b < NegativeInfinity)
                            b = NegativeInfinity;

                        currentSolutions[sourceV, targetV] = Math.Min(a, b);
                    }
                }
                previousSolutions = currentSolutions;
            }   

            // A negative value in a diagonal of a result array sygnals that input graph has a negative cost cycle
            for (int i = 1; i <= numberOfVertices; i++)
            {
                if (currentSolutions[i, i] < 0)
                    throw new NegativeCycleException();
            }

            return currentSolutions;
        }

    }
}
