using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Automation.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.API.Tests
{
    [TestClass]
    public class BookTests : BaseApiTest
    {
        public override string ResourcePath => "books";

        [TestMethod]
        public async Task ReturnsListOfBooksGivenDefaultAddress()
        {
            var response = await Get();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);

            var books = await response.Content.ReadAsAsync<IEnumerable<Book>>();
            Assert.IsNotNull(books);
            Assert.IsTrue(books.Any());
        }

        [TestMethod]
        public async Task RetunsCorrectBookGivenId()
        {
            var books = await Get<IEnumerable<Book>>();
            var expectedBook = books.Last();

            var response = await GetById(expectedBook.Id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);

            var bookResponse = await response.Content.ReadAsAsync<Book>();
            Assert.IsNotNull(bookResponse);
            Assert.AreEqual(expectedBook.Id, bookResponse.Id);
            Assert.AreEqual(expectedBook.Title, bookResponse.Title);
        }

        [TestMethod]
        public async Task Returns400TitleRequiredGivenMissingBookTitle()
        {
            var response = await Post(new Book());
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var message = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Title is required", message);
        }

        [TestMethod]
        public async Task ReturnsNewlyInsertedBookGivenBook()
        {
            var book = new Book
            {
                Author = "Test Author",
                Genre = "Test Genre",
                Read = true,
                Title = "Test Title"
            };

            var response = await Post(book);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);

            var createdBook = await response.Content.ReadAsAsync<Book>();
            Assert.IsNotNull(createdBook.Id);
            Assert.AreEqual(book.Author, createdBook.Author);
            Assert.AreEqual(book.Read, createdBook.Read);

            var deleteResponse = await Delete(createdBook.Id);
            Assert.IsTrue(deleteResponse.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
