using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using Spire.Xls;
using ElectroDetali.Models.HelperModels;
using MediatR;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HandlerModels.Commands;

namespace ElectroDetali.Pages.User
{
    public class CabinetModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;

        public CabinetModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IList<Buy> Buy { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                var res = await _mediator.Send(new GetBuysQueryModel
                {
                    Id = Goods.IndexModel.currentUser.Id
                });

                Buy = res.Value;
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении заказов:\r\n" + ex.Message;
            }

        }

        public IActionResult OnPostChange(IFormCollection form)
        {
            try
            {
                var name = form["Name"].ToString();
                var email = form["Login"].ToString();

                Goods.IndexModel.currentUser.Name = name;
                Goods.IndexModel.currentUser.Email = email;

                var res = _mediator.Send(new ChangeUserDataCommandModel()
                {
                    UserId = Goods.IndexModel.currentUser.Id,
                    UserName = name,
                    Email = email,
                });

                return Redirect("/User/Cabinet");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при обновлении давнных:\r\n" + ex.Message;
                return Page();
            }
        }

        public async Task<FileContentResult> OnPostSave()
        {
            try
            {
                var result = await _mediator.Send(new CreateReportQueryModel
                {
                    Id = Goods.IndexModel.currentUser.Id
                });

                if(result.ErrorMessage != null)
                {
                    return File(System.IO.File.ReadAllBytes($"Report_{DateTime.Now.Date.ToShortDateString()}.xls"), "application/octet-stream", $"Report_{DateTime.Now.Date.ToShortDateString()}.xls");

                }
                else
                {
                    StatusMessage = result.ErrorMessage;
                    return null;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при формировании отчета:\r\n" + ex.Message;
                return null;
            }
        }
    }
}
