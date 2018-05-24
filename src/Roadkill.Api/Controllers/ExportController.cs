﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Roadkill.Api.Interfaces;
using Roadkill.Core.Models;
using Roadkill.Core.Repositories;

namespace Roadkill.Api.Controllers
{
	[Route("export")]
	public class ExportController : Controller, IExportService
	{
		private readonly IPageRepository _pageRepository;

		public ExportController(IPageRepository pageRepository)
		{
			_pageRepository = pageRepository;
		}

		[Route("ExportPagesToXml")]
		[HttpGet]
		public async Task<string> ExportPagesToXml()
		{
			IEnumerable<Page> allPages = await _pageRepository.AllPages();
			XmlSerializer serializer = new XmlSerializer(typeof(List<Page>));

			StringBuilder builder = new StringBuilder();
			using (StringWriter writer = new StringWriter(builder))
			{
				serializer.Serialize(writer, allPages);
				return builder.ToString();
			}
		}
	}
}