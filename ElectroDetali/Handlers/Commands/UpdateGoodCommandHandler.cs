using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace ElectroDetali.Handlers.Commands
{
    public class UpdateGoodCommandHandler : IRequestHandler<UpdateGoodCommandModel, ResponseModel<string>>
    {

        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public UpdateGoodCommandHandler(ILogger<UpdateGoodCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(UpdateGoodCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var good = new Good()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Image = request.Image,
                    Categoryid = request.CategoryId
                };

                _context.Attach(good).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return new ResponseModel<string>
                    {
                        Value = "Товар успешно обновлен"
                    };
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodExists(good.Id))
                    {
                        return new ResponseModel<string>()
                        {
                            ErrorMessage = "Товар не найден"
                        };
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при обновлении товара " + ex, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
        private bool GoodExists(int id)
        {
            return (_context.Goods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
