﻿using MagicalVilla_Web.Models.Dto;

namespace MagicalVilla_Web.Services.IServices
{
    public interface IVillaNumberService
	{
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberCreateDto dto);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDto dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
