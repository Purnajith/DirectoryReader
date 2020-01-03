using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DirectoryReader.Lib.Models.ResponseModels;
using DirectoryReader.Webapp.Models.FileModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DirectoryReader.Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
		#region Members

		private readonly IHostingEnvironment				_env;
        private readonly IOptionsSnapshot<AppSettings>		_settings;
		#endregion

		#region Constructor

		public FileController(IHostingEnvironment env, IOptionsSnapshot<AppSettings> settings)
        {
            this._env = env;
            this._settings = settings;
        }

		#endregion

		#region Action Methods


		// POST: api/File
        [HttpPost]
        public async Task<IActionResult> Post(Input input)
        {
			try
			{
				//, IList<IFormFile> files
				/*
				FileUploaderModel fileUploaderModel = FileUploaderModel.ExecuteCreate(this._env.WebRootPath, this._settings.Value.API.ReaderAPIUrl, this._settings.Value.API.IdentityAPIUrl, files, this._settings.Value.CryptoKey);

				if(fileUploaderModel != null)
				{
					ResponseModel result = await fileUploaderModel.GetResponse();

					if(result != null)
					{
						return Ok(result);
					}
				}
				*/

				return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel(ResponseModel.State.Failed, "Bad Content"));
			}
            catch(Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel(ResponseModel.State.Failed, e.Message));
			}
			
        }

		#endregion
    }

	public class Input
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}