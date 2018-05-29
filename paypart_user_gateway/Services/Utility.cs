using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using paypart_user_gateway.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace paypart_user_gateway.Services
{
    public class Utility
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-$@!()[]?";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringOnly(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<bool> sendMail(User user, string newpwd, IOptions<Settings> settings)
        {
            bool isSent = false;
            EmailMetaData emd = new EmailMetaData();
            emd.body = settings.Value.resetNotifyBody.Replace(settings.Value.resetNotifynewpass, "' " + newpwd + " '");
            emd.datesubmitted = DateTime.Now;
            emd.subject = settings.Value.resetNotifySubject;
            emd.toaddress = user.email;
            string request = string.Empty;
            string responseTxt = string.Empty;
            string contentType = "application/json";

            using (var client = new HttpClient())
            {
                request = JsonHelper.toJson(emd);
                var content = new StringContent(request, Encoding.UTF8, contentType);
                HttpResponseMessage response = await client.PostAsync(settings.Value.notifyUrl + "email", content);
                responseTxt = await response.Content.ReadAsStringAsync();

                isSent = JsonConvert.DeserializeObject<bool>(responseTxt);
            }
            return isSent;
        }
    }
}
