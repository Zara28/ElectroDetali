using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class LoginUserQueryModel : IRequest<ResponseModel<User>>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
