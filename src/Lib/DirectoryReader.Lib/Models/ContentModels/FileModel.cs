using System;
using System.Collections.Generic;
using System.Text;

namespace DirectoryReader.Lib.Models.ContentModels
{
	public class FileModel
	{
		public string Name { get; set; }

		public FileModel()
		{

		}

		public FileModel(string fileName)
		{
			this.Name = fileName;
		}
	}
}
