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

      public Good Good { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }

            var good = await _context.Goods.Include(g => g.Reviews).FirstOrDefaultAsync(m => m.Id == id);
            if (good == null)
            {
                return NotFound();
            }
            else 
            {
                Good = good;
            }

            ViewData["Review"] = Good.Reviews;

            return Page();
        }

        public void AddReview(IFormCollection form)
        {
            var review = new Review
            {

            };
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }
    }
}
