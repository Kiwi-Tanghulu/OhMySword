using System.Net;
using Server;

namespace Server
{
    public class Program
    {
        private static string IPAddress = "192.168.0.7"; // SEH00N PMK DESKTOP IP
        private static int PORT = 2651;

        static void Main(string[] args)
        {
            RoomManager.Instance = new RoomManager();
            PacketManager.Instance = new PacketManager();
            NetworkManager.Instance = new NetworkManager();

            if (NetworkManager.Instance.Listen(IPAddress, PORT) == false)
            {
                Console.WriteLine($"{DateTime.Now.ToString("yy년 MM월 dd일 HH:mm:ss")} [Core] Something Problem Detected With Opening Server");
                return;
            }

            Console.WriteLine($"{DateTime.Now.ToString("yy년 MM월 dd일 HH:mm:ss")} [Core] Server Opened : {IPAddress}:{PORT}");

            RoomManager.Instance.FlushLoop(50);
        }
    }
}