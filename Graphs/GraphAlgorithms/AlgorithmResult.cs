namespace GraphAlgorithmsAndVisualization.Graphs;

internal class AlgorithmElement
{
    internal required Vertex Vertex { get; set; }
    internal required double Distance { get; set; }
    internal required Vertex? Predecessor { get; set; }
    internal required List<Vertex>? ShortestPath { get; set; }
    internal static AlgorithmElement DijkstraElement(Vertex vertex)
    {
        return new()
        {
            Vertex = vertex,
            Distance = double.PositiveInfinity,
            ShortestPath = new(),
            Predecessor = null
        };
    }
    internal static AlgorithmElement BellmanFordElement(Vertex vertex)
    {
        return new()
        {
            Vertex = vertex,
            Distance = double.PositiveInfinity,
            Predecessor = null,
            ShortestPath = null,
        };
    }
}

internal interface IGraphAlgorithm
{
    List<AlgorithmElement> GetResult();
    AlgorithmElement? GetResult(Vertex endvertex);
}