using System.Drawing;
using GraphAlgorithmsAndVisualization.Graphs;
using GraphAlgorithmsAndVisualization.Models;

namespace GraphAlgorithmsAndVisualization.Visualization;

internal class CanvasGraph
{
    private TextElement? ActiveTextElement { get; set; }
    private Vertex? ActiveVertex { get; set; }
    private Edge? ActiveEdge { get; set; }
    private Graph Graph { get; set; }
    private Canvas Canvas { get; set; }
    private List<TextElement> TextElements { get; set; }

    internal CanvasGraph(GraphType type, GraphWeighting weighting)
    {
        Graph = new(type, weighting);
        Canvas = new();
        TextElements = new();
        ActiveTextElement = null;
        ActiveVertex = null;
        ActiveEdge = null;
    }

    #region Drawing
    private void DrawGraph()
    {
        Canvas.Clear();
        foreach(var vertex in Graph.Vertices)
        {
            Color color = ActiveVertex is not null && ActiveVertex.Id == vertex.Id ? Color.Blue : Color.Black;
            Canvas.DrawCircle(vertex.Position, VisualizationSettings.VertexRadius, color);
            Canvas.DrawText(vertex.Position, vertex.Name, color);
        }
        foreach(var edge in Graph.Edges)
        {
            Color color = ActiveEdge is not null && ActiveEdge.Id == edge.Id ? Color.Blue : Color.Black;
            Canvas.DrawLine(edge.Vertex1.Position, edge.Position, color);
            Canvas.DrawLine(edge.Position, edge.Vertex2.Position, color);
            if(Graph.GraphType == GraphType.Directed)
            {
                Canvas.DrawArrow(edge.Vertex1.Position, edge.Position, color);
                Canvas.DrawArrow(edge.Position, edge.Vertex2.Position, color);
            }
            Canvas.DrawText(edge.Position, edge.Name + ": " + edge.Weight, color);
        }
        foreach(var text in TextElements)
        {
            Color color = ActiveTextElement is not null && ActiveTextElement.Id == text.Id ? Color.Blue : Color.Black;
            Canvas.DrawText(text.Position, text.Text, color);
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
        if(ActiveVertex is not null)
        {
            Graph.RemoveVertex(ActiveVertex);
            ActiveVertex = null;
        }
        else if(ActiveEdge is not null)
        {
            Graph.RemoveEdge(ActiveEdge);
            ActiveEdge = null;
        }
        else if(ActiveTextElement is not null)
        {
            TextElements.Remove(ActiveTextElement);
            ActiveTextElement = null;
        }
    }
    internal void HandleRightClick(Position mouse)
    {
        if(ActiveTextElement is not null) ActiveTextElement.Position = mouse;
        else
        {
            var text = TextElementOfPosition(mouse);
            if(text is null)
            {
                TextElement textelement = new(){ Id = TextElement.num++, Color = Color.Black, Position = mouse, Text = ""};
                TextElements.Add(textelement);
                ActiveTextElement = textelement;
            }
            else ActiveTextElement = text;
        }
    }
    internal Command HandleLeftClick(Position mouse)
    {
        if(ActiveTextElement is not null)
        {
            var text = TextElementOfPosition(mouse);
            if(text is not null && ActiveTextElement.Id == text.Id) return new(){ Target = CommandTarget.TextElement, TextElement = text};
            else { ActiveTextElement.Position = mouse; ActiveTextElement = null; }
        }
        else if(ActiveVertex is not null)
        {
            var vertex = VertexOfPosition(mouse);
            if(vertex is not null && ActiveVertex.Id != vertex.Id)
            {
                Position pos = new(){ X = (vertex.Position.X + ActiveVertex.Position.X)/2, Y = (vertex.Position.Y + ActiveVertex.Position.Y)/2};
                string name = ActiveVertex.Name + " " + vertex.Name;
                if(Graph.GraphWeighting == GraphWeighting.Weighted) Graph.AddEdge(new Edge(name, ActiveVertex, vertex, pos, 1));
                else Graph.AddEdge(new Edge(name, ActiveVertex, vertex, pos));
                ActiveVertex = null;
            }
            else if(vertex is not null && ActiveVertex.Id == vertex.Id) return new(){ Target = CommandTarget.Vertex, Vertex = vertex};
            else { ActiveVertex.Position = mouse; ActiveVertex = null; }
        }
        else if(ActiveEdge is not null)
        {
            var edge = EdgeOfPosition(mouse);
            if(edge is not null && ActiveEdge.Id == edge.Id) return new(){ Target = CommandTarget.Edge, Edge = edge};
            else { ActiveEdge.Position = mouse; ActiveEdge = null; }
        }
        else
        {
            var vertex = VertexOfPosition(mouse);
            var edge = EdgeOfPosition(mouse);
            var text = TextElementOfPosition(mouse);
            if(vertex is not null) ActiveVertex = vertex;
            else if(edge is not null) ActiveEdge = edge;
            else if(text is not null) ActiveTextElement = text;
            else
            {
                Vertex vnew = new(mouse);
                Graph.AddVertex(vnew);
            }
        }
        return new(){Target = CommandTarget.None};
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
    internal void HandleInput(VertexModel vm)
    {
        if(ActiveVertex is not null)
        {
            ActiveVertex.Name = vm.Name;
            ActiveVertex = null;
        }
    }
    internal void HandleInput(EdgeModel em)
    {
        if(ActiveEdge is not null)
        {
            ActiveEdge.Name = em.Name;
            ActiveEdge.Weight = Convert.ToDouble(em.Weight);
            ActiveEdge = null;
        }
    }
    internal void HandleInput(TextElementModel tem)
    {
        if(ActiveTextElement is not null)
        {
            ActiveTextElement.Text = tem.Content;
            ActiveTextElement = null;
        }
    }
    #endregion

    #region QueryByName
    private Vertex? QueryVerticesByName(string name)
    {
        foreach(var vertex in Graph.Vertices) if(vertex.Name == name) return vertex;
        return null;
    }
    #endregion

    #region IsInGeometryOfPosition
    private bool IsPointInCircleOfVertex(Position pos, Vertex vertex)
    {
        var lhs = Math.Pow(vertex.Position.X - pos.X, 2) + Math.Pow(vertex.Position.Y - pos.Y, 2);
        var rhs = Math.Pow(VisualizationSettings.VertexRadius/2, 2);
        return lhs <= rhs;
    }
    private bool IsPointInRectangleOfEdge(Position pos, Edge edge)
    {
        string line = edge.Name + ": " + edge.Weight;
        var size = VisualizationSettings.GetTextSize(line);
        var left = edge.Position.X - size.width / 2;
        var right = edge.Position.X + size.width / 2;
        var top = edge.Position.Y - size.height / 2;
        var bottom = edge.Position.Y + size.height / 2;
        return pos.X >= left && pos.X <= right && pos.Y >= top && pos.Y <= bottom;
    }
    private bool IsPointInRectangleOfTextElement(Position pos, TextElement text)
    {
        string line = text.Text;
        var size = VisualizationSettings.GetTextSize(line);
        var left = text.Position.X - size.width / 2;
        var right = text.Position.X + size.width / 2;
        var top = text.Position.Y - size.height / 2;
        var bottom = text.Position.Y + size.height / 2;
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
    private TextElement? TextElementOfPosition(Position pos)
    {
        foreach(var text in TextElements) if(IsPointInRectangleOfTextElement(pos, text)) return text;
        return null;
    }
    #endregion
}

internal enum CommandTarget { None, Vertex, Edge, TextElement}
internal class Command
{
    internal required CommandTarget Target { get; set; }
    internal Vertex? Vertex { get; set; }
    internal Edge? Edge { get; set; }
    internal TextElement? TextElement { get; set; }
}
