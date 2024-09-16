using Microsoft.AspNetCore.Mvc;
using ElectroDetali.Models;
using MediatR;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HandlerModels.Commands;

namespace ElectroDetali.Pages.Goods
{
    public class DeleteModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;

        public DeleteModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Good Good { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var good = await _mediator.Send(new GetGoodQueryModel
                {
                    Id = id
                });

                if (good.Value == null || good.ErrorMessage != null)
                {
                    return NotFound();
                }
                else
                {
                    Good = good.Value[0];
                }
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var result = await _mediator.Send(new DeleteGoodCommandModel
                {
                    Id = (int)id
                });

                if (!String.IsNullOrEmpty(result.ErrorMessage))
                {
                    StatusMessage = result.ErrorMessage;
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при удалении товара:\r\n"+ex.Message;
                return Page();
            }
            
        }
    }
}
