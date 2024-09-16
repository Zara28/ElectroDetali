using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class GetGoodQueryModel : BaseQueryModel, IRequest<ResponseModel<List<Good>>>
    {
        public int? CategoryId { get; set; } = null;
    }
}
