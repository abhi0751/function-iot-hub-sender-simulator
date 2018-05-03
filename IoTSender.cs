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
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var body = await req.Content.ReadAsAsync<JObject>();
            string IotHubUri = (string)body["iotHubUri"];
            string DeviceKey = (string)body["deviceKey"];
            string DeviceId = (string)body["deviceId"];
            using (DeviceClient _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt))
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
