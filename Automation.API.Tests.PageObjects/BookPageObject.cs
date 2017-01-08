using System.Net.Http;
using Automation.Models;

namespace Automation.API.Tests.PageObjects
{
    public class BookPageObject : BasePageObject<Book>
    {
        public BookPageObject(HttpClient client) : base(client)
        {
        }

        public override string ResourcePath => "/api/books/";
    }
}
