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
            ViewData["Pick"] = _context.PickUpPoints.ToList();

            return Page();
        }

        public IActionResult OnPostAddReview(IFormCollection form)
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

        public IActionResult OnPostCreateBuy(IFormCollection form)
        {
            var id = form["id"];
            var pick = form["selector"];
            var dateAdd = _context.PickUpPoints.First(f => f.Id == Convert.ToInt32(pick));
            var buy = new Buy
            {
                Goodid=Convert.ToInt32(id),
                Pointid=Convert.ToInt32(pick),
                Datedelivery = DateTime.Now.Date.AddDays((double)dateAdd.Time),
                Datecreate = DateTime.Now.Date,
                Isbasket = true
            };
            _context.Buys.Add(buy);
            _context.SaveChanges();
            return Redirect($"/Goods/Details?id={id}");
        }
    }

}
