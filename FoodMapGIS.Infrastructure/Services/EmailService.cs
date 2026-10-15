using FoodMapGIS.Application.Common.Interface;
using FoodMapGIS.Application.Common.Interface.Services;
using Microsoft.Extensions.Logging;

namespace FoodMapGIS.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        // 🌟 IN MÃ OTP NỔI BẬT RA CONSOLE ĐỂ DEV TEST DỄ DÀNG:
        _logger.LogInformation("=================================================");
        _logger.LogInformation("[GỬI EMAIL TỚI]: {Email}", toEmail);
        _logger.LogInformation("[TIÊU ĐỀ]: {Subject}", subject);
        _logger.LogInformation("[NỘI DUNG]: \n{Body}", body);
        _logger.LogInformation("=================================================");

        // (Sau này đưa lên Production, bạn chỉ cần cấu hình thêm SMTP MailKit ở đây)
        return Task.CompletedTask;
    }
}