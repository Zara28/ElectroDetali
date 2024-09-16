using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;
using MediatR;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HandlerModels.Commands;

namespace ElectroDetali.Pages.Goods
{
    public class CreateModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var categories = await _mediator.Send(new GetCategoriesQueryModel());
                ViewData["Categoryid"] = new SelectList(categories.Value, "Id", "Name");
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении категорий:\r\n" + ex.Message;
                return Page();
            }
        }

        [BindProperty]
        public Good Good { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || Good == null)
                {
                    return Page();
                }

                var result = await _mediator.Send(new CreateGoodCommandModel
                {
                    Name = Good.Name,
                    Description = Good.Description,
                    Price = Good.Price,
                    CategoryId = (int)Good.Categoryid,
                    Image = Good.Image
                });

                if(!String.IsNullOrEmpty(result.ErrorMessage))
                {
                    StatusMessage = "Ошибка при добавлении товара:\r\n";
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при добавлении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }
    }
}
