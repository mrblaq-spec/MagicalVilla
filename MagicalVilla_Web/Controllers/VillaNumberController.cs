using AutoMapper;
using MagicalVilla_Utility;
using MagicalVilla_Web.Models;
using MagicalVilla_Web.Models.Dto;
using MagicalVilla_Web.Models.ViewModel;
using MagicalVilla_Web.Services;
using MagicalVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;

namespace MagicalVilla_Web.Controllers
{
	public class VillaNumberController : Controller
	{
		private readonly IVillaNumberService _villaNumberService;
		private readonly IMapper _mapper;
		private readonly IVillaService _villaService;
		public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService) 
		{
			_villaNumberService = villaNumberService;
			_villaService = villaService;
			_mapper = mapper;
		}
		public async Task<IActionResult> IndexVillaNumber()
		{
			List<VillaNumberDto> list = new();

			var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));
            }
			return View(list);
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVillaNumber(int id)
		{
			VillaNumberCreateVM villNumberVM = new();
			var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				villNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString(),
					});
			}
			return View(villNumberVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
		{
			if (ModelState.IsValid)
			{
				var res = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
				if (res != null && res.IsSuccess)
				{
                    TempData["success"] = "Villa Number Created Successfully!";
                    return RedirectToAction(nameof(IndexVillaNumber));
				}
				else
				{
					if (res.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", res.ErrorMessages.FirstOrDefault());
					}
				}
			}

			var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if(response != null && response.IsSuccess)
			{
				model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
			}
			return View(model);
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
		{
			VillaNumberUpdateVM villaNumberVM = new();
			var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
				villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);
			}

			response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
                return View(villaNumberVM);
			}

			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
		{

			if (ModelState.IsValid)
			{
				var res = await _villaNumberService.UpdateAsync<APIResponse>
					(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
				if (res != null && res.IsSuccess)
				{
                    TempData["success"] = "Villa Number Updated Successfully!";
                    return RedirectToAction(nameof(IndexVillaNumber));
				}
				else
				{
					if (res.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", res.ErrorMessages.FirstOrDefault());
					}
				}
			}

			var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
			}
			return View(model);
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
		{
			VillaNumberDeleteVM villaNumberVM = new();
			var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				VillaNumberDto model = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
				villaNumberVM.VillaNumber = _mapper.Map<VillaNumberDto>(model);
			}

			response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
				return View(villaNumberVM);
			}

			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
		{
			var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Villa Number Deleted Successfully!";
                return RedirectToAction(nameof(IndexVillaNumber));
			}
			return View(model);
		}
	}
}
