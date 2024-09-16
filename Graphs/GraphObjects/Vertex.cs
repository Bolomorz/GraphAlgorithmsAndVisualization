using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Vertex", IsReference = true)]
internal class Vertex : AGraphElement
{
    [DataMember]
    internal override int Id { get; set; }
    [DataMember]
    internal override string Content { get; set; }
    [DataMember]
    internal override Position Position { get; set; }
    internal override double? Weight { get; set; }

    [DataMember]
    internal List<Vertex> Adjacents { get; set; }

    protected static int num = 0;

    internal Vertex(Position position)
    {
        Id = num++;
        Content = "vertex" + Id;
        Position = position;
        Adjacents = new();
        Weight = null;
    }

    internal int? GetAdjacentIndex(Vertex adj)
    {
        for(int i = 0; i < Adjacents.Count; i++) if(Adjacents[i].Id == adj.Id) return i;
        return null;
    }

    internal void AddAdjacent(Vertex adj)
    {
        if(GetAdjacentIndex(adj) is null) Adjacents.Add(adj);
    }

    internal void RemoveAdjacent(Vertex adj)
    {
        Adjacents.Remove(adj);
    }

    internal override bool Equals(AGraphElement? other)
    {
        if(other is null) return false;
        if(typeof(Vertex) != other.GetType()) return false;
        return this.Id == other.Id;
    }

    public override string ToString() => string.Format("Vertex {0}: {1} {2}", Id, Content, Position);
}