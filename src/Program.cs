using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Luna
{
    static class Program
    {
        public static InputProcess inputProcess;
        public static TcpProcess tcpProcess;

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
			
			await Task.Delay(-1);
        }

		public static Mutex writeMutex = new Mutex();
		public static void WriteLine(string s, ConsoleColor color = ConsoleColor.White)
        {
            writeMutex.WaitOne();
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ResetColor();
            writeMutex.ReleaseMutex();
        }


        static async Task StartTcp(TcpConfig tcpConfig)
        {
            while(true)
            {
                tcpProcess = new TcpProcess();
                await tcpProcess.StartProcess(tcpConfig);
                WriteLine("TcpProcess ended, restarting...");
            }   
        }
        static async Task StartInput()
        {
            await Task.Delay(100);//tcp doesn't run if you don't do this
            while(true)
            {
                inputProcess = new InputProcess();
			    await inputProcess.StartProcess();
                WriteLine("Input ended, restarting...");
            }
        }
    }
}
