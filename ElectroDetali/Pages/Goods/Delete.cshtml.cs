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
    public class DeleteModel : PageModel
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public DeleteModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Good Good { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }

            var good = await _context.Goods.FirstOrDefaultAsync(m => m.Id == id);

            if (good == null)
            {
                return NotFound();
            }
            else 
            {
                Good = good;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }
            var good = await _context.Goods.FindAsync(id);

            if (good != null)
            {
                Good = good;
                _context.Goods.Remove(Good);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
