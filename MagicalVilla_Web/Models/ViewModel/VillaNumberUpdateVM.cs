using MagicalVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicalVilla_Web.Models.ViewModel
{
	public class VillaNumberUpdateVM
	{
        public VillaNumberUpdateDto VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
        public VillaNumberUpdateVM()
        {
            VillaNumber = new VillaNumberUpdateDto();
        }
    }
}
