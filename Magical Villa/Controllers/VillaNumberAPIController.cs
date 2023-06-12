using MagicalVilla_API.Data;
using MagicalVilla_API.Logging;
using MagicalVilla_API.Models;
using MagicalVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using AutoMapper;
using MagicalVilla_API.Repository.IRepository;
using System.Net;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicalVilla_API.Controllers
{
    //[Route("api/VillaAPI")]
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly ILogging _logger;

        protected APIResponse _response;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;

        private readonly IMapper _mapper;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, ILogging logger, IMapper mapper)
        {
            _logger = logger;

            _response = new();
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _dbVilla = dbVilla;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbersAsync()
        {
            try
            {
                //_logger.LogInformation("Getting all villaNumbers");
                _logger.Log("Getting all the VillaNumbers", "");
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumberAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log(" Get Villa Number Error with Id " + id, "error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _logger.Log(" Get Villa Number Error with Id " + id, "error");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumberAsync([FromBody] VillaNumberCreateDto createDto)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _dbVilla.GetAsync(u => u.Id == createDto.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDto);
                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumberAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut(Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumberAsync([FromBody]VillaNumberUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null)
                {
                    return BadRequest(updateDto);
                }
                if (await _dbVilla.GetAsync(u => u.Id == updateDto.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateDto);
                await _dbVillaNumber.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        /*[HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVillaNumberAsync(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villaNumber = await _dbVillaNumber.GetAsync(u => u.Id == id, tracked: false);
            VillaNumberUpdateDto villaNumberDTO = _mapper.Map<VillaNumberUpdateDto>(villaNumber);

            if (villaNumber == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaNumberDTO, ModelState);
            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDTO);
            await _dbVillaNumber.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }*/
    }
}
