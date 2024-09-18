using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetBusketQueryModel : BaseQueryModel, IRequest<ResponseModel<List<Buy>>>
    {
    }
}
