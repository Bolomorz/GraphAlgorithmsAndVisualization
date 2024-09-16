using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Edge", IsReference = true)]
internal class Edge : AbstractGraphElement
{
    [DataMember]
    internal override int Id { get; set; }
    [DataMember]
    internal override string Content { get; set; }
    [DataMember]
    internal override Position Position { get; set; }
    [DataMember]
    internal override double? Weight { get; set; }

    [DataMember]
    internal Vertex Vertex1 { get; set; }
    [DataMember]
    internal Vertex Vertex2 { get; set;}

    protected static int num = 0;

    internal Edge(string content, Vertex vertex1, Vertex vertex2, Position position)
    {
        Id = num++;
        Vertex1 = vertex1;
        Vertex2 = vertex2;
        Content = content;
        Position = position;
        Weight = null;
    }

    internal Edge(string content, Vertex vertex1, Vertex vertex2, Position position, double weight)
    {
        Id = num++;
        Vertex1 = vertex1;
        Vertex2 = vertex2;
        Content = content;
        Position = position;
        Weight = weight;
    }

    internal override bool Equals(AbstractGraphElement? other)
    {
        if(other is null) return false;
        if(typeof(Edge) != other.GetType()) return false;
        return this.Id == other.Id;
    }

    public override string ToString() => string.Format("Edge {0}: {1} {2}", Id, Content, Position);
}