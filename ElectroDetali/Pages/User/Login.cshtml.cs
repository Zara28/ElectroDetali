using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Pages.User
{
    public class LoginModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;
        public LoginModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            try
            {
                var login = form["InputEmail"].ToString();
                var password = form["InputPassword"].ToString();

                var res = await _mediator.Send(new LoginUserQueryModel
                {
                    Login = login,
                    Password = password,
                });

                if(res.ErrorMessage == null)
                {
                    Goods.IndexModel.currentUser = res.Value;
                    return Redirect("/Goods/Index");
                }
                else
                {
                    StatusMessage = res.ErrorMessage;
                    return Page();
                }
                
            }
            catch (Exception ex)
            {
                StatusMessage = "Пользователен не найден";
                return Page();
            }
        }
    }
}
