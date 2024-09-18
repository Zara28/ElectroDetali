using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class CreateBuyCommandModel : IRequest<ResponseModel<string>>
    {
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
