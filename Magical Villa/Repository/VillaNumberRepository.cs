using MagicalVilla_API.Data;
using MagicalVilla_API.Models;
using MagicalVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicalVilla_API.Repository
{
    public class VIllaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository 
    {
        private readonly ApplicationDbContext _db;

        public VIllaNumberRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> CreateAsync(VillaNumber entity)
        {
            entity.CreatedDate = DateTime.Now;
            await dbSet.AddAsync(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
