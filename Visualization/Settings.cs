using GraphAlgorithmsAndVisualization.Models;
using SixLabors.Fonts;

namespace GraphAlgorithmsAndVisualization.Visualization;

internal static class VisualizationSettings
{
    internal static double VertexRadius = 100;
    internal static float FontSize = 50;

    internal static (double width, double height) GetTextSize(string text)
    {
        var size = TextMeasurer.MeasureSize(text, new TextOptions(new Font(SystemFonts.Get("FreeMono"), FontSize)));
        return (size.Width, size.Height);
    }
}

internal static class VisualizationGlobal
{
    internal static CanvasGraph? CurrentCanvasGraph = null;
    internal static Partial CurrentPartial = Partial.Graph;
}