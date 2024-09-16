using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using Spire.Xls;
using ElectroDetali.Models.HelperModels;

namespace ElectroDetali.Pages.User
{
    public class CabinetModel : Models.HelperModels.Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public CabinetModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        public IList<Buy> Buy { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                if (_context.Buys != null)
                {
                    Buy = await _context.Buys
                    .Include(b => b.Good)
                    .Include(b => b.Point)
                    .Include(b => b.User)
                    .Where(b => b.Userid == Goods.IndexModel.currentUser.Id &&
                                b.Isbasket == false)
                    .OrderBy(b => b.Datecreate).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении заказов:\r\n" + ex.Message;
            }

        }

        public IActionResult OnPostChange(IFormCollection form)
        {
            try
            {
                var name = form["Name"].ToString();
                var email = form["Login"].ToString();

                Goods.IndexModel.currentUser.Name = name;
                Goods.IndexModel.currentUser.Email = email;

                _context.Users.Update(Goods.IndexModel.currentUser);
                _context.SaveChanges();
                return Redirect("/User/Cabinet");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при обновлении давнных:\r\n" + ex.Message;
                return Page();
            }
        }

        public FileContentResult OnPostSave()
        {
            try
            {
                var tableToUse = new List<(string, string, string, string, string)>();
                _context.Buys
                    .Include(b => b.Good)
                    .Include(b => b.Point)
                    .Include(b => b.User)
                    .Where(b => b.Userid == Goods.IndexModel.currentUser.Id &&
                                b.Isbasket == false)
                    .OrderBy(b => b.Datecreate).ToList()
                    .ForEach(b =>
                    {
                        tableToUse.Add((b.Datecreate.Value.ToShortDateString(),
                                        b.Datedelivery > DateTime.Now ?
                                        "Ожидается доставка: " + b.Datedelivery.Value.ToShortDateString() :
                                        "Доставлен",
                                        b.Good.Name,
                                        b.Good.Price.ToString(),
                                        b.Point.Adress));
                    });
                Workbook book = new Workbook();
                Worksheet sheet = book.Worksheets[0];

                sheet.Range[$"A1"].Text = "Дата заказа";
                sheet.Range[$"B1"].Text = "Статус заказа";
                sheet.Range[$"C1"].Text = "Товар";
                sheet.Range[$"D1"].Text = "Стоимость в рублях";
                sheet.Range[$"E1"].Text = "Пункт выдачи";

                for (int i = 2; i <= tableToUse.Count + 1; i++)
                {
                    sheet.Range[$"A{i}"].Text = tableToUse[i - 2].Item1;
                    sheet.Range[$"B{i}"].Text = tableToUse[i - 2].Item2;
                    sheet.Range[$"C{i}"].Text = tableToUse[i - 2].Item3;
                    sheet.Range[$"D{i}"].Text = tableToUse[i - 2].Item4;
                    sheet.Range[$"E{i}"].Text = tableToUse[i - 2].Item5;
                }

                book.SaveToFile($"Report_{DateTime.Now.Date.ToShortDateString()}.xls");

                return File(System.IO.File.ReadAllBytes($"Report_{DateTime.Now.Date.ToShortDateString()}.xls"), "application/octet-stream", $"Report_{DateTime.Now.Date.ToShortDateString()}.xls");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при формировании отчета:\r\n" + ex.Message;
                return null;
            }
        }
    }
}
