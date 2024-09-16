using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;
using MediatR;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HandlerModels.Commands;

namespace ElectroDetali.Pages.Goods
{
    public class DetailsModel : Models.HelperModels.Page
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DetailsModel(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public Good Good { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var result = await _mediator.Send(new GetGoodQueryModel
                {
                    Id = id
                });

                if (result.ErrorMessage != null)
                {
                    return NotFound();
                }
                else
                {
                    Good = result.Value[0];
                }

                ViewData["Review"] = Good.Reviews;
                ViewData["GoodId"] = Good.Id;
                var picks = await _mediator.Send(new GetPickQueryModel());
                ViewData["Pick"] = picks.Value;

                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товара:\r\n" + ex.Message;
                return Redirect("./Details");
            }
            
        }

        public async Task<IActionResult> OnPostAddReviewAsync(IFormCollection form)
        {
            try
            {
                var id = form["id"];
                var text = form["review"];

                if (Goods.IndexModel.currentUser == null)
                {

                    var result = await _mediator.Send(new CreateUserCommandModel
                    {
                        Name = _httpContextAccessor.HttpContext.Session.Id
                    });

                    if (result.ErrorMessage != null) {
                        StatusMessage = result.ErrorMessage;
                    }

                    Goods.IndexModel.currentUser = result.Value;
                }

                var resutl = await _mediator.Send( new CreateReviewCommandModel
                {
                    GoodId = Convert.ToInt32(id),
                    Text = text,
                    UserId = Goods.IndexModel.currentUser.Id
                });

                if (resutl.ErrorMessage != null)
                {
                    StatusMessage = resutl.ErrorMessage;
                }

                return Redirect($"/Goods/Details?id={id}");
            }
            catch (Exception ex)
            {
                var id = form["id"];
                StatusMessage = "Ошибка при добавлении отзыва:\r\n" + ex.Message;
                return Redirect($"/Goods/Details?id={id}");
            }
        }

        public async Task<IActionResult> OnPostCreateBuy(IFormCollection form)
        {
            try
            {
                var id = form["id"];
                var pick = form["selector"];
                    
                var piksRes = await _mediator.Send(new GetPickQueryModel
                {
                    Id = Convert.ToInt32(pick)
                });
                var dateAdd = piksRes.Value[0];

                if (Goods.IndexModel.currentUser == null)
                {
                    var result = await _mediator.Send(new CreateUserCommandModel
                    {
                        Name = _httpContextAccessor.HttpContext.Session.Id
                    });

                    if (result.ErrorMessage != null)
                    {
                        StatusMessage = result.ErrorMessage;
                    }

                    Goods.IndexModel.currentUser = result.Value;
                }

                var buy = await _mediator.Send(new CreateBusketRowCommandModel
                {
                    Goodid = Convert.ToInt32(id),
                    Pointid = Convert.ToInt32(pick),
                    Datedelivery = DateTime.Now.Date.AddDays((double)dateAdd.Time),
                    Datecreate = DateTime.Now.Date,
                    Userid = Goods.IndexModel.currentUser.Id,
                });

                if (buy.ErrorMessage != null)
                {
                    StatusMessage = buy.ErrorMessage;
                }

                return Redirect($"/Goods/Details?id={id}");
            }
            catch (Exception ex)
            {
                var id = form["id"];
                StatusMessage = "Ошибка при добавлении в корзину:\r\n" + ex.Message;
                return Redirect($"/Goods/Details?id={id}");
            }
            
        }
    }

}
