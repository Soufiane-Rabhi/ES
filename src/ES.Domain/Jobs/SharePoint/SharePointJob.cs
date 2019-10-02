using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SAM.Domain.Models;
using SAM.Infrastructure.Http.Extensions;
using SAM.Infrastructure.Serialization;

namespace SAM.Domain.Jobs.SharePoint
{
    public class SharePointJob : IJob<SharePointJobData>
    {
        private readonly IMailer _mailer;
        private readonly IConfiguration _configuration;

        public SharePointJob(
            IConfiguration configuration,
            IMailer mailer,
            ILogger<SharePointJobData> logger,
            IJsonSerializer jsonSerialiser,
            IHttpClientFactory httpClientFactory)
            : base(logger, jsonSerialiser, httpClientFactory)
        {
            _mailer = mailer;
            _configuration = configuration;
        }
        public override async Task ExecuteAsync(SharePointJobData data, CancellationToken cancellationToken = default)
        {
            var httpClient = HttpClientFactory.CreateClient("SynchronizationHttp");
            var directory = string.IsNullOrEmpty(data.Directory) ? string.Empty : $"{data.Directory}/";
            Logger.LogInformation($"Sending request to {directory}api/SharePoint/UpdateUsersPermissions");
            var request = new HttpRequestMessage()
                .Post()
                .ForUrl($"{directory}api/SharePoint/UpdateUsersPermissions")
                .WithJsonPayload(data.Permissions, JsonSerializer);

            var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            Logger.LogInformation("Update Done Succefully.");

            var result = JsonSerializer.Deserialize<List<SharePointCommand>>(await response.Content.ReadAsStringAsync());
            if (result.Any())
            {
                var commandGroup = result.GroupBy(command => command.UserEmail);
                foreach (var command in commandGroup)
                {
                    await SendNotificationMessageAsync(command.Key, command.ToList());
                }
            }
        }

        #region "Private Helpers"

        private async Task SendNotificationMessageAsync(string destination, List<SharePointCommand> commands)
        {
            var message = new MailMessage
            {
                Subject = "ICC New authorizations have been assigned to you",
                From = new MailAddress("no-reply@iccwbo.org", "ICC"),
            };

            var recipients = _configuration.GetSection("EmailNotification").Get<List<string>>();
            if (recipients != null && recipients.Any())
            {
                // Test environment [Dev - Staging]
                recipients.ForEach(email => message.To.Add(new MailAddress(email)));
            }
            else
            {
                // Production environment
                message.To.Add(new MailAddress(destination));
            }
            await _mailer.SendAsync(commands, "Mail/ICC", message);
        }

        #endregion
    }
}
