using GraphAlgorithmsAndVisualization.Graphs;
using GraphAlgorithmsAndVisualization.Visualization;

namespace GraphAlgorithmsAndVisualization.Models;

public enum Partial { Graph, Solution, Edge, Vertex, TextElement}
public class GraphModel
{
    public required string X { get; set; }
    public required string Y { get; set; }
    internal Canvas? Canvas { get; set; }
}
public class SolutionModel
{
    internal List<string>? Lines { get; set; }
}
public class EdgeModel
{
    public required string Name { get; set; }
    public required string Weight { get; set; }
    internal Edge? Edge { get; set; }
}
public class VertexModel
{
    public required string Name { get; set; }
    internal Vertex? Vertex { get; set; }
}
public class TextElementModel
{
    public required string Content { get; set; }
    internal TextElement? TextElement { get; set; }
}

