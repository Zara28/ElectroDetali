using ElectroDetali.Models;
using ElectroDetali.Models.HelperModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;

namespace ElectroDetali.Pages.User
{
    public class RegisterModel : Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        private readonly SmtpClient smtpClient;
        public RegisterModel(ElectroDetaliContext context)
        {
            _context = context;
            smtpClient = new SmtpClient();
        }

        public IActionResult OnPost(IFormCollection form)
        {
            try
            {
                var login = form["InputEmail"].ToString();
                var password = form["InputPassword"].ToString();
                var name = form["InputName"].ToString();

                var user = _context.Users.FirstOrDefault(u => u.Email == login
                                                        && u.Password == password
                                                        && u.Isapp == true);
                if (user != null)
                {
                    StatusMessage = "Такой пользователь уже зарегистрирован";
                    return Page();
                }
                var Random = new Random();
                var code = Random.Next(100000);

                user = new Models.User
                {
                    Email = login,
                    Password = password,
                    Name = name,
                    Code = code.ToString(),
                    Isapp = false
                };

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ElectroDetali", "spam@goldev.org"));
                message.To.Add(new MailboxAddress(name, login));
                message.Subject = "Код подтверждения";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Спасибо за регистрацию в ElectroDetali!" +
                    "Ваш код подтверждения: " + code;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("mail.goldev.org", 587, false);
                    client.Authenticate("spam@goldev.org", "gavri1lA123");
                    client.Send(message);
                    client.Disconnect(true);
                }

                _context.Users.Add(user);
                _context.SaveChanges();

                return Redirect("User/Confirm");
                
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при регистрации";
                return Page();
            }
        }
    }
}
