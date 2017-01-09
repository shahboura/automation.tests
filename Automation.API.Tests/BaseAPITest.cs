using System;
using System.Net.Http;
using System.Threading.Tasks;
using Automation.API.Tests.PageObjects;

namespace Automation.API.Tests
{
    public abstract class BaseApiTest<T>
        where T : BasePageObject
    {
        protected readonly string BaseUrl = "http://localhost:9000/";
        protected HttpClient Client;
         
        protected BaseApiTest()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            Client.AcceptJson();
        }
        
        protected async Task Run(Func<TestRunContext<T>, Task> execution)
        {
            var runContext = new TestRunContext<T>(Client);
            try
            {
                await execution(runContext);
            }
            finally
            {
                await TestCleanup(runContext);
            }
        }

        protected virtual async Task TestCleanup(TestRunContext testRunContext)
        {
            await testRunContext.Cleanup();
        }
    }
}
