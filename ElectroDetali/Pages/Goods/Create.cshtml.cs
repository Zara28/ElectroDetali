using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElectroDetali.Models;

namespace ElectroDetali.Pages.Goods
{
    public class CreateModel : PageModel
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public CreateModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["Categoryid"] = new SelectList(_context.Categories, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Good Good { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Goods == null || Good == null)
            {
                return Page();
            }

            _context.Goods.Add(Good);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
