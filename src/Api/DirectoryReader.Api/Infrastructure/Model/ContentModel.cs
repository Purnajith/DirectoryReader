using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryReader.Lib.Models.ContentModels;
using Microsoft.AspNetCore.DataProtection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DirectoryReader.Api.Infrastructure.Model
{
	/// <summary>
	/// Content DB collection model
	/// </summary>
	public class ContentModel
	{
		#region Members

		private readonly IDataProtector	_protector;

		#endregion

		[BsonId]
		public ObjectId _id { get; set; }

		public DirectoryModel Directory { get; set; }

		public ContentModel ()
		{

		}

		public ContentModel (DirectoryModel directoryModel)
		{
			this.Directory = directoryModel;
		}
	}
}
