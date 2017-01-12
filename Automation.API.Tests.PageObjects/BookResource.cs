using System.Net.Http;
using Automation.Models;

namespace Automation.API.Tests.Resources
{
    public class BookResource : BaseResource<Book>
    {
        public BookResource(HttpClient client) : base(client)
        {
        }

        public override string ResourcePath => "/api/books/";
    }
}
