using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;
using MediatR;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Handlers.Queries;
using ElectroDetali.Models.HandlerModels.Commands;

namespace ElectroDetali.Pages.Goods
{
    public class EditModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;

        public EditModel(IMediator mediator)
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

                var result = await _mediator.Send(new GetGoodQueryModel()
                {
                    Id = id
                });

                var good = result.Value[0];
                if (good == null)
                {
                    StatusMessage = result.ErrorMessage;
                    return NotFound();
                }
                Good = good;

                var categories = await _mediator.Send(new GetCategoriesQueryModel());

                ViewData["Categoryid"] = new SelectList(categories.Value, "Id", "Name");
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var result = await _mediator.Send(new UpdateGoodCommandModel
                {
                    Id = Good.Id,
                    Name = Good.Name,
                    Description = Good.Description,
                    CategoryId = (int)Good.Categoryid,
                    Price = Good.Price,
                    Image = Good.Image,
                });


                if(result.ErrorMessage != null)
                {
                    StatusMessage = result.ErrorMessage;
                }
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при обновлении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }
    }
}
