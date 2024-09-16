using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Models.HelperModels
{
    public class Page : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}
