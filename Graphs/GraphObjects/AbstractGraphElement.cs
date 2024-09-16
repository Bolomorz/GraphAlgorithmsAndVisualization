namespace GraphAlgorithmsAndVisualization.Graphs;

internal abstract class AGraphElement
{
    internal abstract int Id { get; set; }
    internal abstract string Content { get; set; }
    internal abstract double? Weight { get; set; }
    internal abstract Position Position { get; set; }
    internal abstract bool Equals(AGraphElement? other);
    public abstract override string ToString();
}