using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.DataStructures;

namespace AlgorithmsCourse2.TasksImplementations
{
    /// <summary>
    /// Implementation of a Kruskal's algorithm for finding minimum spanning tree (MST).
    /// </summary>
    class KruskalsMst
    {
        public long CalculateMstCost(IEnumerable<Edge> edges)
        {
            List<Edge> minimumSpanningTree = new List<Edge>();
            UnionFind<int> unionFind = new UnionFind<int>(); 
            edges = edges.OrderBy(edge => edge.Cost);

            foreach (Edge edge in edges)
            {
                // if edge vertices are not yet connected by other edges,
                // then we can safely add this edge to MST
                if (!unionFind.CheckConnected(edge.Vertex1, edge.Vertex2))
                {
                    unionFind.Union(edge.Vertex1, edge.Vertex2);
                    minimumSpanningTree.Add(edge);
                }
            }

            return minimumSpanningTree.Aggregate<Edge, long>(0, (current, kruskalsEdge) => current + kruskalsEdge.Cost);
        }
    }
}
