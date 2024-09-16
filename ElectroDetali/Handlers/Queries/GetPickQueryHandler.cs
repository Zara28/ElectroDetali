using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Queries
{
    public class GetPickQueryHandler : IRequestHandler<GetPickQueryModel, ResponseModel<List<PickUpPoint>>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public GetPickQueryHandler(ILogger<GetPickQueryModel> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<List<PickUpPoint>>> Handle(GetPickQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                List<PickUpPoint> points = await _context.PickUpPoints.ToListAsync();
                return new()
                {
                    Value = points
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("При получении пунктов выдачи произошла ошибка " + ex.Message, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
