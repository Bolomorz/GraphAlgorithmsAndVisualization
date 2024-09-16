using System.Runtime.Serialization;

namespace GraphAlgorithmsAndVisualization.Graphs;

[DataContract(Name = "Position", IsReference = true)]
internal class Position
{
    [DataMember]
    internal required double X { get; set; } = 0;
    [DataMember]
    internal required double Y { get; set; } = 0;

    public override string ToString() => string.Format("[{0}|{1}]", Math.Round(X, 0).ToString("0.#"), Math.Round(Y, 0).ToString("0.#"));
}