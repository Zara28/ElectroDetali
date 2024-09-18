using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class ConfirmCodeCommandModel : IRequest<ResponseModel<User>>
    {
        public string Code { get; set; }
    }
}
