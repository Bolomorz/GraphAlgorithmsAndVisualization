using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Vertex", IsReference = true)]
internal class Vertex
{
    [DataMember]
    internal int Id { get; set; }
    [DataMember]
    internal string Name { get; set; }
    [DataMember]
    internal Position Position { get; set; }
    [DataMember]
    internal List<Vertex> Adjacents { get; set; }

    protected static int num = 0;

    internal Vertex(Position position)
    {
        Id = num++;
        Name = "vertex" + Id;
        Position = position;
        Adjacents = new();
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

    public override string ToString() => string.Format("Vertex {0}: {1} {2}", Id, Name, Position);
}