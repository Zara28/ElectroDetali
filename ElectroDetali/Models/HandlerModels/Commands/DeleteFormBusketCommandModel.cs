using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class DeleteFormBusketCommandModel : IRequest<ResponseModel<string>>
    {
        public int BusketId { get; set; }
    }
}
