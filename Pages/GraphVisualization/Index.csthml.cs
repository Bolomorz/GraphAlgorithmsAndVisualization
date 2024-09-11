using System.Security.Cryptography.X509Certificates;
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
    [BindProperty]
    public SolutionModel SolutionModel { get; set; }
    #endregion
    public IndexModel()
    {
        GraphModel = new(){ X = "", Y = "", Command = ""};
        SolutionModel = new();
    }

    public void OnGet()
    {
        VisualizationGlobal.CurrentCanvasGraph = new(Graphs.GraphType.Directed, Graphs.GraphWeighting.Weighted);
    }
    
    public IActionResult OnPostSubmit()
    {
        switch(VisualizationGlobal.CurrentPartial)
        {
            case Models.Partial.Solution:
            if(VisualizationGlobal.CurrentCanvasGraph is not null)
            {
                var canvas = VisualizationGlobal.CurrentCanvasGraph.DrawCanvas();
                GraphModel.Canvas = canvas;
            }
            VisualizationGlobal.CurrentPartial = Models.Partial.Graph;
            return Page();
            default:
            VisualizationGlobal.CurrentPartial = Models.Partial.Graph;
            return Page();
        }
    }

    public IActionResult OnPostGraphCanvasClickLeft()
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

    public IActionResult OnPostGraphCanvasClickRight()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null && GraphModel.X != "" && GraphModel.Y != "")
        {
            Position pos = new(){ X = Convert.ToDouble(GraphModel.X), Y = Convert.ToDouble(GraphModel.Y) };
            VisualizationGlobal.CurrentCanvasGraph.HandleRightClick(pos);
            var canvas = VisualizationGlobal.CurrentCanvasGraph.DrawCanvas();
            GraphModel.Canvas = canvas;
        }
        return Page();
    }

    public IActionResult OnPostGraphCanvasDelete()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null)
        {
            VisualizationGlobal.CurrentCanvasGraph.HandleDelete();
            var canvas = VisualizationGlobal.CurrentCanvasGraph.DrawCanvas();
            GraphModel.Canvas = canvas;
        }
        return Page();
    }

    public IActionResult OnPostIssueCommand()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null)
        {
            var lines = VisualizationGlobal.CurrentCanvasGraph.HandleCommand(GraphModel.Command);
            SolutionModel.Lines = lines;
            SolutionModel.Command = GraphModel.Command;
            VisualizationGlobal.CurrentPartial = Models.Partial.Solution;
        }
        return Page();
    }
}