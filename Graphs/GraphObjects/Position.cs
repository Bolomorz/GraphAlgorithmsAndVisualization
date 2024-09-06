using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Position", IsReference = true)]
internal class Position
{
    [DataMember]
    internal required int X { get; set; } = 0;
    [DataMember]
    internal required int Y { get; set; } = 0;

    public override string ToString() => string.Format("[{0}|{1}]", X, Y);
}