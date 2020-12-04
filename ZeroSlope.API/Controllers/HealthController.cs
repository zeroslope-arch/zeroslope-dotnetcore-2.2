using ZeroSlope.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ZeroSlope.Domain.BindingModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ZeroSlope.Service.Controllers
{
	[Route("api/[controller]")]
	public class HealthController
	{
		/// <summary>
		/// Returns the health state of the service.
		/// </summary>
		/// <returns></returns>
		[HttpGet, Route("")]
        [AllowAnonymous]
		public ActionResult List()
		{
            return new OkResult();
		}

	}
}
