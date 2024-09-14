using ElectroDetali.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        public LoginModel(ElectroDetaliContext context)
        {
            _context = context;
        }
        public IActionResult OnPost(IFormCollection form)
        {
            var login = form["InputEmail"].ToString();
            var password = form["InputPassword"].ToString();

            var user = _context.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
            if (user == null)
            {
                Redirect("Error");
            }

            Goods.IndexModel.currentUser = user;
            return Redirect("/Goods/Index");
        }
    }
}
