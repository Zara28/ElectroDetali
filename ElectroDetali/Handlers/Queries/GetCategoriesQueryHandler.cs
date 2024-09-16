using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace ElectroDetali.Handlers.Queries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQueryModel, ResponseModel<List<Category>>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public GetCategoriesQueryHandler(ILogger<GetCategoriesQueryModel> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<List<Category>>> Handle(GetCategoriesQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                List<Category> categories = new();
                if (request.Id == null)
                {
                    categories = await _context.Categories.ToListAsync();
                }
                else
                {
                     categories.Add(await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id));
                }

                return new ResponseModel<List<Category>>()
                {
                    Value = categories
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при получении категорий "+ex.Message, ex);
                return new ResponseModel<List<Category>>
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
