using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;

namespace ElectroDetali.Pages.Goods
{
    public class CreateModel : Models.HelperModels.Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public CreateModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            try
            {
                ViewData["Categoryid"] = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении категорий:\r\n" + ex.Message;
                return Page();
            }
        }

        [BindProperty]
        public Good Good { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || _context.Goods == null || Good == null)
                {
                    return Page();
                }

                _context.Goods.Add(Good);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при добавлении товара:\r\n" + ex.Message;
                return Page();
            }
            
        }
    }
}
