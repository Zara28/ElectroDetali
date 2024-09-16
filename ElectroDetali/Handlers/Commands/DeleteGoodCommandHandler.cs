using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Commands
{
    public class DeleteGoodCommandHandler : IRequestHandler<DeleteGoodCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public DeleteGoodCommandHandler(ILogger<DeleteGoodCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(DeleteGoodCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var good = await _context.Goods.FirstAsync(g => g.Id == request.Id);

                _context.Goods.Remove(good);
                await _context.SaveChangesAsync();

                return new()
                {
                    Value = "Товар успешно удален"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при удалении товара " + ex, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
