using ZeroSlope.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSlope.Service.Filters
{
	public class HandledResultFilter : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			context.ExceptionHandled = true;
			var error = new HandledResult<Exception>(context.Exception).HandleException();

			context.HttpContext.Response.Clear();
			context.HttpContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
			context.HttpContext.Response.StatusCode = error.StatusCode;
			context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(error), Encoding.UTF8);
		}
	}
}
