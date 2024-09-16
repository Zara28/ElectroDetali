using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;

namespace ElectroDetali.Pages.Goods
{
    public class EditModel : Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public EditModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Good Good { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
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
                Good = good;
                ViewData["Categoryid"] = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                _context.Attach(Good).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodExists(Good.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при обновлении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }

        private bool GoodExists(int id)
        {
          return (_context.Goods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
