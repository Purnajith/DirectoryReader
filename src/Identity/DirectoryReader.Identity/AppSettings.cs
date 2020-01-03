using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryReader.Identity
{

	public class AppSettings
	{
		public Logging Logging { get; set; }
		public string AllowedHosts { get; set; }
		public Jwt Jwt { get; set; }
	}

	public class Logging
	{
		public Loglevel LogLevel { get; set; }
	}

	public class Loglevel
	{
		public string Default { get; set; }
	}

	public class Jwt
	{
		public string Key { get; set; }
		public string Issuer { get; set; }
	}

}
