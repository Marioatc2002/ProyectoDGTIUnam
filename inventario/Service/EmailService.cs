using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using inventario.Models;

namespace inventario.Service
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string code);
    }

    public class GmailEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public GmailEmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendOtpEmailAsync(string email, string code)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sistema Inventario", _settings.Correo));
            mensaje.To.Add(new MailboxAddress("", email));
            mensaje.Subject = "Código de Recuperación";

            mensaje.Body = new TextPart("html")
            {
                Text = $"<div style='font-family:sans-serif;'><h2>Código: {code}</h2><p>Válido por 5 min.</p></div>"
            };

            using var client = new SmtpClient();
            // Servidor de Gmail: smtp.gmail.com | Puerto: 587
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Correo, _settings.Clave);
            await client.SendAsync(mensaje);
            await client.DisconnectAsync(true);
        }
    }
}