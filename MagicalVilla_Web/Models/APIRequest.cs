using static MagicalVilla_Utility.SD;

namespace MagicalVilla_Web.Models
{
    public class APIRequest
    {
        public APIType ApiType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
