using MagicalVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicalVilla_Web.Models.ViewModel
{
	public class VillaNumberCreateVM
	{
        public VillaNumberCreateDto VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
        public VillaNumberCreateVM()
        {
            VillaNumber = new VillaNumberCreateDto();
        }
    }
}
