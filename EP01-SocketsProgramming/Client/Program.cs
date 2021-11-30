using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);
            var networkStream = new NetworkStream(socket, true);

            var message = new Message {
                IntProperty = 404,
                StringProperty = "Hello World"
            };

            Console.WriteLine("Sending");
            Print(message);

            var protocol = new XmlMessageProtocol();
            //var protocol = new JsonMessageProtocol();
            await protocol.SendAsync(networkStream, message).ConfigureAwait(false);

            var responseMsg = await protocol.ReceiveAsync(networkStream).ConfigureAwait(false);

            var response = Convert(responseMsg);

            Console.WriteLine("Received");
            Print(response);


            Console.ReadKey();
        }

        static Message Convert(JObject jObject)
            => jObject.ToObject(typeof(Message)) as Message;

        static Message Convert(XDocument xmlDocument)
            => new XmlSerializer(typeof(Message)).Deserialize(new StringReader(xmlDocument.ToString())) as Message;

        static void Print(Message message) => Console.WriteLine($"Message.IntProperty = {message.IntProperty}, Message.StringProperty = {message.StringProperty}");
    }
}
