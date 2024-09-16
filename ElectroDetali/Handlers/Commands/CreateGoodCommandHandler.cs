using ElectroDetali.Handlers.Queries;
using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using NuGet.Protocol.Plugins;

namespace ElectroDetali.Handlers.Commands
{
    public class CreateGoodCommandHandler : IRequestHandler<CreateGoodCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public CreateGoodCommandHandler(ILogger<CreateGoodCommandModel> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(CreateGoodCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var good = new Good
                {
                    Name = request.Name,
                    Description = request.Description,
                    Categoryid = request.CategoryId,
                    Price = request.Price,
                    Image = request.Image,
                };

                await _context.Goods.AddAsync(good);

                await _context.SaveChangesAsync();

                return new()
                {
                    Value = "Товар успешно добавлен"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при создании товара " + ex, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
