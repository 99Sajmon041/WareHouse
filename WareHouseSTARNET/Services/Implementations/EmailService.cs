using System.Net;
using System.Net.Mail;
using System.Text;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.ViewModels.DashboardViewModels;

namespace WareHouseSTARNET.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendCriticalStockEmailAsync(List<CriticalMaterialViewModel> criticalMaterials)
        {
            string to = "simon.durak@starnet.cz";
            string subject = "Objednání skladových zásob, STARNET, s.r.o.";

            var body = new StringBuilder();
            body.Append("<h3>Seznam kritických skladových zásob:</h3>");
            body.Append("<table border='1' cellpadding='8' cellspacing='0'>");
            body.Append("<thead><tr><th>Název materiálu</th><th>Množství na skladě</th><th>Kritické množství</th></tr></thead><tbody>");

            foreach (var item in criticalMaterials)
            {
                body.Append($"<tr><td>{item.MaterialName}</td><td>{item.Quantity}</td><td>{item.CriticalQuantity}</td></tr>");
            }

            body.Append("</tbody></table>");

            var smtpClient = new SmtpClient
            {
                Host = _configuration["Smtp:Host"]!,
                Port = int.Parse(_configuration["Smtp:Port"]!),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _configuration["Smtp:Username"],
                    _configuration["Smtp:Password"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]!),
                Subject = subject,
                Body = body.ToString(),
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
