using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.API.Tests
{
    public abstract class BaseApiTest : IDisposable
    {
        protected HttpClient Client;
        private readonly string _baseUrl = "http://localhost:9000/api/";
        public abstract string ResourcePath { get; }

        protected BaseApiTest()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };

            Client.AcceptJson();
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
        }

        public async Task<HttpResponseMessage> Get()
        {
            return await Client.GetAsync(ResourcePath);
        }

        public async Task<T> Get<T>()
        {
            return await Get<T>(ResourcePath);
        }

        public async Task<HttpResponseMessage> GetById(string id)
        {
            return await Client.GetAsync($"{ResourcePath}/{id}");
        }

        public async Task<T> GetById<T>(string id)
        {
            return await Get<T>($"{ResourcePath}/{id}");
        }

        public async Task<T> Get<T>(string path)
        {
            var response = await Client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }

            return await response.Content.ReadAsAsync<T>();
        }

        public async Task<HttpResponseMessage> Post<T>(T entity)
        {
            return await Client.PostAsJsonAsync(ResourcePath, entity);
        }

        public async Task<HttpResponseMessage> Delete(string id)
        {
            return await Client.DeleteAsync($"{ResourcePath}/{id}");
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
