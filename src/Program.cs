using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Luna
{
    class Program
    {
        public static List<Task> tasks = new List<Task>();
        static async Task Main(string[] args)
        {
            Console.Title = "Luna";
            TcpConfig tcpConfig = new TcpConfig();

            if(!Directory.Exists("storage"))
            {
                Console.WriteLine("storage directory not found, creating");
                Directory.CreateDirectory("storage");
            }
            if(!File.Exists("storage/tcpConfig.json"))
            {
                Console.WriteLine("tcpConfig.json not found, creating");
                File.WriteAllText("storage/tcpConfig.json",JsonConvert.SerializeObject(tcpConfig));
            }
            tcpConfig = JsonConvert.DeserializeObject<TcpConfig>(File.ReadAllText("storage/tcpConfig.json"));

            _ = StartInput();
            _ = StartTcp(tcpConfig);
			
			await Task.Delay(0);
        }

		public static Mutex writeMutex = new Mutex();
		public static void WriteLine(string s)
        {
            writeMutex.WaitOne();
            Console.WriteLine(s);
            writeMutex.ReleaseMutex();
        }


        static async Task StartTcp(TcpConfig tcpConfig)
        {
            await TcpProcess.StartProcess(tcpConfig);
        }
        static async Task StartInput()
        {
            await Task.Delay(100);//tcp doesn't run if you don't do this
			await InputProcess.StartProcess();
        }
    }
}
