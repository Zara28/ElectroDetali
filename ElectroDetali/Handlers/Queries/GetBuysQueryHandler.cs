using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Queries
{
    public class GetBuysQueryHandler : IRequestHandler<GetBuysQueryModel, ResponseModel<List<Buy>>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public GetBuysQueryHandler(ILogger<GetBuysQueryHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<List<Buy>>> Handle(GetBuysQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                var Buy = await _context.Buys
                .Include(b => b.Good)
                .Include(b => b.Point)
                .Include(b => b.User)
                .Where(b => b.Userid == request.Id &&
                            b.Isbasket == false)
                .OrderBy(b => b.Datecreate).ToListAsync();

                return new ResponseModel<List<Buy>>()
                {
                    Value = Buy,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при получении истории заказов");
                return new()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
