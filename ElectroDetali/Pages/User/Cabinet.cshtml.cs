using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElectroDetali.Models;
using Spire.Pdf;
using Spire.Pdf.Tables;
using System.Drawing;

namespace ElectroDetali.Pages.User
{
    public class CabinetModel : PageModel
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;

        public CabinetModel(ElectroDetali.Models.ElectroDetaliContext context)
        {
            _context = context;
        }

        public IList<Buy> Buy { get;set; } = default!;

        public async Task OnGetAsync()
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

        public FileContentResult OnPostSave()
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
            PdfDocument doc = new PdfDocument();
            PdfTable table = new PdfTable();

            table.Columns.Add(new PdfColumn()
            {
                ColumnName = "1"
            });
            table.Columns.Add(new PdfColumn("2"));
            table.Columns.Add(new PdfColumn("3"));
            table.Columns.Add(new PdfColumn("4"));
            table.Columns.Add(new PdfColumn("5"));
            table.DataSource = tableToUse;
            PdfPageBase tocPage = doc.Pages.Insert(0);
            table.Draw(tocPage, new PointF(0, 30));
            doc.SaveToFile("Report.pdf");


            return File(System.IO.File.ReadAllBytes("Report.pdf"), "application/octet-stream", "Report.pdf");
        }

    }
}
