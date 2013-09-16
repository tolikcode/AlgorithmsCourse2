using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.DataStructures;

namespace AlgorithmsCourse2.TasksImplementations
{
    internal class PrimsVertex : IComparable<PrimsVertex>
    {
        public PrimsVertex(int vertexNumber)
        {
            this.VertexNumber = vertexNumber;
            Edges = new List<PrimsEdge>();
        }

        public List<PrimsEdge> Edges { get; private set; }

        public int VertexNumber { get; private set; }
        public PrimsEdge CheapestInEdge { get; set; }    // The cheapest edge that connects this vertex with set of vertices
                                                         // that were already explored by the algorithm
        public int CompareTo(PrimsVertex other)
        {
            // if CheapestInEdge == null, then there is no in edge from explored vertices and the cost is infinite

            if (this.CheapestInEdge == null && other.CheapestInEdge == null)
                return 0;
            if (this.CheapestInEdge == null && other.CheapestInEdge != null)
                return 1;
            if (this.CheapestInEdge != null && other.CheapestInEdge == null)
                return -1;

            return this.CheapestInEdge.Cost.CompareTo(other.CheapestInEdge.Cost);
        }
    }

    internal class PrimsEdge
    {
        public PrimsEdge(PrimsVertex vertex1, PrimsVertex vertex2, int cost)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Cost = cost;
        }

        public PrimsVertex Vertex1 { get; private set; }
        public PrimsVertex Vertex2 { get; private set; }
        public int Cost { get; private set; }

        public PrimsVertex GetAnotherVertex(PrimsVertex vertex)
        {
            if (vertex == Vertex1)
                return Vertex2;
            if (vertex == Vertex2)
                return Vertex1;

            throw new ArgumentException("The edge is not connected with this vertex.");
        }
    }

    class PrimsMst
    {
        /// <summary>
        /// Prim's algorithm for finding minimum spanning tree (MST).
        /// </summary>
        /// <param name="primsVertices">Array of vertices of the graph</param>
        /// <returns>MST cost (sum of all edges costs in MST)</returns>
        public long CalculateMstCost(PrimsVertex[] primsVertices)
        {
            List<PrimsEdge> minimumSpanningTree = new List<PrimsEdge>();

            int startVertexIndex = 1; // this can be any vertex in primsVertices array

            foreach (PrimsEdge edge in primsVertices[startVertexIndex].Edges)
            {
                PrimsVertex otherVertex = edge.GetAnotherVertex(primsVertices[startVertexIndex]);
                if (otherVertex.CheapestInEdge == null || otherVertex.CheapestInEdge.Cost > edge.Cost)
                    otherVertex.CheapestInEdge = edge;
            }

            Heap<PrimsVertex> minHeap = new Heap<PrimsVertex>(false);

            for (int i = 2; i < primsVertices.Length; i++) // skipping empty 0 position and a startVertex
            {
                minHeap.Insert(primsVertices[i]);
            }

            while (minHeap.Count != 0)
            {
                PrimsVertex currentMinVertex = minHeap.ExtractRoot();
                minimumSpanningTree.Add(currentMinVertex.CheapestInEdge);

                foreach (PrimsEdge edge in currentMinVertex.Edges)
                {
                    PrimsVertex otherVertex = edge.GetAnotherVertex(currentMinVertex);
                    if (minHeap.Contains(otherVertex) 
                        && (otherVertex.CheapestInEdge == null || otherVertex.CheapestInEdge.Cost > edge.Cost))
                    {
                        minHeap.Delete(otherVertex);
                        otherVertex.CheapestInEdge = edge;
                        minHeap.Insert(otherVertex);
                    }
                }

            }

            long mstCost = 0;
            foreach (PrimsEdge primsEdge in minimumSpanningTree)
            {
                mstCost += primsEdge.Cost;
            }
            return mstCost;
        }

    }
}
