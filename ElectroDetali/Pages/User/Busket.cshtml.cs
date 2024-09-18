using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Protocol.Plugins;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Xml.Linq;
using ElectroDetali.Models.HelperModels;
using MediatR;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HandlerModels.Commands;

namespace ElectroDetali.Pages.User
{
    public class BusketModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;
        private readonly SmtpClient smtpClient;
        public BusketModel(IMediator mediator)
        {
            _mediator = mediator;
            smtpClient = new SmtpClient();
        }

        public List<Models.Buy> busket { get; set; }
        public async Task OnGetAsync()
        {
            try
            {
                if (Goods.IndexModel.currentUser != null)
                {
                    var result = await _mediator.Send(new GetBusketQueryModel
                    {
                        Id = Goods.IndexModel.currentUser.Id,
                    });
                    if(result.ErrorMessage == null)
                    {
                        busket = result.Value.ToList();
                        ViewData["Buskets"] = busket;
                        ViewData["Sum"] = busket.Sum(b => b.Good.Price);
                    }
                    else StatusMessage = result.ErrorMessage;
                   
                }
                else
                {
                    busket = null;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении корзины:\r\n" + ex.Message;
            }
        }

        public async Task<IActionResult> OnPostDelete(IFormCollection form)
        {
            try
            {
                var id = Convert.ToInt32(form["Id"]);

                var result = await _mediator.Send(new DeleteFormBusketCommandModel()
                {
                    BusketId = id,
                });

                if(result.ErrorMessage != null)
                {
                    StatusMessage = result.ErrorMessage;
                }

                return Redirect("/User/Busket");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при удалении товара:\r\n" + ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCreateBuyAsync(IFormCollection form)
        {
            try
            {
                var mail = form["mail"].ToString();

                var result = await _mediator.Send(new CreateBuyCommandModel()
                {
                    Email = mail ?? Goods.IndexModel.currentUser.Email,
                    UserId = Goods.IndexModel.currentUser.Id
                });

                busket = null;
                return Redirect("/User/Busket");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при оформлении заказа:\r\n" + ex.Message;
                return Page();
            }
        }
    }
}
