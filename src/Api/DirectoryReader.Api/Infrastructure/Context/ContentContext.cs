using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryReader.Api.Infrastructure.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DirectoryReader.Api.Infrastructure.Context
{

	public class ContentContext : IContentContext
	{
		private readonly IMongoDatabase _db;
		public ContentContext(IOptions<AppSettings> options)
		{
			var client = new MongoClient(options.Value.MongoDB.ConnectionString);
			_db = client.GetDatabase(options.Value.MongoDB.Database);
		}
		public IMongoCollection<ContentModel> Content => _db.GetCollection<ContentModel>("Content");
	}
}
