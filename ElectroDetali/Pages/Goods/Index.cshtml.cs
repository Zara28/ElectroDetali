using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;

namespace ElectroDetali.Pages.Goods
{
    public class IndexModel : Models.HelperModels.Page
    {
        public static Models.User currentUser;
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(ElectroDetali.Models.ElectroDetaliContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
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
                if (_context.Goods != null)
                {
                    if (Id == null)
                    {
                        goods = await _context.Goods
                            .Include(g => g.Category).ToListAsync();
                    }
                    else goods = await _context.Goods
                    .Include(g => g.Category).Where(c => c.Categoryid == Id).ToListAsync();

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
                }
                ViewData["Category"] = _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товаров:\r\n" + ex.Message;
            }
            
        }
    }
}

