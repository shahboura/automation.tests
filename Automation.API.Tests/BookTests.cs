using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Automation.API.Tests.Resources;
using Automation.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.API.Tests
{
    [TestClass]
    public class BookTests : BaseApiTest<BookResource>
    {
        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Get"), TestCategory("Book")]
        public async Task GetReturns404NotFoundGivenInvalidBookId()
        {
            await Run(async t =>
            {
                var deleteResponse = await t.Resource.GetByIdRaw("Dummy id");
                var message = await deleteResponse.Content.ReadAsStringAsync();

                Assert.AreEqual("no book found", message);
                Assert.AreEqual(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            });
        }

        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Get"), TestCategory("Book")]
        public async Task GetReturnsListOfBooksGivenDefaultAddress()
        {
            await Run(async t =>
            {
                var response = await t.Resource.GetRaw();

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.IsSuccessStatusCode);

                var books = await response.Content.ReadAsAsync<IEnumerable<Book>>();
                Assert.IsNotNull(books);
                Assert.IsTrue(books.Any());
            });
        }

        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Get"), TestCategory("Book")]
        public async Task GetRetunsCorrectBookGivenId()
        {
            await Run(async t =>
            {
                var books = await t.Resource.Get();
                var expectedBook = books.Last();

                var response = await t.Resource.GetByIdRaw(expectedBook.Id);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.IsSuccessStatusCode);

                var bookResponse = await response.Content.ReadAsAsync<Book>();
                Assert.IsNotNull(bookResponse);
                Assert.AreEqual(expectedBook.Id, bookResponse.Id);
                Assert.AreEqual(expectedBook.Title, bookResponse.Title);
            });
        }

        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Post"), TestCategory("Book")]
        public async Task PostReturns400TitleRequiredGivenMissingBookTitle()
        {
            await Run(async t =>
            {
                var response = await t.Resource.PostRaw(new Book());
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

                var message = await response.Content.ReadAsStringAsync();
                Assert.AreEqual("Title is required", message);
            });
        }

        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Post"), TestCategory("Book")]
        public async Task PostReturnsNewlyInsertedBookGivenBook()
        {
            await Run(async t =>
            {
                var book = GenerateTestBook();

                var response = await t.Resource.PostRaw(book);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.IsTrue(response.IsSuccessStatusCode);

                var createdBook = await response.Content.ReadAsAsync<Book>();
                Assert.AreEqual($"{t.Resource.ResourcePath}{createdBook.Id}"
                    , response.Headers.Location.AbsolutePath);
                Assert.IsNotNull(createdBook.Id);
                Assert.AreEqual(book.Author, createdBook.Author);
                Assert.AreEqual(book.Read, createdBook.Read);
            });
        }

        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Delete"), TestCategory("Book")]
        public async Task DeleteReturns404NotFoundGivenInvalidBookId()
        {
            await Run(async t =>
            {
                var deleteResponse = await t.Resource.Delete("Dummy id");
                var message = await deleteResponse.Content.ReadAsStringAsync();

                Assert.AreEqual("no book found", message);
                Assert.AreEqual(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            });
        }

        [TestMethod]
        [TestCategory("Nightly"), TestCategory("Delete"), TestCategory("Book")]
        public async Task DeleteReturnsNoContentGivenValidBookToDelete()
        {
            await Run(async t =>
            {
                var book = GenerateTestBook();
                var createdBook = await t.Resource.Post(book);
                var deleteResponse = await t.Resource.Delete(createdBook.Id);

                Assert.IsTrue(deleteResponse.IsSuccessStatusCode);
                Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            });
        }

        private static Book GenerateTestBook()
        {
            return new Book
            {
                Author = "Test Author",
                Genre = "Test Genre",
                Read = true,
                Title = "Test Title"
            };
        }
    }
}
