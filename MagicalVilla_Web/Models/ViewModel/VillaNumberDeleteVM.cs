using MagicalVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicalVilla_Web.Models.ViewModel
{
	public class VillaNumberDeleteVM
	{
        public VillaNumberDto VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
        public VillaNumberDeleteVM()
        {
            VillaNumber = new VillaNumberDto();
        }
    }
}
