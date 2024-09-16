using ElectroDetali.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Pages.User
{
    public class ConformModel : Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        public ConformModel(ElectroDetaliContext context)
        {
            _context = context;
        }
        public IActionResult OnPost(IFormCollection form)
        {
            try
            {
                var code = form["Input"].ToString();

                var user = _context.Users.First(u => u.Code == code);
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
