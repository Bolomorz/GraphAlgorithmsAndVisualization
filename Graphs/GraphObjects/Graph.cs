using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "GraphType")]
internal enum GraphType { [EnumMember] Directed, [EnumMember] Undirected }
[DataContract(Name = "GraphWeighting")]
internal enum GraphWeighting { [EnumMember] Weighted, [EnumMember] NonWeighted}

[DataContract(Name = "Graph", IsReference = true)]
internal class Graph
{
    [DataMember]
    internal List<Vertex> Vertices { get; set; }
    [DataMember]
    internal List<Edge> Edges { get; set; } 
    [DataMember]
    internal GraphType GraphType { get; set;}
    [DataMember]
    internal GraphWeighting GraphWeighting { get; set; }
    [DataMember]
    internal int Id { get; set; }
    protected static int num = 0;
    internal Graph(GraphType type, GraphWeighting weighting)
    {
        Id = num++;
        Vertices = new();
        Edges = new();
        GraphType = type;
        GraphWeighting = weighting;
    }

    internal void AddVertex(Vertex vertex)
    {
        if(Vertices.FirstOrDefault(k => k.Id == vertex.Id) is null) Vertices.Add(vertex);
    }
    internal void RemoveEdge(Edge edge)
    {
        var oldedge = Edges.FirstOrDefault(k => k.Id == edge.Id);
        if(oldedge is not null) 
        {
            switch(this.GraphType)
            {
                case GraphType.Directed:
                oldedge.Vertex1.RemoveAdjacent(oldedge.Vertex2);
                break;
                case GraphType.Undirected:
                oldedge.Vertex1.RemoveAdjacent(oldedge.Vertex2);
                oldedge.Vertex2.RemoveAdjacent(oldedge.Vertex1);
                break;
            }
            Edges.Remove(oldedge);
        }
    }
    internal void RemoveVertex(Vertex vertex)
    {
        var oldvertex = Vertices.FirstOrDefault(k => k.Id == vertex.Id);
        if(oldvertex is not null)
        {
            var edges = Edges.Where(k => k.Vertex1.Id == oldvertex.Id || k.Vertex2.Id == oldvertex.Id).ToList();
            foreach(var edge in edges)
            {
                RemoveEdge(edge);
            }
            Vertices.Remove(oldvertex);
        }
    }
    internal bool IsEdgeInGraph(Edge edge)
    {
        switch(this.GraphType)
        {
            case GraphType.Directed:
            foreach(var oldedge in Edges) if(oldedge.Vertex1.Id == edge.Vertex1.Id && oldedge.Vertex2.Id == edge.Vertex2.Id) return true;
            return false;
            case GraphType.Undirected:
            foreach(var oldedge in Edges) if((oldedge.Vertex1.Id == edge.Vertex1.Id || oldedge.Vertex1.Id == edge.Vertex2.Id) && (oldedge.Vertex2.Id == edge.Vertex1.Id || oldedge.Vertex2.Id == edge.Vertex2.Id)) return true;
            return false;
            default: return true;
        }
    }
    internal void AddEdge(Edge edge)
    {
        var oldedge = Edges.FirstOrDefault(k => k.Id == edge.Id);
        if(oldedge is null && !IsEdgeInGraph(edge))
        {
            switch(this.GraphType)
            {
                case GraphType.Undirected:
                AddVertex(edge.Vertex1);
                AddVertex(edge.Vertex2);
                edge.Vertex1.AddAdjacent(edge.Vertex2);
                edge.Vertex2.AddAdjacent(edge.Vertex1);
                Edges.Add(edge);
                break;
                case GraphType.Directed:
                AddVertex(edge.Vertex1);
                AddVertex(edge.Vertex2);
                edge.Vertex1.AddAdjacent(edge.Vertex2);
                Edges.Add(edge);
                break;
            }
        }
    }
}

