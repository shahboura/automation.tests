using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automation.API.Tests.PageObjects
{
    public abstract class BasePageObject : ICleanable
    {
        protected HttpClient Client;
        public abstract string ResourcePath { get; }
        protected readonly List<string> TestData = new List<string>();

        protected BasePageObject(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            Client = client;
        }

        public async Task<HttpResponseMessage> GetRaw()
        {
            return await Get<HttpResponseMessage>(ResourcePath);
        }

        public async Task<HttpResponseMessage> GetByIdRaw(string id)
        {
            return await Client.GetAsync($"{ResourcePath}/{id}");
        }

        public async Task<HttpResponseMessage> PostRaw<T>(T entity)
        {
            var response = await Client.PostAsJsonAsync(ResourcePath, entity);

            if (response.IsSuccessStatusCode && response.Headers.Location != null)
            {
                TestData.Add(response.Headers.Location.AbsolutePath);
            }

            return response;
        }

        public async Task<HttpResponseMessage> Delete(string id)
        {
            TestData.Remove($"{ResourcePath}{id}");
            return await Client.DeleteAsync($"{ResourcePath}/{id}");
        }

        public async Task<T> Get<T>(string path)
            where T : class
        {
            var response = await Client.GetAsync(path);

            if (typeof(T) == typeof(HttpResponseMessage))
            {
                return response as T;
            }

            // Ensure correct response before trying to cast
            if (!response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }

            return await response.Content.ReadAsAsync<T>();
        }

        public async Task Clean()
        {
            for (var i = TestData.Count - 1; i >= 0; i--)
            {
                var response = await Client.DeleteAsync(TestData[i]);
                
                // Throw exception in case of not handled error
                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }

                TestData.RemoveAt(i);
            }
        }
    }

    public abstract class BasePageObject<T> : BasePageObject
        where T : class, new()
    {
        protected BasePageObject(HttpClient client) : base(client)
        {
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await Get<IEnumerable<T>>(ResourcePath);
        }

        public async Task<T> GetById(string id)
        {
            return await Get<T>($"{ResourcePath}/{id}");
        }

        public async Task<T> Post(T entity)
        {
            var response = await PostRaw(entity);
            return await response.Content.ReadAsAsync<T>();
        }
    }
}
