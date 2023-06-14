using AutoMapper;
using MagicalVilla_Utility;
using MagicalVilla_Web.Models;
using MagicalVilla_Web.Models.Dto;
using MagicalVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicalVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		public readonly IVillaService _villaService;
		public readonly IMapper _mapper;
		public HomeController(IVillaService villaService, IMapper mapper)
		{
			_mapper = mapper;
			_villaService = villaService;
		}

		public async Task<IActionResult> Index()
		{
			List<VillaDto> list = new();

			var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
			}
			return View(list);
		}

    }
}