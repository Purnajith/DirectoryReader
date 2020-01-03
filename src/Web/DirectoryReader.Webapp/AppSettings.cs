using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryReader.Webapp
{
	public class AppSettings
	{
		public Logging Logging { get; set; }
		public string AllowedHosts { get; set; }
		public API API { get; set; }

		public string CryptoKey { get; set; }
	}

	public class Logging
	{
		public Loglevel LogLevel { get; set; }
	}

	public class Loglevel
	{
		public string Default { get; set; }
	}

	public class API
	{
		public string URL { get; set; }
		public string ReaderAPI { get; set; }
		public string IdentityAPI { get; set; }

		internal string ReaderAPIUrl
		{
			get
			{
				return URL + "/" + ReaderAPI;
			}
		}

		internal string IdentityAPIUrl
		{
			get
			{
				return URL + "/" + IdentityAPI;
			}
		}
	}

}
