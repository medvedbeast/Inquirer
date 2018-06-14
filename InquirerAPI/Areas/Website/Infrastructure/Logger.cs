using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InquirerAPI.PublicAPI.Models;
using InquirerAPI.Website.Models;

namespace InquirerAPI.Website.Infrastructure
{
    public interface ILogger
    {
        Task Log(string action, bool status, int statusCode, string token, int externalUserId, int userId, string message = null);
    }

    public class Logger : ILogger
    {
        private Models.DatabaseContext InternalDatabase { get; set; }
        private PublicAPI.Models.DatabaseContext Database { get; set; }

        public Logger(Models.DatabaseContext internalDatabase, PublicAPI.Models.DatabaseContext database)
        {
            InternalDatabase = internalDatabase;
            Database = database;
        }

        public async Task Log(string action, bool status, int statusCode, string token, int externalUserId, int userId, string message = null)
        {
            var tokenId = InternalDatabase.Key
                .Where(k => k.Content == token)
                .Select(k => k.Id)
                .FirstOrDefault();

            var externalUser = Database.Users
                .Where(u => u.Id == externalUserId)
                .FirstOrDefault();

            string content = $"<u>'{externalUser.Email}'</u> invoked action '{action}'";
            if (!status)
            {
                content += $"<br/><b class='error'>Message: {message}<br/>Status code: {statusCode}</b>";
            }


            var activity = new Data.Activity
            {
                Status = status,
                KeyId = tokenId,
                OccuredOn = DateTime.Now,
                UserId = userId,
                ExternalUserId = externalUser.Id,
                Content = content
            };

            InternalDatabase.Activity.Add(activity);

            await InternalDatabase.SaveChangesAsync();
        }
    }
}
