using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Queries;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Queries
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQueryModel, ResponseModel<User>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public LoginUserQueryHandler(ILogger<LoginUserQueryHandler> logger, ElectroDetaliContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ResponseModel<User>> Handle(LoginUserQueryModel request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstAsync(u => u.Email == request.Login && u.Password == request.Password.GetHash());

                return new()
                {
                    Value = user,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при входе", ex);
                return new()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
