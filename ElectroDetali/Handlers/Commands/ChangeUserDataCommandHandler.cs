using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Handlers.Commands
{
    public class ChangeUserDataCommandHandler : IRequestHandler<ChangeUserDataCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;
        private readonly ElectroDetaliContext _context;

        public ChangeUserDataCommandHandler(ILogger<ChangeUserDataCommandHandler> logger,
                                   ElectroDetaliContext context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(ChangeUserDataCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

                user.Name = request.UserName;
                user.Email = request.Email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new()
                {
                    Value = "Данные успешно обновлены"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при обновлении данных " + ex.Message, ex);
                return new()
                {
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
