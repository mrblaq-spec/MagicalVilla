using MagicalVilla_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicalVilla_Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }

        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
