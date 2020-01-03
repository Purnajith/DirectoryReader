using System;
using System.Collections.Generic;
using System.Text;
using DirectoryReader.Lib.Security;
using Microsoft.AspNetCore.DataProtection;

namespace DirectoryReader.Lib.Models.ContentModels
{
	public class DirectoryModel
	{
		public string Name { get; set; }

		public List<DirectoryModel> DirectoryList { get; set; }

		public List<FileModel> FileList { get; set; }

		public DirectoryModel()
		{

		}

		public DirectoryModel(string directoryName)
		{
			this.Name = directoryName;
		}


		public void ClearProtection(string cryptoKey)
		{
			Crypto crypto = new Crypto(cryptoKey);

			this.CleanDirectoryProtection(this, crypto);
		}

		public DirectoryModel CleanDirectoryProtection(DirectoryModel directory, Crypto protector)
		{
			directory.Name = this.Unprotect(protector, directory.Name);

			if(directory.DirectoryList != null && directory.DirectoryList.Count > 0)
			{
				for (int i = 0; i < directory.DirectoryList.Count; i++)
				{
					directory.DirectoryList[i] = this.CleanDirectoryProtection(directory.DirectoryList[i], protector);
				}
			}

			if(directory.FileList != null && directory.FileList.Count > 0)
			{
				for (int i = 0; i < directory.FileList.Count; i++)
				{
					directory.FileList[i].Name = this.Unprotect(protector, directory.FileList[i].Name);
				}
			}

			return directory;
		}


		private string Unprotect(Crypto protector, string value)
		{
			return protector.Decrypt(value);
		}

	}
}
