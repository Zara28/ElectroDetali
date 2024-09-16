using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetGoodAndReviewQueryModel : BaseQueryModel, IRequest<ResponseModel<Good>>
    {
    }
}
