using GraphAlgorithmsAndVisualization.Graphs;
using GraphAlgorithmsAndVisualization.Models;
using GraphAlgorithmsAndVisualization.Visualization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GraphAlgorithmsAndVisualization.Pages.GraphVisualization;

public class IndexModel : PageModel
{
    #region AspForFields
    [BindProperty]
    public GraphModel GraphModel { get; set; }
    #endregion
    public IndexModel()
    {
        VisualizationGlobal.CurrentPartial = Models.Partial.Graph;
        GraphModel = new(){ X = "", Y = ""};
    }

    public void OnGet()
    {
        VisualizationGlobal.CurrentCanvasGraph = new(Graphs.GraphType.Directed, Graphs.GraphWeighting.Weighted);
    }

    public IActionResult OnPostGraphCanvasClick()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null && GraphModel.X != "" && GraphModel.Y != "")
        {
            Position pos = new(){ X = Convert.ToDouble(GraphModel.X), Y = Convert.ToDouble(GraphModel.Y)};
            var cmd = VisualizationGlobal.CurrentCanvasGraph.HandleLeftClick(pos);
            var canvas = VisualizationGlobal.CurrentCanvasGraph.DrawCanvas();
            GraphModel.Canvas = canvas;
        }
        return Page();
    }
}