using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class CreateReviewCommandModel : IRequest<ResponseModel<string>>
    {
        public int UserId { get; set; }
        public int GoodId { get; set; }
        public string Text { get; set; }
    }
}
