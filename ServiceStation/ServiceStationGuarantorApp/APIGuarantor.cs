using Newtonsoft.Json;
using ServiceStationContracts.ViewModels;
using System.Net.Http.Headers;
using System.Text;

namespace ServiceStationGuarantorApp
{
    public class APIGuarantor
    {
        private static readonly HttpClient _guarantor = new();

        public static GuarantorViewModel? Guarantor { get; set; } = null;

        public static void Connect(IConfiguration configuration)
        {
            _guarantor.BaseAddress = new Uri(configuration["IPAddress"]);
            _guarantor.DefaultRequestHeaders.Accept.Clear();
            _guarantor.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static T? GetRequest<T>(string requestUrl)
        {
            var response = _guarantor.GetAsync(requestUrl);
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

            var response = _guarantor.PostAsync(requestUrl, data);

            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (!response.Result.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
        }
    }
}
