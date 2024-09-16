using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Protocol.Plugins;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Xml.Linq;

namespace ElectroDetali.Pages.User
{
    public class BusketModel : Page
    {
        private readonly ElectroDetali.Models.ElectroDetaliContext _context;
        private readonly SmtpClient smtpClient;
        public BusketModel(Models.ElectroDetaliContext context)
        {
            _context = context;
            smtpClient = new SmtpClient();
        }

        public List<Models.Buy> busket { get; set; }
        public void OnGet()
        {
            try
            {
                if (Goods.IndexModel.currentUser != null)
                {
                    busket = _context.Buys.Where(b => b.Userid == Goods.IndexModel.currentUser.Id
                                            && b.Isbasket == true)
                                       .Include(b => b.Good)
                                       .Include(b => b.User)
                                       .Include(b => b.Point).ToList();
                    ViewData["Buskets"] = busket;
                    ViewData["Sum"] = busket.Sum(b => b.Good.Price);
                }
                else
                {
                    busket = null;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при получении корзины:\r\n" + ex.Message;
            }
        }

        public IActionResult OnPostDelete(IFormCollection form)
        {
            try
            {
                var id = Convert.ToInt32(form["Id"]);
                var obj = _context.Buys.FirstOrDefault(b => b.Id == id);
                _context.Buys.Remove(obj);
                _context.SaveChanges();
                return Redirect("/User/Busket");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при удалении товара:\r\n" + ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCreateBuyAsync(IFormCollection form)
        {
            try
            {
                var mail = form["mail"].ToString();
                var obj = await _context.Buys.Where(b => b.Userid == Goods.IndexModel.currentUser.Id
                                            && b.Isbasket == true).ToListAsync();

                foreach (var item in obj)
                {
                    item.Datecreate = DateTime.Now;
                    item.Isbasket = false;
                    _context.Buys.Update(item);
                    _context.SaveChanges();
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ElectroDetali", "spam@goldev.org"));
                message.To.Add(new MailboxAddress("", mail));
                message.Subject = "Заказ на сайте ElectroDetali";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Ваш заказ успешно оформлен. Отслеживайте в личном кабинете состояние доставки";
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("mail.goldev.org", 587, false);
                    client.Authenticate("spam@goldev.org", "gavri1lA123");
                    client.Send(message);
                    client.Disconnect(true);
                }

                busket = null;
                return Redirect("/User/Busket");
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка при оформлении заказа:\r\n" + ex.Message;
                return Page();
            }
        }
    }
}
