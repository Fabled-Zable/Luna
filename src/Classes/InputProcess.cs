using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luna
{
	public class InputProcess
	{
		public async Task StartProcess()
		{
			
			bool running = true;
			while(running)
			{
				string input = "";
				input += Console.ReadKey(true).KeyChar;
				Program.writeMutex.WaitOne();
				Console.Write(input);
				while(true)
				{
					ConsoleKeyInfo key = Console.ReadKey();
					if(key.Key == ConsoleKey.Backspace)
					{
						if(input.Length < 1){break;}
						input = input.Substring(0,input.Length-1);
						Console.Write(" ");
						Console.SetCursorPosition(Console.CursorLeft -1, Console.CursorTop);
					}
					else if(key.Key == ConsoleKey.Enter){break;}
					else
					{
						input += key.KeyChar;
					}
				}
				Console.WriteLine();
				if(input == ""){continue;}
				string[] tokens = input.Split(" ");

				switch(tokens[0].ToLower())
				{	
					case "beep":
						Program.multiColorPrint("Beep beep lettuce!",Program.rainbowColors,new Random().Next(Program.rainbowColors.Length));
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
							running = false;
							Program.lunaSay("Goodbye!");
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
						if(tokens.Length > 1)
						{
							Program.tcpProcess.sendData(input.Substring(tokens[0].Length + 1));
						}
						else
						{
							writeLine("Usage: tcp command\nExample: tcp print('hello world');");
						}
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
							config.password = tokens[3];
							config.logLevel = Program.tcpProcess.config.logLevel;
							Program.tcpProcess.disconnect();
							Program.StartTcp(config);
						}
					}
					break;
					case "reconnect":
					case "recon":
					{
						TcpConfig config = Program.tcpProcess.config;
						Program.tcpProcess.disconnect();
						Program.tcpProcess = new TcpProcess();
						Program.StartTcp(config);
					}
					break;
					case "setloglevel":
					{
						if(tokens.Length == 3 )
						{
							string type = tokens[1];
							string sLevel = tokens[2];

							object level;
							
							bool success = Enum.TryParse(typeof(LogLevel), sLevel, ignoreCase:true, out level );
							if(success)
							{
								if(tokens[1] == "tcp")
								{
									Program.tcpProcess.config.logLevel = (LogLevel)level;
								}
								else if(tokens[1] == "discord")
								{
									writeLine("not implemented");
								}
								writeLine($"Log level set to {Program.tcpProcess.config.logLevel}");
							}
							else
							{
								writeLine("Failed parsing");
							}
						}
						else
						{
							writeLine("Usage setloglevel {tcp | discord} {none | print | all}");
						}
					}
					break;

					default:
						bool wasGreeting = false;
						foreach(string greeting in Program.greetings)
						{
							if(greeting.Replace("!","").ToLower() == tokens[0].ToLower().Replace("!",""))
							{
								wasGreeting = true;
								break;
							}
						}
						if(wasGreeting)
						{
							Program.greet();
						}
						else
						{
							if( SimpleResponses.ContainsKey(input.ToLower()))
							{
								Program.lunaSay(SimpleResponses[input] + '\n');
							}
						}
					break;
				}
				Program.writeMutex.ReleaseMutex();
			}
			await Task.CompletedTask;
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

		
		public static readonly Dictionary<string,string> SimpleResponses = new Dictionary<string, string>
		{
			["how are you"] = 
			"I'm doing fine",

			["what is your favorite color"] = 
			"Gray!",

			["when is your birthday"] =
			"February 20th, 2020!",

			["what is the point"] =
			"...",

			["yeet"] =
			"yote",

			["yote"] =
			"yeet",

			["ping"] = 
			"pong",

			["pong"] =
			"ping",

			["zxcvbcbgxgvsvzxcvbcb,mbcbm"] =
			"I love that song!",

			["bxcvcxzzccbvcxxcvbczz"] =
			"I love that song!",

			["knokbot"] =
			"He seemed kinda robotic",

			["luna"] =
			"Thats me!",

			["sorrn"] =
			"Was nothing like Rob's Realm of Secrets",

			["wizard wars"] =
			"Nothing like a good magic duel",
			["psychomermaid"] =
			"Oh no I hate having automamy I have no idea",

			["matt"] =
			"https://www.youtube.com/watch?v=dQw4w9WgXcQ",

			["zable"] =
			"What a nerd",

			["sonic7089"] =
			"According to all known laws of aviation,  there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the ground.  The bee, of course, flies anyway  because bees don't care what humans think is impossible.  Yellow, black. Yellow, black.Yellow, black. Yellow, black.  Ooh, black and yellow!Let's shake it up a little.  Barry! Breakfast is ready!  Coming!",

			["numan"] =
			"Once called TheSadNumanator",

			["carlospaul"] =
			"carlospaul is an 80 year old cactus with a knack for pooping on cars",

			["glitchgames"] =
			"*party horn noises*",

			["lordknightscie"] =
			"the highly unqualified lord of all knights",

			["vamist"] =
			"meow",

			["tflippy"] =
			"Flippers",

			["pirate rob"] =
			"Pop",

			["who are you"] =
			"Who is anyone?",

			["pi"] =
			"3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679",

			["joker563"] =
			"It's barfé!",

			["69"] =
			"N I C E",

			["it's dangerous to go alone"] =
			"Take this!"
		};	
	}
}
