using GraphAlgorithmsAndVisualization.Models;
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
        GraphModel = new(){ X = "", Y = ""};
    }

    public void OnGet()
    {
        
    }

    public void OnPostGraphCanvasClick()
    {
        Console.WriteLine("{0}|{1}", GraphModel.X, GraphModel.Y);
    }
}