using Mango.Web.Models;
using Mango.Web.Service.IService;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Json;


namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(requestDto.Url);
            if (requestDto.Data != null) 
            {
                message.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestDto.Data), Encoding.UTF8, "application/json");
                //message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");

            }

            HttpResponseMessage? apiResponse = null;           

            switch (requestDto.ApiType)   
            {
                case Utility.SD.ApiType.POST:
                    message.Method=HttpMethod.Post;
                    break;
                case Utility.SD.ApiType.PUT:
                    message.Method=HttpMethod.Put;
                    break;
                case Utility.SD.ApiType.DELETE:
                    message.Method=HttpMethod.Delete;
                    break;
                case Utility.SD.ApiType.GET:
                    message.Method=HttpMethod.Get;
                    break;
            }
            apiResponse = await client.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new() { IsSuccess = false , Message = "Not Found"};

                case System.Net.HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };

                case System.Net.HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Eroor" };

                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
    }
}
