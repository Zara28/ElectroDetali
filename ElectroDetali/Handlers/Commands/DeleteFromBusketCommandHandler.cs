using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Commands
{
    public class DeleteFromBusketCommandHandler : IRequestHandler<DeleteFormBusketCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public DeleteFromBusketCommandHandler(ILogger<DeleteFromBusketCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(DeleteFormBusketCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var obj = await _context.Buys.FirstOrDefaultAsync(b => b.Id == request.BusketId);
                _context.Buys.Remove(obj);
                await _context.SaveChangesAsync();

                return new ResponseModel<string>()
                {
                    Value = "ТОвар успешно удален из корзины"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при удалении товара из корзины " + ex.Message, ex);
                return new()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
