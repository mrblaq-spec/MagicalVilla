using AutoMapper;
using MagicalVilla_Web.Models;
using MagicalVilla_Web.Models.Dto;
using MagicalVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MagicalVilla_Web.Controllers
{
    public class VillaController : Controller 
    {
        public readonly IVillaService _villaService;
        public readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _mapper = mapper;
            _villaService = villaService;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDto> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

		public async Task<IActionResult> CreateVilla(int id)
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVilla(VillaCreateDto model)
		{
			if(ModelState.IsValid)
            {
				var response = await _villaService.CreateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Villa Created Successfully!";
					return RedirectToAction(nameof(IndexVilla));
				}
			}
			TempData["error"] = "Error Encountered!";
			return View(model);
		}

		public async Task<IActionResult> UpdateVilla(int villaId)
		{
			var response = await _villaService.GetAsync<APIResponse>(villaId);
			if (response != null && response.IsSuccess)
			{
				VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
				return View(_mapper.Map<VillaUpdateDto>(model));
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
		{
			if (ModelState.IsValid)
			{
				var response = await _villaService.UpdateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Villa Updated Successfully!";
					return RedirectToAction(nameof(IndexVilla));
				}
			}
			TempData["error"] = "Error Encountered!";

			return View(model);
		}

		public async Task<IActionResult> DeleteVilla(int villaId)
		{
			var response = await _villaService.GetAsync<APIResponse>(villaId);
			if (response != null && response.IsSuccess)
			{
				VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
				return View(model);
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVilla(VillaDto model)
		{
				var response = await _villaService.DeleteAsync<APIResponse>(model.Id);
				if (response != null && response.IsSuccess)
				{
				TempData["success"] = "Villa Deleted Successfully!";
				return RedirectToAction(nameof(IndexVilla));
				}
			TempData["error"] = "Error Encountered!";
			return View(model);
		}
	}
}
