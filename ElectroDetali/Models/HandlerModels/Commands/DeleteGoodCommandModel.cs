using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class DeleteGoodCommandModel : IRequest<ResponseModel<string>>
    {
        public int Id { get; set; }
    }
}
