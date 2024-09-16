using System.Drawing;
using GraphAlgorithmsAndVisualization.Graphs;
using GraphAlgorithmsAndVisualization.Models;

namespace GraphAlgorithmsAndVisualization.Visualization;

internal class CanvasGraph
{
    private AbstractGraphElement? ActiveGraphElement { get; set; }
    private Graph Graph { get; set; }
    private Canvas Canvas { get; set; }
    private List<Text> Texts { get; set; }

    internal CanvasGraph(GraphType type, GraphWeighting weighting)
    {
        Graph = new(type, weighting);
        Canvas = new();
        Texts = new();
        ActiveGraphElement = null;
    }

    #region Drawing
    private void DrawGraph()
    {
        Canvas.Clear();
        foreach(var vertex in Graph.Vertices)
        {
            Color color = ActiveGraphElement is not null && ActiveGraphElement.Equals(vertex) ? Color.Blue : Color.Black;
            Canvas.DrawCircle(vertex.Position, VisualizationSettings.VertexRadius, color);
            Canvas.DrawText(vertex.Position, vertex.Content, color);
        }
        foreach(var edge in Graph.Edges)
        {
            Color color = ActiveGraphElement is not null && ActiveGraphElement.Equals(edge) ? Color.Blue : Color.Black;
            Canvas.DrawLine(edge.Vertex1.Position, edge.Position, color);
            Canvas.DrawLine(edge.Position, edge.Vertex2.Position, color);
            if(Graph.GraphType == GraphType.Directed)
            {
                Canvas.DrawArrow(edge.Vertex1.Position, edge.Position, color);
                Canvas.DrawArrow(edge.Position, edge.Vertex2.Position, color);
            }
            Canvas.DrawText(edge.Position, edge.Content + ": " + edge.Weight, color);
        }
        foreach(var text in Texts)
        {
            Color color = ActiveGraphElement is not null && ActiveGraphElement.Equals(text) ? Color.Blue : Color.Black;
            Canvas.DrawText(text.Position, text.Content, color);
        }
    }
    internal Canvas DrawCanvas()
    {
        DrawGraph();
        return Canvas;
    }
    #endregion

