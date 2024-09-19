using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Handlers.Commands
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommandModel, ResponseModel<string>>
    {

        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public CreateReviewCommandHandler(ILogger<CreateReviewCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(CreateReviewCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var review = new Review
                {
                    Goodid = request.GoodId,
                    Userid = request.UserId,
                    Value = request.Text
                };

                await _context.AddAsync(review);

                await _context.SaveChangesAsync();

                return new()
                {
                    Value = "Отзыв успешно добавлен"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при создании отзыва " + ex, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
