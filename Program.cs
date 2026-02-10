using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace bHapticsServer
{
    internal static class Program
    {
        private const int UdpPort = 5015;
        public static TactsuitVR tactsuitVR;

        private static void Main()
        {
            tactsuitVR = new TactsuitVR();
            CreateUdpServer();
        }

        private static void CreateUdpServer()
        {
            using (var udpServer = new UdpClient(new IPEndPoint(IPAddress.Loopback, UdpPort)))
            {
                Console.WriteLine($"UDP Server started listening on 127.0.0.1:{UdpPort}");

                while (true)
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var receivedBytes = udpServer.Receive(ref remoteEndPoint);
                    var receivedText = Encoding.UTF8.GetString(receivedBytes);
                    HandleMessage(receivedText);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void HandleMessage(string message)
        {
            Console.WriteLine($"Received message: {message}");
    
            if(message.Contains(","))
            {
                var parameters = message.Split(',');        
                var effectKey = parameters[0];
                var intensity = float.TryParse(parameters[1], out var intensityResult) ? intensityResult : 1f;
                var duration = float.TryParse(parameters[2], out var durationResult) ? durationResult : 1f;
                var offsetX = float.TryParse(parameters[3], out var offsetXResult) ? offsetXResult : 0;
                var offsetY = float.TryParse(parameters[4], out var offsetYResult) ? offsetYResult : 0;

                TactsuitVR.PlaybackHaptics(effectKey, intensity, duration, offsetX, offsetY);
                Console.WriteLine($"Playing effect with parameters: {message}");
            }
            else
            {
                TactsuitVR.PlaybackHaptics(message);
                Console.WriteLine($"Playing effect: {message}");
            }
        }
    }
}
