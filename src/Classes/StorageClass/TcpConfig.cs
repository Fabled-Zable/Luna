namespace Luna
{
	public enum LogLevel
	{
		none,
		print,
		all
	}
    public class TcpConfig
    {
		public string host = "127.0.0.1";
		public ushort port = 50301;
		public string password = "youwillneverguessthishahaha";
		public LogLevel logLevel = LogLevel.all;

		public override string ToString()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}
	}
}
