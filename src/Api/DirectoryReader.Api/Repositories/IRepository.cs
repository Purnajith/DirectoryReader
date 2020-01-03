using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryReader.Api.Repositories
{
	public interface IRepository <T>
	{
		Task Create(T t);
	}
}
