using System.Drawing;
using GraphAlgorithmsAndVisualization.Graphs;

namespace GraphAlgorithmsAndVisualization.Visualization;

internal class Canvas
{
    internal List<TextElement> TextElements { get; set; }
    internal List<LineElement> LineElements { get; set; }
    internal List<ArrowElement> ArrowElements { get; set; }
    internal List<CircleElement> CircleElements { get; set; }
    internal Canvas()
    {
        TextElements = new();
        LineElements = new();
        ArrowElements = new();
        CircleElements = new();
    }

    internal void Clear()
    {
        TextElements.Clear();
        LineElements.Clear();
        ArrowElements.Clear();
        CircleElements.Clear();
    }

    internal void DrawCircle(Position position, double radius, Color color)
    {
        CircleElements.Add(new()
        {
            Center = position,
            Radius = radius,
            Color = color
        });
    }
    internal void DrawText(Position position, string text, Color color)
    {
        TextElements.Add(new()
        {
            Position = position,
            Text = text,
            Color = color
        });
    }
    internal void DrawLine(Position start, Position end, Color color)
    {
        LineElements.Add(new()
        {
            Start = start,
            End = end,
            Color = color,
            Thickness = 1
        });
    }
    internal void DrawArrow(Position start, Position end, Color color)
    {
        double theta = Math.Atan2(end.Y - start.Y, end.X - start.X);

        Position tip = new(){ X = (start.X + end.X) / 2, Y = (start.Y + end.Y) / 2};
        Position lpoint = new(){ X = tip.X - 50, Y = tip.Y + 10};
        Position rpoint = new(){ X = tip.X - 50, Y = tip.Y - 10};

        var left = Rotate(lpoint, tip, theta);
        var right = Rotate(rpoint, tip, theta);

        ArrowElements.Add(new()
        {
            Tip = tip,
            Left = left,
            Right = right,
            Color = color
        });
    }
    
    private Position Rotate(Position pos, Position center, double angle)
    {
        double sin = Math.Sin(angle);
        double cos = Math.Cos(angle);
        double px = pos.X - center.X;
        double py = pos.Y - center.Y;
        double x = px * cos - py * sin;
        double y = px * sin + py * cos;

        return new(){ X = x + center.X, Y = y + center.Y };
    }
}

internal class TextElement
{
    internal int Id { get; set; }
    internal required Position Position { get; set; }
    internal required string Text { get; set; }
    internal required Color Color { get; set; }
    internal static int num = 0;
}

internal class LineElement
{
    internal required Position Start { get; set; }
    internal required Position End { get; set; }
    internal required Color Color { get; set; }
    internal required double Thickness { get; set; }
}

internal class ArrowElement
{
    internal required Position Tip { get; set; }
    internal required Position Left { get; set; }
    internal required Position Right { get; set; }
    internal required Color Color { get; set; }
}

internal class CircleElement
{
    internal required Position Center { get; set; }
    internal required double Radius { get; set; }
    internal required Color Color { get; set; }
}