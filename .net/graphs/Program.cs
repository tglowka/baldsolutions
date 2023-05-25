// See https://aka.ms/new-console-template for more information
using System.Linq;

Console.WriteLine("Hello, World!");

int[,] graph = new int[,] {           { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
                                      { 4, 0, 8, 0, 0, 0, 0, 11, 0 },
                                      { 0, 8, 0, 7, 0, 4, 0, 0, 2 },
                                      { 0, 0, 7, 0, 9, 14, 0, 0, 0 },
                                      { 0, 0, 0, 9, 0, 10, 0, 0, 0 },
                                      { 0, 0, 4, 14, 10, 0, 2, 0, 0 },
                                      { 0, 0, 0, 0, 0, 2, 0, 1, 6 },
                                      { 8, 11, 0, 0, 0, 0, 1, 0, 7 },
                                      { 0, 0, 2, 0, 0, 0, 6, 7, 0 } };

//int[,] graph = new int[,] {
//{ 0, 4, 4, 0, 0},
//{4, 0, 8, 7, 0},
//{4, 8, 0, 7, 4},
//{0, 7, 7, 0, 9},
//{ 0, 0, 4, 9, 0}};

GFG t = new GFG();
t.dijkstra(graph, 0);

public class GFG
{
    static int V = 9;

    public void printSolution(int[] dist, int n, List<List<int>> paths)
    {
        var paths2 = paths.Select(x => x.Select(y => (y+1).ToString())).ToArray();

        Console.WriteLine("Vertex Distance from Source");
        for (int i = 0; i < V; i++)
        {
            Console.Write((i+1) + "\t" + dist[i] + "\t, path:");
            Console.WriteLine(string.Join(", ", paths2[i]));

        }
    }


    public void dijkstra(int[,] graph, int src)
    {
        var distance = new int[V];
        var visited = new bool[V];

        var paths = new List<List<int>>();


        for (int i = 0; i < V; i++)
        {
            distance[i] = int.MaxValue;
            visited[i] = false;
            paths.Add(new List<int>());
        }

        distance[src] = 0;
        paths[src].Add(src);

        // Find shortest path for all vertices
        for (int count = 0; count < V - 1; count++)
        {
            // Pick the minimum distance vertex
            // from the set of vertices not yet
            // processed. u is always equal to
            // src in first iteration.
            int u = minDistance(distance, visited);

            // Mark the picked vertex as processed
            visited[u] = true;

            // Update dist value of the adjacent
            // vertices of the picked vertex.
            for (int v = 0; v < V; v++)

                // Update dist[v] only if is not in
                // sptSet, there is an edge from u
                // to v, and total weight of path
                // from src to v through u is smaller
                // than current value of dist[v]
                if (
                    graph[u, v] != 0 &&
                    !visited[v] &&
                    distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v]
                    )
                {
                    distance[v] = distance[u] + graph[u, v];
                    paths[v] = new List<int>(paths[u]) { v };
                }
        }

        // print the constructed distance array
        printSolution(distance, V, paths);
    }
    public int minDistance(int[] distance, bool[] visited)
    {
        var min = int.MaxValue;
        var min_index = -1;

        for (int i = 0; i < V; i++)
            if (!visited[i] && distance[i] <= min)
            {
                min = distance[i];
                min_index = i;
            }

        return min_index;
    }

}