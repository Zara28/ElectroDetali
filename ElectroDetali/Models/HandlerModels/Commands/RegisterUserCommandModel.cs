using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class RegisterUserCommandModel : IRequest<ResponseModel<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
