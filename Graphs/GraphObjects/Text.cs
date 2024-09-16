using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Text", IsReference = true)]
internal class Text : AGraphElement
{
    [DataMember]
    internal override int Id { get; set; }
    [DataMember]
    internal override Position Position { get; set; }
    [DataMember]
    internal override string Content { get; set; }
    [DataMember]
    internal override double? Weight { get; set; } = null;

    internal static int num = 0;

    internal Text(Position position, string content)
    {
        Id = num++;
        Position = position;
        Content = content;
        Weight = null;
    }

    internal override bool Equals(AGraphElement? other)
    {
        if(other is null) return false;
        if(typeof(Text) != other.GetType()) return false;
        return this.Id == other.Id;
    }

    public override string ToString() => string.Format("TextElement {0}: {1} {2}", Id, Content, Position);
}