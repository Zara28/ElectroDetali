using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Queries
{
    public class GetGoodQueryHandler : IRequestHandler<GetGoodQueryModel, ResponseModel<List<Good>>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public GetGoodQueryHandler(ILogger<GetGoodQueryHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseModel<List<Good>>> Handle(GetGoodQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                List<Good> goods = new();
                if (request.Id == null)
                {
                    goods = await _context.Goods.Include(g => g.Category).ToListAsync();
                }
                else
                {
                    goods.Add(await _context.Goods.FirstAsync(g => g.Id == request.Id));
                }

                if(request.CategoryId != null)
                {
                    goods = goods.Where(g => g.Categoryid == request.CategoryId).ToList();
                }

                return new()
                {
                    Value = goods
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при получении товаров:"+ex.Message, ex);
                return new ResponseModel<List<Good>>()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
