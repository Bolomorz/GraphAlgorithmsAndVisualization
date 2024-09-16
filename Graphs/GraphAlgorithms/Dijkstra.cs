namespace GraphAlgorithmsAndVisualization.Graphs;

internal class Dijkstra : IGraphAlgorithm
{
    private List<AlgorithmElement> Elements { get; set; }
    private List<Vertex> Q { get; set; }
    private Graph Graph { get; set; }
    private Vertex StartVertex { get; set; }

    internal Dijkstra(Graph graph, Vertex startvertex)
    {
        if(graph.GraphType != GraphType.Directed || graph.GraphWeighting != GraphWeighting.Weighted) throw new Exception("Can only use Dijkstra on Weighted Directed Graph");
        Graph = graph;
        Q = new();
        Elements = new();
        StartVertex = startvertex;
        Init();
        DijkstraCalculation();
        foreach(var ae in Elements) ae.ShortestPath = ShortestPath(ae.Vertex);
    }

    private void Init()
    {
        foreach(var vertex in Graph.Vertices)
        {
            var ae = AlgorithmElement.DijkstraElement(vertex);
            if(vertex.Id == StartVertex.Id) ae.Distance = 0;
            Elements.Add(ae);
            Q.Add(vertex);
        }
    }
    private void DijkstraCalculation()
    {
        while(Q.Count > 0)
        {
            var index = IndexOfVertexWithSmallestDistanceInQ();
            if(index == -1) break;
            var u = Q[index];
            Q.Remove(u);
            foreach(var v in u.Adjacents) if(IsInQ(v)) DistanceUpdate(u, v);
        }
    }
    private int IndexOfVertexWithSmallestDistanceInQ()
    {
        var dist = double.PositiveInfinity;
        var index = -1;
        for(int i = 0; i < Q.Count; i++)
        {
            var u = Q[i];
            var ae = Elements.First(e => e.Vertex.Id == u.Id);
            if(ae is not null && ae.Distance < dist)
            {
                index = i;
                dist = ae.Distance;
            }
        }
        return index;
    }
    private bool IsInQ(Vertex v)
    {
        foreach(var q in Q) if(q.Id == v.Id) return true;
        return false;
    }
    private void DistanceUpdate(Vertex u, Vertex v)
    {
        var aeu = Elements.First(e => e.Vertex.Id == u.Id);
        var aev = Elements.First(e => e.Vertex.Id == v.Id);
        if(aeu is not null && aev is not null)
        {
            var alt = aeu.Distance + WeightingOfEdge(u, v);
            if(alt is not null && alt < aev.Distance)
            {
                aev.Distance = (double)alt;
                aev.Predecessor = u;
            }
        }
    }
    private double? WeightingOfEdge(Vertex u, Vertex v)
    {
        foreach(var edge in Graph.Edges) if(edge.Vertex1 == u && edge.Vertex2 == v) return edge.Weight;
        return null;
    }
    private List<Vertex> ShortestPath(Vertex v)
    {
        List<Vertex> path = new();
        path.Add(v);
        var u = v;
        var ae = Elements.First(e => e.Vertex.Id == v.Id);
        while(ae is not null)
        {
            if(ae.Predecessor is null) break; 
            else
            {
                u = ae.Predecessor;
                path.Insert(0, u);
                ae = Elements.First(e => e.Vertex.Id == u.Id);
            }
        }
        return path;
    }
    public List<AlgorithmElement> GetResult() => Elements;
    public AlgorithmElement? GetResult(Vertex endvertex)
    {
        foreach(var ae in Elements) if(ae.Vertex.Id == endvertex.Id) return ae;
        return null;
    }
}