using ElectroDetali.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Pages.User
{
    public class ConformModel : PageModel
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        public ConformModel(ElectroDetaliContext context)
        {
            _context = context;
        }
        public IActionResult OnPost(IFormCollection form)
        {
            var code = form["Input"].ToString();

            var user = _context.Users.FirstOrDefault(u => u.Code == code);
            if (user == null)
            {
                Redirect("Error");
            }
            Goods.IndexModel.currentUser = user;
            return Redirect("/Goods/Index");
        }
    }
}
