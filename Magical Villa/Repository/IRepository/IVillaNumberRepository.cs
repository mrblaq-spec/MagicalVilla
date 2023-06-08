using MagicalVilla_API.Models;
using System.Linq.Expressions;

namespace MagicalVilla_API.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> CreateAsync(VillaNumber villaNumber);
        Task<VillaNumber> UpdateAsync(VillaNumber villaNumber);
    }
}
