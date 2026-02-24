using inventario.Models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace inventario.Service
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioSettings _settings;

        public TwilioSmsService(IOptions<TwilioSettings> options)
        {
            // .Value contiene los datos ya cargados desde el JSON
            _settings = options.Value;
        }
        public async Task SendOtpAsync(string phoneNumber, string code)
        {
            var cleanedPhone = phoneNumber.Trim().Replace(" ", "").Replace("-", "");
            // Leemos los valores desde appsettings.json
            TwilioClient.Init(_settings.Sid, _settings.Token);

            if (!cleanedPhone.StartsWith("+"))
            {
                cleanedPhone = "+52" + cleanedPhone;
            }

            try
            {
                TwilioClient.Init(_settings.Sid, _settings.Token);
                await MessageResource.CreateAsync(
                    body: $"Tu código de verificación es: {code}",
                    from: new PhoneNumber(_settings.From),
                    to: new PhoneNumber(cleanedPhone) // Enviamos el número ya formateado
                );
            }
            catch (Exception ex)
            {
                // Log de error...
                throw;
            }
        }
    }

}
