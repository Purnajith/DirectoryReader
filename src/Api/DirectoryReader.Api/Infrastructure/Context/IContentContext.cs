using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using DirectoryReader.Api.Infrastructure.Model;

namespace DirectoryReader.Api.Infrastructure.Context
{
	public interface IContentContext
	{
		IMongoCollection<ContentModel> Content { get; }
	}
}
