using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Inquirer.Controllers;

namespace Inquirer.Models
{
    public class WebClient
    {
        private IConfiguration Configuration { get; }
        private GenericController Controller { get; }

        public WebClient(IConfiguration configuration, GenericController controller)
        {
            Configuration = configuration;
            Controller = controller;
        }

        public async Task<RequestResult<T>> Send<T>(string url, HttpMethod method, object data) where T : new()
        {
            var targetUrl = Configuration["API:URL"] + url;
            string media = "application/json";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(targetUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(media));

            HttpRequestMessage request = new HttpRequestMessage(method, targetUrl);

            string cookie = $"token={ Configuration["API:TOKEN"] };";
            if (Controller.CurrentUser != null)
            {
                cookie += $" user-id={ Controller.CurrentUser.Id };";
            }
            request.Headers.Add("Cookie", cookie);
            var agents = Controller.HttpContext.Request.Headers["User-Agent"];
            if (agents.Count > 0)
            {
                request.Headers.Add("User-Agent", agents[0]);
            }
            request.Headers.Add(
                "X-Forwarded-For",
                Controller.HttpContext.Connection.RemoteIpAddress + " / " + Controller.HttpContext.Connection.LocalIpAddress
            );

            if (method != HttpMethod.Get)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, media);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            RequestResult<T> result = new RequestResult<T>
            {
                IsSuccessfull = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode
            };
            if (response.IsSuccessStatusCode)
            {
                result.Content = JsonConvert.DeserializeObject<T>(responseContent);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                result.Exception = responseContent;
            }
            else
            {
                result.Errors = JsonConvert.DeserializeObject<List<object>>(responseContent);
            }

            return result;
        }

        public async Task<RequestResult<object>> Send(string url, HttpMethod method, object data)
        {
            return await Send<object>(url, method, data);
        }

        public async Task<RequestResult<T>> Get<T>(string url, object data) where T : new()
        {
            return await Send<T>(url, HttpMethod.Get, data);
        }

        public async Task<RequestResult<T>> Post<T>(string url, object data) where T : new()
        {
            return await Send<T>(url, HttpMethod.Post, data);
        }

        public async Task<RequestResult<T>> Put<T>(string url, object data) where T : new()
        {
            return await Send<T>(url, HttpMethod.Put, data);
        }

        public async Task<RequestResult<T>> Delete<T>(string url, object data) where T : new()
        {
            return await Send<T>(url, HttpMethod.Delete, data);
        }

        public async Task<RequestResult<object>> Get(string url, object data)
        {
            return await Send<object>(url, HttpMethod.Get, data);
        }

        public async Task<RequestResult<object>> Post(string url, object data)
        {
            return await Send<object>(url, HttpMethod.Post, data);
        }

        public async Task<RequestResult<object>> Put(string url, object data)
        {
            return await Send<object>(url, HttpMethod.Put, data);
        }

        public async Task<RequestResult<object>> Delete(string url, object data)
        {
            return await Send<object>(url, HttpMethod.Delete, data);
        }
    }

    public class RequestResult<T> where T : new()
    {
        public int StatusCode { get; set; }
        public bool IsSuccessfull { get; set; }
        public T Content { get; set; }
        public IEnumerable<object> Errors { get; set; }
        public string Exception { get; set; }
    }
}
