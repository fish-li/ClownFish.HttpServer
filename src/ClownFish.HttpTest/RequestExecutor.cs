using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.WebClient;

namespace ClownFish.HttpTest
{
	public sealed class RequestExecutor
	{
		private RequestTest _request;

		public ResponseResult Result { get; private set; }

		public string ErrorMessage { get; private set; }

		public List<AssertResult> CheckResults { get; set; }


		public RequestExecutor(RequestTest request)
		{
			if( request == null )
				throw new ArgumentNullException(nameof(request));

			_request = request;
			this.Result = new ResponseResult();
		}

		public bool Execute()
		{
			if( SendRequest() == false )
				return false;


			if( _request.Response == null )
				return true;        // 不需要检查响应结果


			return CheckResponse();
		}


		private bool SendRequest()
		{
			try {
				HttpOption option = HttpOption.FromRawText(_request.Request);
				option.ReadResponseAction = x => {
					this.Result.StatusCode = (int)x.StatusCode;
					this.Result.Headers = x.Headers.CloneObject();
				};
				this.Result.ResponseText = option.Send();
			}
			catch( WebException webException ) {
				HttpWebResponse response = (webException.Response as HttpWebResponse);

				this.Result.StatusCode = (int)response.StatusCode;
				this.Result.Headers = response.Headers.CloneObject();

				RemoteWebException remoteException = new RemoteWebException(webException);
				this.Result.ResponseText = remoteException.ResponseText;
			}
			catch( Exception ex ) {
				this.ErrorMessage = ex.Message;
				return false;
			}

			return true;
		}


		private bool CheckResponse()
		{
			if( _request.Response.StatusCode > 0 ) {
				if( _request.Response.StatusCode != this.Result.StatusCode ) {
					this.ErrorMessage = $"{_request.Response.StatusCode} != {this.Result.StatusCode}";
					return false;
				}
			}

			this.CheckResults = new List<AssertResult>();

			if( _request.Response.Headers != null ) {
				foreach(var h in _request.Response.Headers ) {
					if( string.IsNullOrEmpty(h.AssertMode) || string.IsNullOrEmpty(h.Name) )
						continue;

					HeaderAssertChecker checker = HeaderAssertChecker.Create(h.AssertMode);
					AssertResult result = checker.Execute(h, this.Result);
					this.CheckResults.Add(result);

					if( result.IsPassed == false ) {
						this.ErrorMessage = result.Message;
						return false;
					}
				}
			}


			if( _request.Response.Body != null ) {
				foreach( var b in _request.Response.Body ) {
					if( string.IsNullOrEmpty(b.AssertMode) || string.IsNullOrEmpty(b.Name) )
						continue;

					BodyAssertChecker checker = BodyAssertChecker.Create(b.AssertMode);
					AssertResult result = checker.Execute(b, this.Result);
					this.CheckResults.Add(result);

					if( result.IsPassed == false ) {
						this.ErrorMessage = result.Message;
						return false;
					}
				}
			}

			return true;
		}

	}
}
