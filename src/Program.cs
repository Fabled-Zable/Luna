﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Luna
{
    static class Program
    {
        readonly static string[] greetings = {"Hello","Hi!","Welcome!", "Hoi", "Henlo"};
        readonly static string[] emotes = {":D",":)","C:",":P",":o"};
        public static void greet()
        {
            Random random = new Random();
            string greeting = greetings[random.Next(0,greetings.Length)] + " " + emotes[random.Next(emotes.Length)];
            if(random.Next(100) == 69)
            {
                greeting = "Ugh. Don't talk to me until i've had my coffee D:";
            }
            
            ConsoleColor[] colors = {ConsoleColor.Red,ConsoleColor.Yellow,ConsoleColor.Green,ConsoleColor.Blue,ConsoleColor.Magenta};

            int offset = random.Next(colors.Length);
            for(int i = 0; i < greeting.Length; i++)
            {
                Console.ForegroundColor = colors[((i + offset)%colors.Length)];
                char c = greeting[i];
                if(c == ' '){offset--;}
                Console.Write(c);
            }
            Console.Write('\n');
            Console.ResetColor();
        }


        public static InputProcess inputProcess;
        public static TcpProcess tcpProcess;
        public static TcpConfig tcpConfig;

        public static List<Task> tasks = new List<Task>();
        static async Task Main(string[] args)
        {
            Console.Title = "Luna";
            greet();
            tcpConfig = new TcpConfig();

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


        public static async Task StartTcp(TcpConfig tcpConfig)
        {
			await Task.Delay(50);//I'm sure I'm doing something wrong with async if I have to do this
            tcpProcess = new TcpProcess();
            await tcpProcess.StartProcess(tcpConfig);
            WriteLine("TcpProcess ended");
        }
        static async Task StartInput()
        {
            await Task.Delay(50);//tcp doesn't run if you don't do this
            while(true)
            {
                inputProcess = new InputProcess();
			    await inputProcess.StartProcess();
                WriteLine("Input ended, restarting...");
            }
        }
    }
}
