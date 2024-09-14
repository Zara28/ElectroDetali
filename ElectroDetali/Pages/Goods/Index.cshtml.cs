using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;

namespace ElectroDetali.Pages.Goods
{
    public class IndexModel : PageModel
    {
        public static Models.User currentUser;
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(ElectroDetali.Models.ElectroDetaliContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<Good> Good { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync(int? id)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var random = new Random(1000000);
            if (session == null)
            {
                session.SetString("session", random.Next().ToString());
            }
            if (_context.Goods != null)
            {
                if(id == null)
                {
                    Good = await _context.Goods
                        .Include(g => g.Category).ToListAsync();
                }
                else Good = await _context.Goods
                .Include(g => g.Category).Where(c => c.Categoryid == id).ToListAsync();

                if(!String.IsNullOrEmpty(SearchString))
                {
                    Good = Good.Where(g => g.Name.ToLower().Contains(SearchString.ToLower())).ToList();
                }
            }
            ViewData["Category"] = _context.Categories.ToList();
        }
    }
}

