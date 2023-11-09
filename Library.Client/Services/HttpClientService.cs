using Library.Client.Models;
using Library.Domain.Dtos;
using Library.Domain.Dtos.User;
using Library.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Library.Client.Services
{
    public class HttpClientService
    {
        private readonly HttpClient client;

        public HttpClientService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<ResponseData<TResponse>> SendGetRequest<TResponse>(string action, Dictionary<string, object> queryParams)
        {
            var sb = new StringBuilder("?");

            foreach (var param in queryParams)
            {
                sb.Append($"{param.Key}={param.Value.ToString()}&");
            }

            var res = await client.GetAsync($"{action}{sb.ToString()}");

            if (res.IsSuccessStatusCode)
            {
                var s = await res.Content.ReadAsStringAsync();

                return new ResponseData<TResponse> { Data = await res.Content.ReadFromJsonAsync<TResponse>() };
            }
            else
            {
                var s = await res.Content.ReadAsStringAsync();
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();

                if (String.IsNullOrEmpty(exception?.Message))
                {
                    return new ResponseData<TResponse>
                    {
                        Data = default(TResponse),
                        Excetption = new ExceptionResultDto
                        {
                            Code = (int)res.StatusCode,
                            Message = await res.Content.ReadAsStringAsync()
                        }
                    };
                }

                return new ResponseData<TResponse> { Data = default(TResponse), Excetption = exception };
            }

        }

        public async Task<ResponseData<TResponse>> SendPostRequest<TRequest, TResponse>(string action, TRequest requestData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var res = await client.PostAsync(action, content);

            var s = await content.ReadAsStringAsync();

            if (res.IsSuccessStatusCode)
            {
                return new ResponseData<TResponse> { Data = await res.Content.ReadFromJsonAsync<TResponse>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();

                if(String.IsNullOrEmpty(exception.Message))
                {
                    return new ResponseData<TResponse> 
                    { 
                        Data = default(TResponse), 
                        Excetption = new ExceptionResultDto 
                        { 
                            Code = (int) res.StatusCode,
                            Message = await res.Content.ReadAsStringAsync()
                        } };
                }

                return new ResponseData<TResponse> { Data = default(TResponse), Excetption = exception };
            }
        }

        public async Task<ResponseData<TResponse>> SendPutRequest<TRequest, TResponse>(string action, Dictionary<string, object> queryParams, TRequest requestData)
        {
            var sb = new StringBuilder("?");

            foreach (var param in queryParams)
            {
                sb.Append($"{param.Key}={param.Value.ToString()}&");
            }
            var query = sb.ToString();

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var res = await client.PutAsync($"{action}{query}", content);

            if (res.IsSuccessStatusCode)
            {
                return new ResponseData<TResponse> { Data = await res.Content.ReadFromJsonAsync<TResponse>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();

                if (String.IsNullOrEmpty(exception.Message))
                {
                    return new ResponseData<TResponse>
                    {
                        Data = default(TResponse),
                        Excetption = new ExceptionResultDto
                        {
                            Code = (int)res.StatusCode,
                            Message = await res.Content.ReadAsStringAsync()
                        }
                    };
                }

                return new ResponseData<TResponse> { Data = default(TResponse), Excetption = exception };
            }
        }

        public async Task<ResponseData<TResponse>> SendDeleteRequest<TResponse>(string action, Dictionary<string, object> queryParams)
        {
            var sb = new StringBuilder("?");

            foreach (var param in queryParams)
            {
                sb.Append($"{param.Key}={param.Value.ToString()}&");
            }

            var res = await client.DeleteAsync($"{action}{sb.ToString()}");

            if (res.IsSuccessStatusCode)
            {
                var s = await res.Content.ReadAsStringAsync();

                return new ResponseData<TResponse> { Data = await res.Content.ReadFromJsonAsync<TResponse>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();

                if (String.IsNullOrEmpty(exception?.Message))
                {
                    return new ResponseData<TResponse>
                    {
                        Data = default(TResponse),
                        Excetption = new ExceptionResultDto
                        {
                            Code = (int)res.StatusCode,
                            Message = await res.Content.ReadAsStringAsync()
                        }
                    };
                }

                return new ResponseData<TResponse> { Data = default(TResponse), Excetption = exception };
            }

        }
    }
}
