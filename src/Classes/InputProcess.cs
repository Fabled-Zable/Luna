﻿using System;
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
						Program.greet();
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
					case "connect":
					{
						if(tokens.Length <= 3)
						{
							writeLine("Usage: connect host port password\nexample: connect 127.0.0.1 50301 hahayouwillneverguessthis");
						}

						string host = tokens[1];
						int port = -1;
						try
						{
							port = ushort.Parse(tokens[2]);
						}
						catch(FormatException)
						{
							writeLine("Could not parse port");
						}

						if(port != -1)
						{
							TcpConfig config = new TcpConfig();
							config.host = host;
							config.port = (ushort)port;
							config.password = tokens[2];
							config.logLevel = Program.tcpConfig.logLevel;
							Program.tcpProcess.disconnect();
							Program.StartTcp(config);
						}
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
