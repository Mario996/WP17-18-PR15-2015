using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebProj.Common
{
	public class NotFoundWithMessageResult : IHttpActionResult
	{
		private string message;

		public NotFoundWithMessageResult(string message)
		{
			this.message = message;
		}

		public string Message
		{
			get
			{
				return message;
			}
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(HttpStatusCode.Conflict);
			response.Content = new StringContent(Message);
			return Task.FromResult(response);
		}
	}
}