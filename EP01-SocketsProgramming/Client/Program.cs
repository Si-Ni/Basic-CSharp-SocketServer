using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadKey();

            var endpoint = new IPEndPoint(IPAddress.Loopback, 3000);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endpoint);

            var networkStream = new NetworkStream(socket, true);

            var msg = "Hello World";

            var buffer = System.Text.Encoding.UTF8.GetBytes(msg);

            networkStream.Write(buffer, 0, buffer.Length);

            var response = new byte[1024];

            var bytesRead = networkStream.Read(response, 0, response.Length);
            var responseStr = System.Text.Encoding.UTF8.GetString(response);

            Console.WriteLine($"Received: {responseStr}");

            Console.ReadKey();
        }
    }
}
