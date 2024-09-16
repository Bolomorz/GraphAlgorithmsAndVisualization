namespace GraphAlgorithmsAndVisualization.Graphs;

internal abstract class AbstractGraphElement
{
    internal abstract int Id { get; set; }
    internal abstract string Content { get; set; }
    internal abstract double? Weight { get; set; }
    internal abstract Position Position { get; set; }
    internal abstract bool Equals(AbstractGraphElement? other);
    public abstract override string ToString();
}