using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmsCourse2.DataStructures;

namespace AlgorithmsCourse2.TasksImplementations
{
    class SingleLinkClustering
    {
        /// <summary>
        /// Computes a max-spacing k-clustering.
        /// </summary>
        /// <param name="distancesBetweenNodes"></param>
        /// <param name="numberOfNodes"></param>
        /// <param name="kNumberOfClusters">k - target number of clusters</param>
        /// <returns>Maximal (over all possible k clusters) minimal space between k clusters</returns>
        public int ComputeMaxSpacingKClustering(IEnumerable<Edge> distancesBetweenNodes, int numberOfNodes, int kNumberOfClusters)
        {
            UnionFind<int> unionFind = new UnionFind<int>();

            // Each node is represented just by integer (node number). We add all nodes to UnionFind datastructure
            for (int i = 1; i <= numberOfNodes; i++)
            {
                unionFind.Add(i);
            }

            distancesBetweenNodes = distancesBetweenNodes.OrderBy(edge => edge.Cost);

            foreach (Edge edge in distancesBetweenNodes)
            {
                if (unionFind.ClustersCount == kNumberOfClusters)
                    break;

                unionFind.Union(edge.Vertex1, edge.Vertex2);
            }

            if(unionFind.ClustersCount != kNumberOfClusters)
                throw new Exception(string.Format("Failed to split into {0} clusters.", kNumberOfClusters));

            foreach (Edge edge in distancesBetweenNodes)
            {
                if (!unionFind.CheckConnected(edge.Vertex1, edge.Vertex2))
                {
                    return edge.Cost;
                }
            }

            return 0; // if you get here, you have one cluster only
        }

    }
}
