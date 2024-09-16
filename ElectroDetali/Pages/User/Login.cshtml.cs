using ElectroDetali.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Pages.User
{
    public class LoginModel : Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        public LoginModel(ElectroDetaliContext context)
        {
            _context = context;
        }
        public IActionResult OnPost(IFormCollection form)
        {
            try
            {
                var login = form["InputEmail"].ToString();
                var password = form["InputPassword"].ToString();

                var user = _context.Users.First(u => u.Email == login && u.Password == password);

                Goods.IndexModel.currentUser = user;
                return Redirect("/Goods/Index");
            }
            catch (Exception ex)
            {
                StatusMessage = "Пользователен не найден";
                return Page();
            }
        }
    }
}