    #region HandleInput
    internal void HandleDelete()
    {
        if(ActiveGraphElement is not null && ActiveGraphElement.GetType() == typeof(Vertex))
        {
            Graph.RemoveVertex((Vertex)ActiveGraphElement);
            ActiveGraphElement = null;
        }
        else if(ActiveGraphElement is not null && ActiveGraphElement.GetType() == typeof(Edge))
        {
            Graph.RemoveEdge((Edge)ActiveGraphElement);
            ActiveGraphElement = null;
        }
        else if(ActiveGraphElement is not null && ActiveGraphElement.GetType() == typeof(Text))
        {
            Texts.Remove((Text)ActiveGraphElement);
            ActiveGraphElement = null;
        }
    }
    internal void HandleRightClick(Position mouse)
    {
        if(ActiveGraphElement is not null) ActiveGraphElement.Position = mouse;
        else
        {
            var text = TextElementOfPosition(mouse);
            if(text is null)
            {
                Text textelement = new(mouse, "Text");
                Texts.Add(textelement);
                ActiveGraphElement = textelement;
            }
            else ActiveGraphElement = text;
        }
    }
    internal CommandTarget HandleLeftClick(Position mouse)
    {
        if(ActiveGraphElement is not null && ActiveGraphElement.GetType() == typeof(Text))
        {
            var text = TextElementOfPosition(mouse);
            if(ActiveGraphElement.Equals(text)) return new(){ Command = Command.GraphElement, Target = text};
            else { ActiveGraphElement.Position = mouse; ActiveGraphElement = null; }
        }
        else if(ActiveGraphElement is not null && ActiveGraphElement.GetType() == typeof(Vertex))
        {
            var vertex = VertexOfPosition(mouse);
            if(vertex is not null && !ActiveGraphElement.Equals(vertex))
            {
                Position pos = new(){ X = (vertex.Position.X + ActiveGraphElement.Position.X)/2, Y = (vertex.Position.Y + ActiveGraphElement.Position.Y)/2};
                string name = ActiveGraphElement.Content + " " + vertex.Content;
                if(Graph.GraphWeighting == GraphWeighting.Weighted) Graph.AddEdge(new Edge(name, (Vertex)ActiveGraphElement, vertex, pos, 1));
                else Graph.AddEdge(new Edge(name, (Vertex)ActiveGraphElement, vertex, pos));
                ActiveGraphElement = null;
            }
            else if(vertex is not null && ActiveGraphElement.Equals(vertex)) return new(){ Command = Command.GraphElement, Target = vertex};
            else { ActiveGraphElement.Position = mouse; ActiveGraphElement = null; }
        }
        else if(ActiveGraphElement is not null && ActiveGraphElement.GetType() == typeof(Edge))
        {
            var edge = EdgeOfPosition(mouse);
            if(edge is not null && ActiveGraphElement.Equals(edge)) return new(){ Command = Command.GraphElement, Target = edge};
            else { ActiveGraphElement.Position = mouse; ActiveGraphElement = null; }
        }
        else
        {
            var vertex = VertexOfPosition(mouse);
            var edge = EdgeOfPosition(mouse);
            var text = TextElementOfPosition(mouse);
            if(vertex is not null) ActiveGraphElement = vertex;
            else if(edge is not null) ActiveGraphElement = edge;
            else if(text is not null) ActiveGraphElement = text;
            else
            {
                Vertex vnew = new(mouse);
                Graph.AddVertex(vnew);
            }
        }
        return new(){Command = Command.None};
    }
    internal List<string> HandleCommand(string command)
    {
        command = command.ToLower();
        var cparts = command.Split(' ');
        List<string> lines = new();
        switch(cparts[0])
        {
            case "help":
                lines.Add("[Help]");
                lines.Add("[Controls]");
                lines.Add("\tenter 'controls' for controls");
                lines.Add("[Algorithms]");
                lines.Add("\tenter vertices (source, target) by name");
                lines.Add("[Dijkstra]");
                lines.Add("\tcalculate shortestpath from source vertex to every other vertex");
                lines.Add("\tsyntax: dijkstra source");
                lines.Add("\tsyntax: dijkstra source target");
                lines.Add("[BellmanFord]");
                lines.Add("\tcalculate distance from source vertex to every other vertex");
                lines.Add("\tsyntax: bellmanford source");
                lines.Add("\tsyntax: bellmanford source target");
            break;
            case "controls":
                lines.Add("[Controls]");
                lines.Add("left click on free space to create new vertex");
                lines.Add("right click on free space to create new text");
                lines.Add("activate vertex by clicking on circle of vertex");
                lines.Add("activate edge by clicking on label of edge");
                lines.Add("activate text by clicking on text");
                lines.Add("activated vertex/edge/text will be highlighted blue");
                lines.Add("[Activated vertex]");
                lines.Add("\tclick on activated vertex to open config menu for vertex");
                lines.Add("\tclick on other vertex to create edge between vertices");
                lines.Add("\tclick on free space to relocate vertex");
                lines.Add("\tpress 'DEL' to delete vertex");
                lines.Add("[Activated edge]");
                lines.Add("\tclick on activated edge to open config menu for edge");
                lines.Add("\tclick on free space to relocate label of edge");
                lines.Add("\tpress 'DEL' to delete edge");
                lines.Add("[Activated text]");
                lines.Add("\tclick on activated text to open config menu for text");
                lines.Add("\tpress 'DEL' to delete text");
            break;
            case "dijkstra":
                if(Graph.GraphType != GraphType.Directed || Graph.GraphWeighting != GraphWeighting.Weighted)
                {
                    lines.Add("[Dijkstra]");
                    lines.Add("cannot use Dijkstra on graph other than WeightedDirectedGraph");
                    break;
                }
                if(cparts.Length == 2)
                {
                    var source = QueryVerticesByName(cparts[1]);
                    if(source is null && cparts[1] != "?")
                    {
                        lines.Add("[Dijkstra]");
                        lines.Add(String.Format("vertex {0} not found", cparts[1]));
                    }
                    else if (source is null)
                    {
                        lines.Add("[Dijkstra]");
                        lines.Add("\tcalculate shortestpath from source vertex to every other vertex");
                        lines.Add("\tsyntax: dijkstra source");
                        lines.Add("\tsyntax: dijkstra source target");
                    }
                    else
                    {
                        Dijkstra djk = new Dijkstra(Graph, source);
                        var result = djk.GetResult();
                        lines.Add("[Dijkstra]");
                        lines.Add("startvertex: " + source);
                        foreach(var ae in result)
                        {
                            lines.Add("-----------------------------------------------");
                            lines.Add("vertex: " + ae.Vertex.ToString());
                            lines.Add("distance: " + ae.Distance);
                            lines.Add("predecessor: " + ae.Predecessor?.ToString() ?? "-");
                            string path = "shortestpath: ";
                            if(ae.ShortestPath is not null)
                            {
                                foreach(var ele in ae.ShortestPath)
                                {
                                    path += "(" + ele + ") ";
                                }
                                lines.Add(path);
                            }
                        }
                    }
                }
                else if(cparts.Length == 3)
                {
                    var source = QueryVerticesByName(cparts[1]);
                    var target = QueryVerticesByName(cparts[2]);
                    if(source is null)
                    {
                        lines.Add("[Dijkstra]");
                        lines.Add(String.Format("vertex {0} not found", cparts[1]));
                    }
                    else if(target is null)
                    {
                        lines.Add("[Dijkstra]");
                        lines.Add(String.Format("vertex {0} not found", cparts[1]));
                    }
                    else
                    {
                        Dijkstra djk = new Dijkstra(Graph, source);
                        var result = djk.GetResult(target);
                        lines.Add("[Dijkstra]");
                        lines.Add("startvertex: " + source);
                        lines.Add("-----------------------------------------------");
                        if (result is not null)
                        {
                            lines.Add("vertex: " + result.Vertex.ToString());
                            lines.Add("distance: " + result.Distance);
                            lines.Add("predecessor: " + result.Predecessor?.ToString() ?? "-");
                            if(result.ShortestPath is not null)
                            {
                                string path = "shortestpath: ";
                                foreach (var ele in result.ShortestPath)
                                {
                                    path += "(" + ele + ") ";
                                }
                                lines.Add(path);
                            }
                        }
                        else
                        {
                            lines.Add(String.Format("vertex {0} not found", target));
                        }
                    }
                }
                else
                {
                    lines.Add("[Dijkstra]");
                    lines.Add("\tcalculate shortestpath from source vertex to every other vertex");
                    lines.Add("\tsyntax: dijkstra source");
                    lines.Add("\tsyntax: dijkstra source target");
                }
            break;
            
            case "bellmanford":
                if (Graph.GraphType != GraphType.Directed || Graph.GraphWeighting != GraphWeighting.Weighted)
                {
                    lines.Add("[BellmanFord]");
                    lines.Add("cannot use BellmanFord on graph other than WeightedDirectedGraph");
                    break;
                }
                if(cparts.Length == 2)
                {
                    var source = QueryVerticesByName(cparts[1]);
                    if(source is null && cparts[1] != "?")
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add(String.Format("vertex {0} not found", cparts[1]));
                    }
                    else if(source is null)
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add("\tcalculate distance from source vertex to every other vertex");
                        lines.Add("\tsyntax: bellmanford source");
                        lines.Add("\tsyntax: bellmanford source target");
                    }
                    else
                    {
                        var bf = new BellmanFord(Graph, source);
                        var result = bf.GetResult();
                        lines.Add("[BellmanFord]");
                        lines.Add("startvertex: " + source);
                        foreach (var ae in result)
                        {
                            lines.Add("-----------------------------------------------");
                            lines.Add("vertex: " + ae.Vertex.ToString());
                            lines.Add("distance: " + ae.Distance);
                            lines.Add("predecessor: " + ae.Predecessor?.ToString() ?? "-");
                        }
                    }
                }
                else if(cparts.Length == 3)
                {
                    var source = QueryVerticesByName(cparts[1]);
                    var target = QueryVerticesByName(cparts[2]);
                    if (cparts[1] == "?" || cparts[2] == "?")
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add("\tcalculate distance from source vertex to every other vertex");
                        lines.Add("\tsyntax: bellmanford source");
                        lines.Add("\tsyntax: bellmanford source target");
                    }
                    else if(source is null)
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add(String.Format("vertex {0} not found", cparts[1]));
                    }
                    else if(target is null)
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add(String.Format("vertex {0} not found", cparts[2]));
                    }
                    else
                    {
                        var bf = new BellmanFord(Graph, source);
                        var result = bf.GetResult(target);
                        lines.Add("[BellmanFord]");
                        lines.Add("startvertex: " + source);
                        if (result is not null)
                        {
                            lines.Add("-----------------------------------------------");
                            lines.Add("vertex: " + result.Vertex.ToString());
                            lines.Add("distance: " + result.Distance);
                            lines.Add("predecessor: " + result.Predecessor?.ToString() ?? "-");
                        }
                        else
                        {
                            lines.Add(String.Format("vertex {0} not found", target));
                        }
                    }
                }
                else
                {
                    lines.Add("[BellmanFord]");
                    lines.Add("\tcalculate distance from source vertex to every other vertex");
                    lines.Add("\tsyntax: bellmanford source");
                    lines.Add("\tsyntax: bellmanford source target");
                }
            break;
            
            default:
                lines.Add("enter 'help' for help");
                lines.Add("enter 'controls' for controls");
            break;
        }
        return lines;
    }
    internal void HandleInput(GraphElementModel gem)
    {
        if(ActiveGraphElement is not null)
        {
            ActiveGraphElement.Content = gem.Content;
            ActiveGraphElement.Weight = gem.Weight is null ? null : double.Parse(gem.Weight);
            ActiveGraphElement = null;
        }
    }
    #endregion

    #region QueryByName
    private Vertex? QueryVerticesByName(string content)
    {
        foreach(var vertex in Graph.Vertices) if(vertex.Content == content) return vertex;
        return null;
    }
    #endregion

    #region IsInGeometryOfPosition
    private bool IsPointInCircleOfVertex(Position pos, Vertex vertex)
    {
        var lhs = Math.Pow(vertex.Position.X - pos.X, 2) + Math.Pow(vertex.Position.Y - pos.Y, 2);
        var rhs = Math.Pow(VisualizationSettings.VertexRadius, 2);
        return lhs <= rhs;
    }
    private bool IsPointInRectangleOfEdge(Position pos, Edge edge)
    {
        string line = edge.Content + ": " + edge.Weight;
        var size = VisualizationSettings.GetTextSize(line);
        var left = edge.Position.X - size.width / 2;
        var right = edge.Position.X + size.width / 2;
        var top = edge.Position.Y - size.height / 2 - 10;
        var bottom = edge.Position.Y + size.height / 2 + 10;
        return pos.X >= left && pos.X <= right && pos.Y >= top && pos.Y <= bottom;
    }
    private bool IsPointInRectangleOfTextElement(Position pos, Text text)
    {
        string line = text.Content;
        var size = VisualizationSettings.GetTextSize(line);
        var left = text.Position.X - size.width / 2;
        var right = text.Position.X + size.width / 2;
        var top = text.Position.Y - size.height / 2 - 10;
        var bottom = text.Position.Y + size.height / 2 + 10;
        return pos.X >= left && pos.X <= right && pos.Y >= top && pos.Y <= bottom;
    }
    #endregion

    #region ElementOfPosition
    private Vertex? VertexOfPosition(Position pos)
    {
        foreach(var vertex in Graph.Vertices) if(IsPointInCircleOfVertex(pos, vertex)) return vertex;
        return null;
    }
    private Edge? EdgeOfPosition(Position pos)
    {
        foreach(var edge in Graph.Edges) if(IsPointInRectangleOfEdge(pos, edge)) return edge;
        return null;
    }
    private Text? TextElementOfPosition(Position pos)
    {
        foreach(var text in Texts) if(IsPointInRectangleOfTextElement(pos, text)) return text;
        return null;
    }
    #endregion
}

internal enum Command { None, GraphElement}
internal class CommandTarget
{
    internal required Command Command { get; set; }
    internal AbstractGraphElement? Target { get; set; }
}
