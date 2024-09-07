using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GraphAlgorithmsAndVisualization.Pages.GraphVisualization;

public class IndexModel : PageModel
{

    #region AspForFields
    [BindProperty]
    public string X { get; set; }
    [BindProperty]
    public string Y { get; set; }
    #endregion
    public IndexModel()
    {
    }

    public void OnGet()
    {

    }

    public void OnPostGraphCanvasClick()
    {
        Console.WriteLine("{0}|{1}", X, Y);
    }
}