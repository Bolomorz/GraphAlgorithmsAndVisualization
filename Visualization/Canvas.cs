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
        double theta = Math.Atan2(end.Y - start.Y, end.X - start.X) * 180 / Math.PI;

        Position tip = new(){ X = start.X + ((end.X - start.X) / 1.35), Y = start.Y + ((end.Y - start.Y) / 1.35)};
        Position lpoint = new(){ X = tip.X + 3, Y = tip.Y + 15};
        Position rpoint = new(){ X = tip.X - 3, Y = tip.Y + 15};

        double angle = theta + 90;
        var left = Rotate(lpoint, tip, angle);
        var right = Rotate(rpoint, tip, angle);

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
        double x = Math.Cos(angle) * (pos.X - center.X) - Math.Sin(angle) * (pos.Y - center.Y) + center.X;
        double y = Math.Sin(angle) * (pos.X - center.X) + Math.Cos(angle) * (pos.Y - center.Y) + center.Y;
        return new(){ X = x, Y = y };
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