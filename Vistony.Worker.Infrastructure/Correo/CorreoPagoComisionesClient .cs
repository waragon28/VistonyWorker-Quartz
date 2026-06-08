using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;

namespace Vistony.Worker.Infrastructure.Correo
{
    public class CorreoPagoComisionesClient : ICorreoPagoComisionesClient
    {
        private readonly IConfiguration _configuration;

        public CorreoPagoComisionesClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarAsync(string titulo, string cuerpo)
        {
            string host = _configuration["Correo:Host"]!;
            int port = Convert.ToInt32(_configuration["Correo:Port"] ?? "587");
            string userFrom = _configuration["Correo:UserFrom"]!;
            string password = _configuration["Correo:Password"]!;

            var destinatarios = _configuration
                .GetSection("Correo:DestinatariosPagoComisiones")
                .Get<List<string>>() ?? new List<string>();

            using var smtp = new SmtpClient(host, port);
            using var mail = new MailMessage();

            mail.From = new MailAddress(userFrom);

            foreach (var destinatario in destinatarios)
            {
                mail.To.Add(destinatario);
            }

            mail.Subject = titulo;
            mail.Body = cuerpo;

            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(userFrom, password);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            await smtp.SendMailAsync(mail);
        }
    }
}
