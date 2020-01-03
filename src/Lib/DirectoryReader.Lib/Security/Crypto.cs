using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Chilkat;

namespace DirectoryReader.Lib.Security
{
	public class Crypto
	{
		#region Constants

		private const string ALGORITHM = "aes";
		private const string CIPHER_MODE = "ebc";
		private const int KEY_LENGTH = 256;
		private const int PADDING_SCHEMA = 0;
		private const string ENCODE_MODE = "hex";
		private const string IVHEX = "000102030405060708090A0B0C0D0E0F";

		#endregion

		#region Members

		private Crypt2 crypt = null;

		#endregion

		public Crypto(string key)
		{

			Global glob = new Global();
			glob.UnlockBundle(CIPHER_MODE);

			this.crypt = new Crypt2();
			this.crypt.CryptAlgorithm = ALGORITHM;
			this.crypt.CipherMode = CIPHER_MODE;
			this.crypt.KeyLength = KEY_LENGTH;
			this.crypt.PaddingScheme = PADDING_SCHEMA;
			this.crypt.EncodingMode = ENCODE_MODE;
			this.crypt.SetEncodedIV(IVHEX,ENCODE_MODE);
			this.crypt.SetEncodedKey(key,ENCODE_MODE);
		}

		public string Encrypt(string value)
		{
			return crypt.EncryptStringENC(value);
		}

		public string Decrypt(string value)
		{
			return crypt.DecryptStringENC(value);
		}
	}
}
