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
        readonly static ConsoleColor favoriteColor = ConsoleColor.DarkGray;
        public readonly static string[] greetings = {"Hello","Hi!","Welcome!", "Hoi", "Henlo","hai", "Hallo","Halloo!","Hullo","Salutations!"};
        public readonly static string[] emotes = {":D",":)","(:","C:",":p",":o","o:","\\(OuO)/","\\O/"};
        public readonly static ConsoleColor[] rainbowColors = {ConsoleColor.Red,ConsoleColor.Yellow,ConsoleColor.Green,ConsoleColor.Cyan,ConsoleColor.Blue,ConsoleColor.Magenta};

        public static void multiColorPrint(string text, ConsoleColor[] colors, int offset = 0)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            for(int i = 0; i < text.Length; i++)
            {
                Console.ForegroundColor = colors[(i + offset)%colors.Length];
                char c = text[i];
                if(c == ' '){offset--;}
                Console.Write(c);
            }
            Console.ForegroundColor = originalColor;

        }

        public static void greet()
        {
            DateTime date = DateTime.Now;

            bool rainbow = true;
            ConsoleColor[] colors = rainbowColors;
            Random random = new Random();
            string greeting = greetings[random.Next(0,greetings.Length)] + " " + emotes[random.Next(emotes.Length)];
            bool birthday = date.Day == 20 && date.Month == 2;
            if(birthday)
            {
                greeting = $"Happy birthday to me! <3\nI am now {date.Year - 2020} years old!";
            }
            if(random.Next(101) == 69)
            {
                greeting = "Ugh! DON'T TALK TO ME UNTIL I'VE HAD MY COFFEE! >:(" + (birthday ? "\n... even if it is my birthday..." : "");
                rainbow = false;
                colors = new ConsoleColor[] {ConsoleColor.Red, ConsoleColor.DarkRed};
            }
            lunaSay(greeting + '\n',colors);
        }

        public static void lunaSay(string s)
        {
            lunaSay(s,rainbowColors);
        }
        public static void lunaSay(string s, ConsoleColor[] colors)
        {
            Console.ForegroundColor = favoriteColor;
            Console.Write("Luna: ");
            multiColorPrint(s,colors, new Random().Next(colors.Length));
            Console.ResetColor();
        }


        public static InputProcess inputProcess;
        public static TcpProcess tcpProcess;

        public static List<Task> tasks = new List<Task>();
        static async Task Main(string[] args)
        {
            Console.Title = "Luna";
            greet();
            TcpConfig tcpConfig = new TcpConfig();

            if(!Directory.Exists("storage"))//todo, refactor file stuff
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
