﻿using MagicalVilla_API.Data;
using MagicalVilla_API.Models;
using MagicalVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicalVilla_API.Repository
{
    public class VIllaRepository : Repository<Villa>, IVillaRepository 
    {
        private readonly ApplicationDbContext _db;

        public VIllaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
