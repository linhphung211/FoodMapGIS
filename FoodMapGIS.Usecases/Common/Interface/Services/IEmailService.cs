using System;
using System.Collections.Generic;
using System.Text;

namespace FoodMapGIS.Application.Common.Interface.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
    }
}
