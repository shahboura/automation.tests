using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Automation.API.Tests.Resources;

namespace Automation.API.Tests
{
    public class TestRunContext
    {
        protected HttpClient Client;
        private readonly List<ICleanable> _resources = new List<ICleanable>();
        
        public TestRunContext(HttpClient client)
        {
            Client = client;
        }

        public TZ CreateResource<TZ>()
               where TZ : BaseResource
        {
            var resource = (TZ)Activator.CreateInstance(typeof(TZ), Client);
            _resources.Add(resource);

            return resource;
        }

        public virtual async Task Cleanup()
        {
            for (var i = _resources.Count - 1; i >= 0; i--)
            {
                await _resources[i].Clean();
            }
        }
    }

    public class TestRunContext<T> : TestRunContext
        where T : BaseResource
    {
        private T _resource;
        public T Resource => _resource ?? (_resource = CreateResource<T>());

        public TestRunContext(HttpClient client) : base(client)
        {
        }
    }
}
