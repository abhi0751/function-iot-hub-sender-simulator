using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

namespace IoTHubSender
{
    public static class IoTSender
    {
       
        [FunctionName("Send_to_IoT_Hub")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var body = await req.Content.ReadAsAsync<JObject>();
            string connectionString = (string)body["connectionString"];
            using (DeviceClient _deviceClient = DeviceClient.CreateFromConnectionString(connectionString))
            {
                string messageString = @"{
                    ""message"": ""I created an IoT Application!"",
                    ""location"": ""Here or there""
                }";
                var message = new Message(Encoding.UTF8.GetBytes(messageString));

                await _deviceClient.SendEventAsync(message);
                return req.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
