using System;
using System.Collections.Generic;
using System.Text;

namespace DirectoryReader.Lib.Models.ResponseModels
{
	public class ResponseModel
	{
		#region Enums

		public enum State
		{
			Success,
			Failed
		}

		#endregion

		#region Properties

		public State ResponseState { get ; set;}

		public string Response { get ; set;}
		public string ExceptionMessage { get ; set;}

		#endregion

		#region Constructor

		private ResponseModel()
		{
			
		}

		public ResponseModel(State state, string message = null)
		{
			this.ResponseState = state;

			switch (state)
			{
				case State.Success:		
					this.Response = "Success";
					break;
				case State.Failed:
					this.Response = "Failed";
					this.ExceptionMessage = message;
					break;
			}
		}

		#endregion
	}
}
