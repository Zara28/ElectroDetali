using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Queries
{
    public class GetGoodAndReviewQueryHandler : IRequestHandler<GetGoodAndReviewQueryModel, ResponseModel<Good>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public GetGoodAndReviewQueryHandler(ILogger<GetGoodAndReviewQueryModel> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<Good>> Handle(GetGoodAndReviewQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                var good = await _context.Goods.Include(g => g.Reviews)
                                                .ThenInclude(r => r.User)
                                                .Include(g => g.Category)
                                                .FirstOrDefaultAsync(g => g.Id == request.Id);
                return new()
                {
                    Value = good
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("При получении товара с отзывами произошла ошибка " + ex.Message, ex);
                return new()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
