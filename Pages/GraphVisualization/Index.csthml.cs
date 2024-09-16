using System.Security.Cryptography.X509Certificates;
using GraphAlgorithmsAndVisualization.Graphs;
using GraphAlgorithmsAndVisualization.Models;
using GraphAlgorithmsAndVisualization.Visualization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace GraphAlgorithmsAndVisualization.Pages.GraphVisualization;

public class IndexModel : PageModel
{
    #region AspForFields
    [BindProperty]
    public GraphModel GraphModel { get; set; }
    [BindProperty]
    public SolutionModel SolutionModel { get; set; }
    [BindProperty]
    public GraphElementModel ElementModel { get; set; }
    #endregion
    public IndexModel()
    {
        GraphModel = new(){ X = "", Y = "", Command = ""};
        SolutionModel = new();
        ElementModel = new(){ Content = "", Weight = null};
    }

    public void OnGet()
    {
        VisualizationGlobal.CurrentCanvasGraph = new(Graphs.GraphType.Directed, Graphs.GraphWeighting.Weighted);
    }

    public IActionResult OnPostSubmit()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null)
        {
            VisualizationGlobal.CurrentCanvasGraph.HandleInput(ElementModel);
            DisplayGraph();
        }
        return Page();
    }
    
    public IActionResult OnPostClose()
    {
        DisplayGraph();
        return Page();
    }

    public IActionResult OnPostGraphCanvasClickLeft()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null && GraphModel.X != "" && GraphModel.Y != "")
        {
            Position pos = new(){ X = Convert.ToDouble(GraphModel.X), Y = Convert.ToDouble(GraphModel.Y)};
            var cmdtarget = VisualizationGlobal.CurrentCanvasGraph.HandleLeftClick(pos);
            if(cmdtarget.Command == Command.None) DisplayGraph();
            else if(cmdtarget.Command == Command.GraphElement && cmdtarget.Target is not null) DisplayConfig(cmdtarget.Target);
        }
        return Page();
    }

    public IActionResult OnPostGraphCanvasClickRight()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null && GraphModel.X != "" && GraphModel.Y != "")
        {
            Position pos = new(){ X = Convert.ToDouble(GraphModel.X), Y = Convert.ToDouble(GraphModel.Y) };
            VisualizationGlobal.CurrentCanvasGraph.HandleRightClick(pos);
            DisplayGraph();
        }
        return Page();
    }

    public IActionResult OnPostGraphCanvasDelete()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null)
        {
            VisualizationGlobal.CurrentCanvasGraph.HandleDelete();
            DisplayGraph();
        }
        return Page();
    }

    public IActionResult OnPostIssueCommand()
    {
        DisplaySolution();
        return Page();
    }

    private void DisplayConfig(AbstractGraphElement element)
    {
        ElementModel.GraphElement = element;
        VisualizationGlobal.CurrentPartial = Models.Partial.GraphElement;
    }
    private void DisplaySolution()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null)
        {
            var lines = VisualizationGlobal.CurrentCanvasGraph.HandleCommand(GraphModel.Command);
            SolutionModel.Lines = lines;
            SolutionModel.Command = GraphModel.Command;
            VisualizationGlobal.CurrentPartial = Models.Partial.Solution;
        }
    }
    private void DisplayGraph()
    {
        if(VisualizationGlobal.CurrentCanvasGraph is not null)
        {
            var canvas = VisualizationGlobal.CurrentCanvasGraph.DrawCanvas();
            GraphModel.Canvas = canvas;
        }
        VisualizationGlobal.CurrentPartial = Models.Partial.Graph;
    }
}