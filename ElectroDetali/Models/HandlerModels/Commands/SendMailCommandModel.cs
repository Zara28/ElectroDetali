using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class SendMailCommandModel : IRequest<ResponseModel<string>>
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
