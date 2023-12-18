using System.Net;
using System.Net.Sockets;

namespace Client
{
    internal class Program
    {
        private static int _number;
        private static readonly Random Random = new();
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Input();
            Console.ReadLine();
        }

        static void Input()
        {
            while (_number <= 10000)
            {
                _number = Random.Next(10000);
                Console.WriteLine($"Gia tri : {_number}");
                SendToServer(_number);

                Thread.Sleep(1000);
            }
        }

        static void SendToServer(int number)
        {
            try
            {
                var serverIpAddess = IPAddress.Loopback;
                var serverPort = 5000;
                var serverEndPoint = new IPEndPoint(serverIpAddess, serverPort);

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(serverEndPoint);
                if (socket.Connected)
                {
                    var numberBytes = BitConverter.GetBytes(number);
                    socket.Send(numberBytes);
                    socket.Shutdown(SocketShutdown.Send);
                    Console.WriteLine($"Da gui du lieu toi {serverEndPoint} thanh cong");
                }
                else
                {
                    Console.WriteLine("Khong the ket noi!!!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }
    }
}
