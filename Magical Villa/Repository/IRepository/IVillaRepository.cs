using MagicalVilla_API.Models;
using System.Linq.Expressions;

namespace MagicalVilla_API.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}
