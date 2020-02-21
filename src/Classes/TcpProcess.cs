using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.Sockets;

namespace Luna
{
    public static class TcpProcess
    {
        private static TcpConfig config;
        private static TcpClient client = new TcpClient();
        private static NetworkStream ns;


        public static async Task StartProcess(TcpConfig _config)
        {
			Console.WriteLine("Starting Tcp");
			config = _config;

            startConnection();
		}
        
        private static void startConnection()
        {
            client.Connect("10.0.0.200",50301);
            ns = client.GetStream();

            sendData("derpa");
        }

        public static void sendData(string s)
        {
            ns.Write(System.Text.Encoding.ASCII.GetBytes(s + '\n'),0,s.Length + 1);
            beginDataRead();
        }

        private static async Task beginDataRead()
        {
            StreamReader reader = new StreamReader(ns);
            while(true)
			{
                 string chunk = reader.ReadLine();
                DataChunkReceived(chunk);
			}
        }

        private static void DataChunkReceived(string s)
        {
            if(config.logLevel == LogLevel.all)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Program.WriteLine('\t' + s);
                Console.ResetColor();
            }
        }

    }
}