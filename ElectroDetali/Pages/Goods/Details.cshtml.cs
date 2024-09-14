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
    public class DetailsModel : PageModel
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public DetailsModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        public Good Good { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }

            var good = await _context.Goods.Include(g => g.Reviews).
                Include(g => g.Category).
                FirstOrDefaultAsync(m => m.Id == id);
            if (good == null)
            {
                return NotFound();
            }
            else 
            {
                Good = good;
            }

            ViewData["Review"] = Good.Reviews;
            ViewData["GoodId"] = Good.Id;

            return Page();
        }

        public IActionResult OnPost(IFormCollection form)
        {
            var id = form["id"];
            var text = form["review"];
            var review = new Review
            {
                Goodid = Convert.ToInt32(id),
                Value = text
            };
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return Redirect($"/Goods/Details?id={id}");
        }
    }

}
