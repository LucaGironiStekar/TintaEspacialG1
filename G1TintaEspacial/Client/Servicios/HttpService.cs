using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace G1TintaEspacial.Client.Servicios
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient http;
        public HttpService(HttpClient http)
        {
            this.http = http;
        }

        public HttpClient Http { get; }
        public async Task<HttpRespuesta<T>> Get<T>(string url)
        {
            var response = await Http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DeserealizarRespuesta<T>(response);
                return new HttpRespuesta<T>(respuesta, false, response);

            }
            else
            {
                return new HttpRespuesta<T>(default, true, response);
            }
        }

        public async Task<HttpRespuesta<object>> Post<T>(string url, T enviar)
        {
            try
            {
                var enviarJson = JsonSerializer.Serialize(enviar);
                var enviarContent = new StringContent(enviarJson, Encoding.UTF8, "application/json");
                var respuesta = await http.PostAsync(url, enviarContent);
                return new HttpRespuesta<object>(null, !respuesta.IsSuccessStatusCode, respuesta);
            }
            catch (Exception e) { throw; }
        }

        public async Task<HttpRespuesta<object>> Put<T>(string url, T enviar)
        {
            try
            {
                var enviarJson = JsonSerializer.Serialize(enviar);
                var enviarContent = new StringContent(enviarJson, Encoding.UTF8, "application/json");
                var respuesta = await http.PutAsync(url, enviarContent);
                return new HttpRespuesta<object>(null, !respuesta.IsSuccessStatusCode, respuesta);

            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<HttpRespuesta<object>> Delete(string url)
        {
            var respuesta = await http.DeleteAsync(url);
            return new HttpRespuesta<object>(null, !respuesta.IsSuccessStatusCode, respuesta);
        }
        private async Task<T> DeserealizarRespuesta<T>(HttpResponseMessage response)
        {
            var respuestastring = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestastring, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });



        }
    }
}
