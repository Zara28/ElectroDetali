using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MailKit.Net.Smtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;

namespace ElectroDetali.Pages.User
{
    public class RegisterModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;
        public RegisterModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            try
            {
                var login = form["InputEmail"].ToString();
                var password = form["InputPassword"].ToString();
                var name = form["InputName"].ToString();

                var res = await _mediator.Send(new RegisterUserCommandModel
                {
                    Email = login,
                    Name = name,
                    Password = password
                });

                if(res.ErrorMessage == null)
                {
                    return Redirect("./Confirm");
                }

                StatusMessage = res.ErrorMessage;
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при регистрации";
                return Page();
            }
        }
    }
}
