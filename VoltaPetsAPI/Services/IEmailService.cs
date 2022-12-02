using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace VoltaPetsAPI
{
    public interface IEmailService
    {
        Task<Response> SendEmailAsync<T>(string to, string templateId, T data, string from = "voltapets@gmail.com");
    }

    public class SendGridMailService : IEmailService
    {
        private IConfiguration _configuration;

        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> SendEmailAsync<T>(string hacia, string templateId, T data, string desde = "voltapets@gmail.com")
        {
            var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(desde, "Volta Pets"));
            msg.AddTo(new EmailAddress(hacia));
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(data);
            var response = await client.SendEmailAsync(msg);

            return response;
        }

    }

}

