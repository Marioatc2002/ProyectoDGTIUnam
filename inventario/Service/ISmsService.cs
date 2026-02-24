

namespace inventario.Service
{
    public interface ISmsService
    {
        Task SendOtpAsync(string phoneNumber, string code);
    }

}
