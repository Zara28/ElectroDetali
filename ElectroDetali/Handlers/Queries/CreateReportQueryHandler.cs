using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spire.Xls;

namespace ElectroDetali.Handlers.Queries
{
    public class CreateReportQueryHandler : IRequestHandler<CreateReportQueryModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public CreateReportQueryHandler(ILogger<CreateReportQueryHandler> logger,
                                   IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(CreateReportQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                var tableToUse = new List<(string, string, string, string, string)>();

                var buys = await _mediator.Send(new GetBuysQueryModel()
                {
                    Id = request.Id,
                });

                buys.Value
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

                return new()
                {
                    Value = "Отчет успешно сформирован"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при формировании отчета " + ex.Message, ex);
                return new()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
