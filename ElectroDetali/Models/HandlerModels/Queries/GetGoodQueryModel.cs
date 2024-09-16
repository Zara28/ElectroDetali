using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetGoodQueryModel : BaseQueryModel, IRequest<ResponseModel<List<Good>>>
    {
    }
}
