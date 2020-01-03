using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryReader.Api.Infrastructure.Context;
using DirectoryReader.Api.Infrastructure.Model;
using MongoDB.Driver;

namespace DirectoryReader.Api.Repositories.Directory
{
	public class ContentRepository : IRepository<ContentModel>
	{
		#region Members

		private readonly IContentContext _context;

		#endregion

		#region Constructors

		public ContentRepository(IContentContext context)
		{
			_context = context;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create new content 
		/// </summary>
		/// <param name="content">content object</param>
		/// <returns></returns>
		public async Task Create(ContentModel content)
		{
			await _context.Content.InsertOneAsync(content);
		}

		#endregion
	}
}
