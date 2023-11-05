using System.Net;
using Server;

namespace Server
{
    public class Program
    {
        //private static string IPAddress = "172.31.3.230";
        private static string IPAddress = "172.30.1.16";
        private static int PORT = 2651;

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