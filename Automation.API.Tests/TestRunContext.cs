using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Automation.API.Tests.PageObjects;

namespace Automation.API.Tests
{
    public class TestRunContext
    {
        protected HttpClient Client;
        private readonly List<ICleanable> _pageObjects = new List<ICleanable>();
        
        public TestRunContext(HttpClient client)
        {
            Client = client;
        }

        public TZ CreatePageObject<TZ>()
               where TZ : BasePageObject
        {
            var pageObject = (TZ)Activator.CreateInstance(typeof(TZ), Client);
            _pageObjects.Add(pageObject);

            return pageObject;
        }

        public virtual async Task Cleanup()
        {
            for (var i = _pageObjects.Count - 1; i >= 0; i--)
            {
                await _pageObjects[i].Clean();
            }
        }
    }

    public class TestRunContext<T> : TestRunContext
        where T : BasePageObject
    {
        private T _pageObject;
        public T PageObject => _pageObject ?? (_pageObject = CreatePageObject<T>());

        public TestRunContext(HttpClient client) : base(client)
        {
        }
    }
}
