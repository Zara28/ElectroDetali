using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetBuysQueryModel : BaseQueryModel, IRequest<ResponseModel<List<Buy>>>
    {
    }
}
