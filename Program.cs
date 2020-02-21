using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Luna
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Luna";

			InputProcess.StartProcess();

			await Task.Delay(0);
        }

		public static Mutex writeMutex = new Mutex();
		static void WriteLine(string s)
        {
            writeMutex.WaitOne();
            Console.WriteLine(s);
            writeMutex.ReleaseMutex();
        }
    }
}
