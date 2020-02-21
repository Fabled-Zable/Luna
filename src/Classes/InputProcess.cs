using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
	public static class InputProcess
	{
		public static async Task StartProcess()
		{
			while(true)
			{
				Program.writeMutex.WaitOne();
				string input = Console.ReadLine();
				string[] tokens = input.Split(" ");
				switch(tokens[0].ToLower())
				{
					case "help":
					{
						writeLine("Not implemented");
					}
					break;
					case "credits":
					{
							writeLine("Luna is currently being developed by Zable and I'm sure other will/have helped at some point");
					}
					break;
					case "exit":
					{
							write("Are you sure you want to exit? [y/*]");
							if(Console.ReadKey().Key == ConsoleKey.Y)
							{
								Environment.Exit(0);
							}
						Console.WriteLine("");
					}
					break;
					case "cls":
					case "clear":
					{
						Console.Clear();
					}
					break;
					case "tcp":
					{
						TcpProcess.sendData(input.Substring(tokens[0].Length + 1));
					}
					break;
				}
				Program.writeMutex.ReleaseMutex();
			}
		}

		private static void writeLine(string s)
		{
			write(s + '\n');
		}
		private static void write(string s)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("\t" + s);
			Console.ResetColor();
		}
	}
}
