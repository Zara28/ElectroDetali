using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using NuGet.Protocol.Plugins;

namespace ElectroDetali.Handlers.Commands
{
    public class CreateBusketCommandHandler : IRequestHandler<CreateBusketRowCommandModel, ResponseModel<string>>
    {

        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public CreateBusketCommandHandler(ILogger<CreateBusketCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(CreateBusketRowCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var busket = new Buy
                {
                    Goodid = request.Goodid,
                    Userid = request.Userid,
                    Pointid = request.Pointid,
                    Datecreate = request.Datecreate,
                    Datedelivery = request.Datedelivery,
                    Isbasket = true
                };

                await _context.Buys.AddAsync(busket);
                await _context.SaveChangesAsync();

                return new()
                {
                    Value = "Товар успешно добавлен в корзину"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при создании записи корзины " + ex, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
