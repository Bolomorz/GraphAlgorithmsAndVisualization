namespace GraphAlgorithmsAndVisualization.Graphs;

internal class BellmanFord : IGraphAlgorithm
{
    private List<AlgorithmElement> Elements { get; set;}
    private Graph Graph { get; set; }
    private Vertex StartVertex { get; set; }

    internal BellmanFord(Graph graph, Vertex startvertex)
    {
        if(graph.GraphType != GraphType.Directed || graph.GraphWeighting != GraphWeighting.Weighted) throw new Exception("Can only use BellmanFord on Weighted-Directed Graphs.");
        Elements = new();
        Graph = graph;
        StartVertex = startvertex;
        Init();
        RelaxEdges();
        CheckForNegativeWeightCycles();
    }

    private void Init()
    {
        foreach(var vertex in Graph.Vertices)
        {
            var ae = AlgorithmElement.BellmanFordElement(vertex);
            if(ae.Vertex.Id == StartVertex.Id) ae.Distance = 0;
            Elements.Add(ae);
        }
    }
    private void RelaxEdges()
    {
        for(int i = 0; i < Elements.Count-1; i++)
        {
            foreach(var edge in Graph.Edges)
            {
                var aeu = Elements.First(e => e.Vertex.Id == edge.Vertex1.Id);
                var aev = Elements.First(e => e.Vertex.Id == edge.Vertex2.Id);
                if(aeu is not null && aev is not null)
                {
                    if(edge.Weight is not null && aeu.Distance + edge.Weight < aev.Distance)
                    {
                        aev.Distance = aeu.Distance + (double)edge.Weight;
                        aev.Predecessor = aeu.Vertex;
                    }
                }
            }
        }
    }
    private void CheckForNegativeWeightCycles()
    {
        foreach(var edge in Graph.Edges)
        {
            var aeu = Elements.First(e => e.Vertex.Id == edge.Vertex1.Id);
            var aev = Elements.First(e => e.Vertex.Id == edge.Vertex2.Id);

            if(aeu.Distance + edge.Weight < aev.Distance)
            {
                aev.Predecessor = aeu.Vertex;
                List<bool> visited = new();
                foreach(var ae in Elements) visited.Add(false);
                visited[Elements.IndexOf(aev)] = true;
                while(!visited[Elements.IndexOf(aeu)])
                {
                    visited[Elements.IndexOf(aeu)] = true;
                    aeu = Elements.First(e => e.Vertex.Id == aeu.Predecessor?.Id);
                }
                List<Vertex> ncycle = new(){aeu.Vertex};
                var v = aeu.Predecessor;
                while(v is not null && v.Id != aeu.Vertex.Id)
                {
                    ncycle.Add(v);
                    v = Elements.First(e => e.Vertex.Id == v.Id)?.Predecessor;
                }
                throw new Exception("Graph contains a negative-weight cycle");
            }
        }
    }

    public List<AlgorithmElement> GetResult() => Elements;
    public AlgorithmElement? GetResult(Vertex endvertex)
    {
        foreach(var ae in Elements) if(ae.Vertex.Id == endvertex.Id) return ae;
        return null;
    }
}