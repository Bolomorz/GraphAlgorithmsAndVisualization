using GraphAlgorithmsAndVisualization.Graphs;
using GraphAlgorithmsAndVisualization.Visualization;

namespace GraphAlgorithmsAndVisualization.Models;

public enum Partial { Graph, Solution, Edge, Vertex, TextElement}
public class GraphModel
{
    public required string X { get; set; }
    public required string Y { get; set; }
    public required string Command { get; set; }
    internal Canvas? Canvas { get; set; }
}
public class SolutionModel
{
    internal string? Command { get; set; }
    internal List<string>? Lines { get; set; }
}
public class GraphElementModel
{
    public required string Content { get; set; }
    public required string? Weight { get; set; }
    internal AbstractGraphElement? Edge { get; set; }
}

