using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Handlers.Commands
{
    public class CreateBuyCommandHandler : IRequestHandler<CreateBuyCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;
        private readonly IMediator _mediator;

        public CreateBuyCommandHandler(ILogger<CreateBuyCommandHandler> logger,
                                   ElectroDetaliContext context, IMediator mediator)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<ResponseModel<string>> Handle(CreateBuyCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var obj = await _mediator.Send(new GetBusketQueryModel
                {
                    Id = request.UserId,
                });

                foreach (var item in obj.Value)
                {
                    item.Datecreate = DateTime.Now;
                    item.Isbasket = false;
                    _context.Buys.Update(item);
                    _context.SaveChanges();
                }

                var result = await _mediator.Send(new SendMailCommandModel()
                {
                    Email = request.Email,
                    Subject = "Заказ на ElectroDetaili",
                    Body = "Спасибо за ваш заказ! Следите за статусом доставки в личном кабинете на нашем сайте"
                });

                return new ResponseModel<string>()
                {
                    Value = "Заказ успешно создан"
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при создании заказа " + ex.Message, ex);
                return new()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
