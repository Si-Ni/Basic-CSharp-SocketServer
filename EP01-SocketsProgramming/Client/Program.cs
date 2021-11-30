using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Client {

    public class Message {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }

    class Program {

        

        static async Task Main(string[] args) {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadKey();

            var endpoint = new IPEndPoint(IPAddress.Loopback, 3000);

            //var channel = new ClientChannel<JsonMessageProtocol, JObject>();
            var channel = new ClientChannel<XmlMessageProtocol, XDocument>();

            channel.OnMessage(OnMessage);

            await channel.ConnectAsync(endpoint).ConfigureAwait(false);

            var message = new Message {
                IntProperty = 404,
                StringProperty = "Hello World"
            };

            Console.WriteLine("Sending");
            Print(message);

            await channel.SendAsync(message).ConfigureAwait(false);

            Console.ReadKey();
        }

        static Task OnMessage(JObject jObject) {
            Console.WriteLine("Received JObject Message");
            Print(Convert(jObject));
            return Task.CompletedTask;
        }
        
        static Task OnMessage(XDocument xDocument) {
            Console.WriteLine("Received xDocument Message");
            Print(Convert(xDocument));
            return Task.CompletedTask;
        }

        static Message Convert(JObject jObject)
            => jObject.ToObject(typeof(Message)) as Message;

        static Message Convert(XDocument xmlDocument)
            => new XmlSerializer(typeof(Message)).Deserialize(new StringReader(xmlDocument.ToString())) as Message;

        static void Print(Message message) => Console.WriteLine($"Message.IntProperty = {message.IntProperty}, Message.StringProperty = {message.StringProperty}");
    }
}
