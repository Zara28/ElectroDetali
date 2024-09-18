using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectroDetali.Pages.User
{
    public class ConformModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;
        public ConformModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> OnPost(IFormCollection form)
        {
            try
            {
                var code = form["Input"].ToString();

                var result = await _mediator.Send(new ConfirmCodeCommandModel
                {
                    Code = code,
                });

                if(result.ErrorMessage == null)
                {
                    Goods.IndexModel.currentUser = result.Value;
                    return Redirect("/Goods/Index");
                }
                else
                {
                    StatusMessage = result.ErrorMessage;
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
