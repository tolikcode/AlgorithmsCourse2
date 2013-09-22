using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.DataStructures;

namespace AlgorithmsCourse2.TasksImplementations
{
    internal struct KruskalsEdge
    {
        private readonly int vertex1;
        private readonly int vertex2;
        private readonly int cost;

        public KruskalsEdge(int vertex1, int vertex2, int cost)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.cost = cost;
        }

        public int Vertex1
        {
            get { return vertex1; }
        }

        public int Vertex2
        {
            get { return vertex2; }
        }

        public int Cost
        {
            get { return cost; }
        }
    }

    class KruskalsMst
    {
        public long CalculateMstCost(IEnumerable<KruskalsEdge> edges)
        {
            List<KruskalsEdge> minimumSpanningTree = new List<KruskalsEdge>();
            UnionFind<int> unionFind = new UnionFind<int>(); 
            edges = edges.OrderBy(edge => edge.Cost);

            foreach (KruskalsEdge edge in edges)
            {
                // if edge vertices are not yet connected by other edges,
                // then we can safely add this edge to MST
                if (!unionFind.CheckConnected(edge.Vertex1, edge.Vertex2))
                {
                    unionFind.Union(edge.Vertex1, edge.Vertex2);
                    minimumSpanningTree.Add(edge);
                }
            }

            return minimumSpanningTree.Aggregate<KruskalsEdge, long>(0, (current, kruskalsEdge) => current + kruskalsEdge.Cost);
        }
    }
}
