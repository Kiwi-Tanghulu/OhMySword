using System.Net;
using Server;

namespace Server
{
    public class Program
    {
        private static string IPAddress = "172.31.3.230";
        //private static string IPAddress = "172.30.1.16";
        private static int PORT = 2651;

        static void Main(string[] args)
        {
            RoomManager.Instance = new RoomManager();
            PacketManager.Instance = new PacketManager();
            NetworkManager.Instance = new NetworkManager();

            if (NetworkManager.Instance.Listen(IPAddress, PORT) == false)
            {
                Console.WriteLine($"[Core] Something Problem Detected With Opening Server");
                return;
            }

            Console.WriteLine($"[Core] Server Opened : {IPAddress}:{PORT}");

            RoomManager.Instance.FlushLoop(50);
        }
    }
}