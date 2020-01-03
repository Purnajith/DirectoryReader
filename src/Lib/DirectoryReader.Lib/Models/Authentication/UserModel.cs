using System;
using System.Collections.Generic;
using System.Text;

namespace DirectoryReader.Lib.Models.Authentication
{
	public class UserModel
	{
		public string Username { get; set; }
		public string EmailAddress { get; set; }

		public string Password { get; set; }
	}
}
