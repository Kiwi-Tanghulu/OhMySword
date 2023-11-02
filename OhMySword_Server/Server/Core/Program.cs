using System.Net;
using Server.Core;

namespace Server
{
    public class Program
    {
        private static string IPAddress = "172.31.3.230";
        private static int PORT = 8081;

        static void Main(string[] args)
        {
            PacketManager.Instance = new PacketManager();
            NetworkManager.Instance = new NetworkManager();

            if (NetworkManager.Instance.Listen(IPAddress, PORT) == false)
            {
                Console.WriteLine("문제가 생겼습니다");
                return;
            }

            while(true)
            {
                // loop
            }
        }
    }
}