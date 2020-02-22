using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.Sockets;

namespace Luna
{
    public class TcpProcess
    {
        private bool disposed = false;
        private TcpConfig config;
        private TcpClient client = new TcpClient();
        private NetworkStream ns;


        public async Task StartProcess(TcpConfig _config)
        {
			config = _config;

            startConnection();
		}

        public async Task StartProcess(TcpConfig _config, string host, ushort port, string password)
        {
            config = _config;
            config.host = host;
            config.port = port;
            config.password = password;

            startConnection();
        }
        
        private void startConnection()
        {
            client.Connect(config.host,config.port);
            ns = client.GetStream();

            sendData(config.password);
			beginDataRead();
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

        private async Task beginDataRead()
        {
            StreamReader reader = new StreamReader(ns);
            while(!disposed)
			{
                string chunk = reader.ReadLine();
                DataChunkReceived(chunk);
			}
        }

        private void DataChunkReceived(string s)
        {
            Program.WriteLine(s, ConsoleColor.DarkMagenta);
        }

        internal void disconnect()
        {
            client.Close();
        }
    }
}