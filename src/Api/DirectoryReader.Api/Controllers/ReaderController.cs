using System;
using System.Threading.Tasks;
using DirectoryReader.Api.Infrastructure.Model;
using DirectoryReader.Api.Repositories;
using DirectoryReader.Lib.Models.ContentModels;
using DirectoryReader.Lib.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DirectoryReader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
    public class ReaderController : ControllerBase
    {
		#region Members

		private readonly IRepository<ContentModel>			_contentRepository;
		private readonly IOptionsSnapshot<AppSettings>		_settings;

		#endregion

		#region Constructor

		public ReaderController (IRepository<ContentModel> repository, IOptionsSnapshot<AppSettings> settings)
		{
			this._contentRepository = repository;
			this._settings = settings;
		}

		#endregion

		#region Action Methods

		/// <summary>
		/// Content reader post method. Only accepts directory lib model data
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        // POST: api/Reader
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DirectoryModel model)
        {
			if(model != null)
			{
				try
				{
					// clean content protection
					model.ClearProtection(this._settings.Value.CryptoKey);

					// create the object 
					ContentModel content = new ContentModel(model);

					// insert
					await this._contentRepository.Create(content);

					// return as success
					return Ok(new ResponseModel(ResponseModel.State.Success));
				}
				catch (Exception e)
				{
					// else send the exception message
					return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel(ResponseModel.State.Failed, e.Message));
				}
			}

			// Finally send the invalid state
			return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel(ResponseModel.State.Failed, "Bad Request"));
        }

		#endregion
    }
}
