using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Luna
{
	public class InputProcess
	{
		public async Task StartProcess()
		{
			while(true)
			{
				Console.CursorVisible = false;
				Console.ReadKey(true);
				Console.CursorVisible = true;
				Program.writeMutex.WaitOne();
				Console.Write(">");
				string input = Console.ReadLine();
				string[] tokens = input.Split(" ");
				switch(tokens[0].ToLower())
				{	
					case "hi":
					case "hoi":
					case "henlo":
					case "hello":
					{
						Random r = new Random();
						string[] response = {"Hello!", "Go away >:(", "Hi", "Don't talk to me before I've had my coffee", "Hoi", "Henlo"};
						writeLine(response[r.Next(0,response.Length)]);
					}
					break;
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
						Program.tcpProcess.sendData(input.Substring(tokens[0].Length + 1));
					}
					break;
					case "disconnect":
					{
							Program.tcpProcess.disconnect();
					}
					break;
				}
				Program.writeMutex.ReleaseMutex();
			}
		}

		private void writeLine(string s)
		{
			write(s + '\n');
		}
		private void write(string s)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("\t" + s);
			Console.ResetColor();
		}
	}
}
