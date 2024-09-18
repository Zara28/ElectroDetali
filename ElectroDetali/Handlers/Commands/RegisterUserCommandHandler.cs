using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger, 
            ElectroDetaliContext context,
            IMediator mediator)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        public async Task<ResponseModel<string>> Handle(RegisterUserCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email
                                                        && u.Password == request.Password.GetHash()
                                                        && u.Isapp == true);
               
               if(user != null)
                {
                    return new()
                    {
                        ErrorMessage = "Такой пользователь уже существует"
                    };
                } 

                var Random = new Random();
                var code = Random.Next(100000);

                user = new Models.User
                {
                    Email = request.Email,
                    Password = request.Password.GetHash(),
                    Name = request.Name,
                    Code = code.ToString(),
                    Isapp = false
                };

                _context.Users.Add(user);

                var res = _mediator.Send(new SendMailCommandModel
                {
                    Email = request.Email,
                    Subject = "Подтверждение регистрации на ElectroDetali",
                    Body = "Спасибо за регистрацию! Ваш код подтверждения " + code
                });

                await _context.SaveChangesAsync();

                return new()
                {
                    Value = "На почту было отправлено письмо с кодом подтверждения"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при регистрации", ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
