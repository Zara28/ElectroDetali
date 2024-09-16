using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using NuGet.Protocol.Plugins;

namespace ElectroDetali.Handlers.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandModel, ResponseModel<User>>
    {

        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<User>> Handle(CreateUserCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password
                };

                var newUser = await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                return new()
                {
                    Value = newUser.Entity
                };
            }
            catch (Exception ex) {
                _logger.LogError("Произошла ошибка при создании пользователя " + ex, ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
