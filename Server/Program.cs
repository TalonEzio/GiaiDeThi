using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        private static int _number = 0;
        private static readonly List<int> CacheClient = new List<int>();
        private static bool _isStopped = false;
        static void Main()
        {

            Console.Title = "Server";

            var ipAddress = IPAddress.Loopback;
            var port = 5000;
            var endPoint = new IPEndPoint(ipAddress, port);

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            listener.Listen(10);

            var thread = new Thread(() => HandleClient(listener));
            thread.Start();


            while (!_isStopped)
            {
                //Nothing...
            }
            
            Console.WriteLine("Bam phim bat ki de ket thuc chuong trinh");
            Console.ReadKey();
        }

        static void HandleClient(Socket listener)
        {
            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];
            Console.WriteLine("Server da san sang");
            while (true)
            {
                var socket = listener.Accept();
                Console.WriteLine($"Da nhan ket noi tu {socket.RemoteEndPoint}");
                var receiveLength = socket.Receive(buffer);
                socket.Shutdown(SocketShutdown.Receive);
                _number = BitConverter.ToInt32(buffer, 0);
                if (_number > 10000)
                {
                    _isStopped = true;
                    break;
                }

                CacheClient.Add(_number);
                Console.WriteLine($"Gia tri nho nhat phia client: {CacheClient.Min()}");
                Array.Clear(buffer, 0, receiveLength);
            }
        }
    }
}
