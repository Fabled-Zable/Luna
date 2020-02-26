using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.Sockets;

namespace Luna
{
    public class TcpProcess
    {
        public TcpConfig config;
        private TcpClient client = new TcpClient();
        private NetworkStream ns;


        public async Task StartProcess(TcpConfig _config)
        {
			config = _config;

            await startConnection();
		}

        public async Task StartProcess(TcpConfig _config, string host, ushort port, string password)
        {
            config = _config;
            config.host = host;
            config.port = port;
            config.password = password;

            await startConnection();
        }
        
        private async Task startConnection()
        {
            client.Connect(config.host,config.port);
            ns = client.GetStream();
            sendData(config.password);
            sendData($"print('Luna has connected :D');");
            
			await beginMainLoop();
		}

        public void sendData(string s)
        {
			if(client.Connected == false)
			{
				Program.WriteLine("\t not connected", ConsoleColor.DarkMagenta);
			}
			else
				ns.Write(System.Text.Encoding.ASCII.GetBytes(s + '\n'),0,s.Length + 1);
        }

        private async Task beginMainLoop()
        {
            StreamReader reader = new StreamReader(ns);
            while(client.Connected)
			{
                string chunk = reader.ReadLine();
                DataChunkReceived(chunk);
			}
            
            await Task.CompletedTask;
        }

        private void DataChunkReceived(string s)
        {
            if(config.logLevel == LogLevel.all)
                Program.WriteLine(s, ConsoleColor.DarkMagenta);
        }

        internal void disconnect()
        {
            client.Close();
        }
    }
}