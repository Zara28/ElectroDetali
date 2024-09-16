using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetCategoriesQueryModel : BaseQueryModel, IRequest<ResponseModel<List<Category>>>
    {
    }
}
