using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Queries
{
    public class GetBusketQueryHandler : IRequestHandler<GetBusketQueryModel, ResponseModel<List<Buy>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public GetBusketQueryHandler(ILogger<GetBusketQueryHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<List<Buy>>> Handle(GetBusketQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                var busket = _context.Buys.Where(b => b.Userid == request.Id
                                            && b.Isbasket == true)
                                       .Include(b => b.Good)
                                       .Include(b => b.User)
                                       .Include(b => b.Point).ToList();
                return new()
                {
                    Value = busket
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при получении коризны "+ex.Message, ex);
                return new ResponseModel<List<Buy>>()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
