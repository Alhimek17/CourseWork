using ServiceStationContracts.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace ServiceStationExecutorApp
{
    public class APIExecutor
    {
        private static readonly HttpClient _executor = new();

        public static ExecutorViewModel? Executor { get; set; } = null;

        public static void Connect(IConfiguration configuration)
        {
            _executor.BaseAddress = new Uri(configuration["IPAddress"]);
            _executor.DefaultRequestHeaders.Accept.Clear();
            _executor.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static T? GetRequest<T>(string requestUrl)
        {
            var response = _executor.GetAsync(requestUrl);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception(result);
            }
        }

        public static void PostRequest<T>(string requestUrl, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _executor.PostAsync(requestUrl, data);

            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (!response.Result.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
        }
    }
}
