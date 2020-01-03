using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO.Compression;
using DirectoryReader.Lib.Models.ContentModels;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using DirectoryReader.Lib.Models.ResponseModels;
using Microsoft.AspNetCore.DataProtection;
using DirectoryReader.Lib.Security;
using DirectoryReader.Lib.Models.Authentication;

namespace DirectoryReader.Webapp.Models.FileModels
{
	public class FileUploaderModel
	{
		#region Constants
		
		private const string FOLDER_PATH_SEPARATOR = "/";
		private const string FOLDER_UPLOAD = "_upload";
		private const string FOLDER_EXTRACT = "_extract";

		#endregion

		#region Members

		private readonly string rootFolder = String.Empty;
		private readonly string readerApiUrl  =  String.Empty;
		private readonly string authenticateApiUrl  =  String.Empty;
		private readonly IList<IFormFile>  files = null;
		private Crypto	crypto;
		private string token = String.Empty;

		#endregion

		#region Constructor

		private FileUploaderModel(string rootFolder, string readerApiUrl, string authenticateApiUrl, IList<IFormFile> files, string cryptoKey)
		{
			this.rootFolder = rootFolder;
			this.readerApiUrl = readerApiUrl;
			this.authenticateApiUrl = authenticateApiUrl;
			this.files = files;
			this.crypto = new Crypto(cryptoKey);
		}

		#endregion

		#region Execute Create

		public static FileUploaderModel ExecuteCreate(string rootFolder, string readerApiUrl, string authenticateApiUrl, IList<IFormFile> files, string cryptoKey)
		{
			FileUploaderModel result = null;

			if(!String.IsNullOrEmpty(rootFolder) && !String.IsNullOrEmpty(readerApiUrl) && !String.IsNullOrEmpty(authenticateApiUrl) && files.Count > 0)
			{
				result = new FileUploaderModel(rootFolder, readerApiUrl, authenticateApiUrl, files, cryptoKey);
			}

			return result;
		}

		#endregion

		#region Methods

		public async Task<ResponseModel> GetResponse()
		{
			ResponseModel result = null;

			TokenModel	tokenModel = await this.Authenticate(new UserModel() { Username = "TestUser" });

			if(tokenModel != null)
			{
				this.token = tokenModel.Token;

				if(!String.IsNullOrEmpty(this.token))
				{
					// loop through the files 
					foreach (IFormFile file in this.files)
					{
						string fileName = this.GetFileName(file);

						if(!String.IsNullOrEmpty(fileName))
						{
							string filePath = this.UploadFile(fileName, file);

							if(!String.IsNullOrEmpty(filePath))
							{
								string extractPath = this.GetFolderPath(FOLDER_EXTRACT);

								// check if exists 
								// then delete 
								this.ClearDirectory(extractPath);

								// extract the content
								ZipFile.ExtractToDirectory(filePath, extractPath);

								DirectoryModel model = this.GenarateContentModel(extractPath);

						
								this.ClearDirectory(extractPath);

								if(model != null)
								{
									result = await this.SendToContentReader(model);
								}

							}
						}
					}
				}
			}

			return result;
		}


		private void ClearFile(string filePath)
		{
			if(File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		private void ClearDirectory(string folderPath)
		{
			if(Directory.Exists(folderPath))
			{
				Directory.Delete(folderPath, true);
			}
		}

		private async Task<TokenModel> Authenticate(UserModel user)
		{
			TokenModel result = null;
			HttpClient httpClient = new HttpClient();
			string jsonString = JsonConvert.SerializeObject(user);

			HttpResponseMessage response = await httpClient.PostAsync(this.authenticateApiUrl, new StringContent(jsonString, Encoding.UTF8, "application/json"));

			if (response.IsSuccessStatusCode)
			{
				result = await response.Content.ReadAsAsync<TokenModel>();
			}

			return result;
		}


		private async Task<ResponseModel> SendToContentReader(DirectoryModel model)
		{
			ResponseModel result = null;
			HttpClient httpClient = new HttpClient();
			string jsonString = JsonConvert.SerializeObject(model);
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
			HttpResponseMessage response = await httpClient.PostAsync(this.readerApiUrl, new StringContent(jsonString, Encoding.UTF8, "application/json"));

			if (response.IsSuccessStatusCode)
			{
				result = await response.Content.ReadAsAsync<ResponseModel>();
			}
			else
			{
				result = await response.Content.ReadAsAsync<ResponseModel>();
			}

			return result;
		}

		private DirectoryModel GenarateContentModel(string extractPath)
		{
			DirectoryModel	result = null;
			DirectoryInfo extractDirectory = new DirectoryInfo(extractPath);

			if(extractDirectory != null && extractDirectory.GetDirectories() != null && extractDirectory.GetDirectories().Length > 0)
			{
				result = this.GetDirectoryModel(extractDirectory.GetDirectories()[0]);
			}

			return result;
		}

		private List<FileModel> GetFileList(FileInfo[] files)
		{
			List<FileModel> result = new List<FileModel>();

			foreach (FileInfo file in files)
			{
				result.Add(new FileModel(this.crypto.Encrypt(file.Name)));
			}

			return result;
		}

		private List<DirectoryModel> GetDirectoryList(DirectoryInfo[] directories)
		{
			List<DirectoryModel> result = new List<DirectoryModel>();

			foreach (DirectoryInfo directory in directories)
			{
				result.Add(this.GetDirectoryModel(directory));
			}

			return result;
		}

		private DirectoryModel GetDirectoryModel(DirectoryInfo directory)
		{
			DirectoryModel result = new DirectoryModel(this.crypto.Encrypt(directory.Name));
				
			if(directory.GetFiles() != null && directory.GetFiles().Length > 0)
			{
				result.FileList = this.GetFileList(directory.GetFiles());
			}

			if(directory.GetDirectories() != null && directory.GetDirectories().Length > 0)
			{
				result.DirectoryList = this.GetDirectoryList(directory.GetDirectories());
			}

			return result;
		}

		private string UploadFile(string fileName, IFormFile file)
		{
			string result = this.GetUploadPathWithFilename(fileName);

			// before created check if exits 
			// if exists then delete 
			this.ClearFile(result);

			using (FileStream output = System.IO.File.Create(result))
			{
				if (output != null)
				{
					file.CopyToAsync(output);
				}
			}

			return result;
		}


		private string GetFileName(IFormFile file)
		{
			string result = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

			if(!String.IsNullOrEmpty(result))
			{
				result = this.EnsureCorrectFilename(result);
			}

			return result;
		}


		private string EnsureCorrectFilename(string filename)
		{
		  if (filename.Contains("\\"))
			filename = filename.Substring(filename.LastIndexOf("\\") + 1);

		  return filename;
		}

		private string GetUploadPathWithFilename(string filename)
		{
			return this.CombinePath(this.GetFolderPath(FOLDER_UPLOAD), filename);
		}

		private string GetFolderPath(string folder)
		{
			string folderPath = this.CombinePath(this.rootFolder, folder);

			if (Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			return folderPath;
		}

		private string CombinePath(string val, string val2)
		{
			return val + FOLDER_PATH_SEPARATOR + val2;
		}

		#endregion
	}
}
