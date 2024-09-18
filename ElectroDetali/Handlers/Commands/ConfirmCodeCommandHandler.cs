using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Handlers.Commands
{
    public class ConfirmCodeCommandHandler : IRequestHandler<ConfirmCodeCommandModel, ResponseModel<User>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public ConfirmCodeCommandHandler(ILogger<ConfirmCodeCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<User>> Handle(ConfirmCodeCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _context.Users.First(u => u.Code == request.Code);
                if(user == null)
                {
                    return new ResponseModel<User>()
                    {
                        ErrorMessage = "Код не найден"
                    };
                }

                user.Isapp = true;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new ResponseModel<User>()
                {
                    Value = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при подтверждении ", ex);
                return new ResponseModel<User>()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
