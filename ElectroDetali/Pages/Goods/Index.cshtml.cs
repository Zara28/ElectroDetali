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
using NuGet.Versioning;
using ElectroDetali.Models.HandlerModels.Queries;

namespace ElectroDetali.Pages.Goods
{
    public class IndexModel : Models.HelperModels.Page
    {
        public static Models.User currentUser;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public PaginatedList<Good> Good { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SortingValue { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 15;


        public async Task OnGetAsync(int? Id, int? pageIndex)
        {
            try
            {
                var session = _httpContextAccessor.HttpContext.Session;
                var goods = new List<Good>();
                var random = new Random(1000000);
                if (session == null)
                {
                    session.SetString("session", random.Next().ToString());
                }

                var model = new GetGoodQueryModel();
                if (Id != null)
                {
                    model.CategoryId = Id;
                }

                var result = await _mediator.Send(model);

                goods = result.Value;

                if (!String.IsNullOrEmpty(SearchString))
                {
                    goods = goods.Where(g => g.Name.ToLower().Contains(SearchString.ToLower())).ToList();
                }
                else
                {
                    pageIndex = 1;
                }
                if (!String.IsNullOrEmpty(SortingValue))
                {
                    goods = SortingValue switch
                    {
                        "ASC" => goods.OrderBy(g => g.Price).ToList(),
                        "DESC" => goods.OrderByDescending(g => g.Price).ToList(),
                        _ => goods
                    };
                }

                Good = PaginatedList<Good>.CreateAsync(
                goods.ToList(), pageIndex ?? 1, PageSize);

                var resCat = await _mediator.Send(new GetCategoriesQueryModel());

                ViewData["Category"] = resCat.Value;
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товаров:\r\n" + ex.Message;
            }
            
        }
    }
}

