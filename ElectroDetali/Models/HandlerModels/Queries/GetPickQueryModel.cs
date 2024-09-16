using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetPickQueryModel : BaseQueryModel, IRequest<ResponseModel<List<PickUpPoint>>>
    {
    }
}
