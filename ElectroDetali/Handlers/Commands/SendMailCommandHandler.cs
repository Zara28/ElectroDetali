using ElectroDetali.Models;
using ElectroDetali.Models.HandlerModels.Commands;
using ElectroDetali.Models.HelperModels;
using MailKit.Net.Smtp;
using MediatR;
using MimeKit;

namespace ElectroDetali.Handlers.Commands
{
    public class SendMailCommandHandler : IRequestHandler<SendMailCommandModel, ResponseModel<string>>
    {
        private readonly ILogger _logger;

        public SendMailCommandHandler(ILogger<UpdateGoodCommandHandler> logger)
        {
            _logger = logger;
        }
        public async Task<ResponseModel<string>> Handle(SendMailCommandModel request, CancellationToken cancellationToken)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ElectroDetali", VariablesStorage.GetVariable("SMTP_MAIL")));
                message.To.Add(new MailboxAddress("", request.Email));
                message.Subject = request.Subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = request.Body;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(VariablesStorage.GetVariable("SMTP_ADRESS"), 587, false);
                    client.Authenticate(VariablesStorage.GetVariable("SMTP_MAIL"), VariablesStorage.GetVariable("SMTP_PASSWORD"));
                    client.Send(message);
                    client.Disconnect(true);
                }
                return new()
                {
                    Value = "Письмо успешно отправлено"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Произошла ошибка при отправке " + ex.Message, ex);
                return new ResponseModel<string>()
                {
                    ErrorMessage = ex.Message,
                };
            }
            ;
        }
    }
}
