using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse2.TasksImplementations
{
    internal struct Edge
    {
        private readonly int vertex1;
        private readonly int vertex2;
        private readonly int cost;

        public Edge(int vertex1, int vertex2, int cost)
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
}
