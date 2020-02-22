using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.Sockets;

namespace Luna
{
    public class TcpProcess
    {
        private TcpConfig config;
        private TcpClient client = new TcpClient();
        private NetworkStream ns;


        public async Task StartProcess(TcpConfig _config)
        {
			Console.WriteLine("Starting Tcp");
			config = _config;

            startConnection();
		}
        
        private void startConnection()
        {
            client.Connect("10.0.0.200",50301);
            ns = client.GetStream();

            sendData("derpa");
			beginDataRead();
		}

        public void sendData(string s)
        {
            ns.Write(System.Text.Encoding.ASCII.GetBytes(s + '\n'),0,s.Length + 1);
        }

        private async Task beginDataRead()
        {
            StreamReader reader = new StreamReader(ns);
            while(true)
			{
                 string chunk = reader.ReadLine();
                DataChunkReceived(chunk);
			}
        }

        private void DataChunkReceived(string s)
        {
            Program.WriteLine(s, ConsoleColor.DarkMagenta);
        }

    }
}