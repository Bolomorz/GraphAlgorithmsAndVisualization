using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Edge", IsReference = true)]
internal class Edge
{
    [DataMember]
    internal int Id { get; set; }
    [DataMember]
    internal Vertex Vertex1 { get; set; }
    [DataMember]
    internal Vertex Vertex2 { get; set;}
    [DataMember]
    internal string Name { get; set; }
    [DataMember]
    internal Position Position { get; set; }
    [DataMember]
    internal double? Weight { get; set; }

    protected static int num = 0;

    internal Edge(string name, Vertex vertex1, Vertex vertex2, Position position)
    {
        Id = num++;
        Vertex1 = vertex1;
        Vertex2 = vertex2;
        Name = name;
        Position = position;
        Weight = null;
    }

    internal Edge(string name, Vertex vertex1, Vertex vertex2, Position position, double weight)
    {
        Id = num++;
        Vertex1 = vertex1;
        Vertex2 = vertex2;
        Name = name;
        Position = position;
        Weight = weight;
    }

    public override string ToString() => string.Format("Edge {0}: {1} {2}", Id, Name, Position);
}