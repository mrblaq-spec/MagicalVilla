using MagicalVilla_Utility;
using MagicalVilla_Web.Models;
using MagicalVilla_Web.Models.Dto;
using MagicalVilla_Web.Services.IServices;

namespace MagicalVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        public readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.POST,
                Data = dto,
                Url = villaUrl + "/api/villaNumberAPI"
			});
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.DELETE,
                Url = villaUrl + "/api/villaNumberAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.GET,
                Url = villaUrl + "/api/villaNumberAPI"
			});
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.GET,
                Url = villaUrl + "/api/villaNumberAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            { 
                ApiType = SD.APIType.PUT,
                Data = dto,
                Url = villaUrl + "/api/villaNumberAPI"
			});
        }
    }
}
