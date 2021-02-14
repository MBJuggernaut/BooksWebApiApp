using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebApplication;
using WebApplication.Controllers;
using WebApplication.Models;

namespace WebApplicationNUnitTestProject
{
    [TestFixture]
    public class BooksControllerTests
    {
        private TransactionScope transactionScope;
        private BooksController controller;
        private BooksDataContext context;

        [SetUp]
        public void Init()
        {
            var x = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).Build();
            context = (BooksDataContext)x.Services.GetService(typeof(BooksDataContext));

            controller = new BooksController(context);

            transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TearDown]
        public void CleanUp()
        {
            transactionScope.Dispose();
        }

        [Test]
        public async Task GetBooksTest()
        {
            var result = await controller.GetBooks();

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetBookTest()
        {
            var result = await controller.GetBook(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value.Id, 1);
        }

        [Test]
        public async Task PutBook()
        {
            int booksCountBefore = context.Books.Select(x => x).Count();
            var bookToPut = new Book() { Id = 3, Name = "Wild Cards", Authors = new List<Author>() { new Author() { Name = "George R.R. Martin" } } };
            var result = await controller.PutBook(3, bookToPut);
            int booksCountAfter = context.Books.Select(x => x).Count();
            var bookToFind = await context.Books.FirstOrDefaultAsync(i => i.Id == 3);

            Assert.IsNotNull(bookToFind);
            Assert.AreEqual(bookToFind.Id, 3);
            Assert.AreEqual(bookToFind.Name, bookToPut.Name);
            Assert.AreEqual(bookToFind.Authors, bookToPut.Authors);
            Assert.AreEqual(booksCountBefore, booksCountAfter);
        }

        [Test]
        public async Task PostBook()
        {
            int booksCountBefore = context.Books.Select(x => x).Count();
            var bookToPost = new Book() { Name = "Fahrenheit 451", Authors = new List<Author>() { new Author() { Name = "Ray Bradbury" } } };
            var result = await controller.PostBook(bookToPost);
            int booksCountAfter = context.Books.Select(x => x).Count();
            bool exists = context.Books.Any(b => b.Name == bookToPost.Name);

            Assert.AreEqual(booksCountBefore, booksCountAfter - 1);
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task DeleteBook()
        {
            int booksCountBefore = context.Books.Select(x => x).Count();
            int bookToDeleteId = 1;
            var result = await controller.DeleteBook(bookToDeleteId);
            int booksCountAfter = context.Books.Select(x => x).Count();
            bool exists = context.Books.Any(b => b.Id == bookToDeleteId);

            Assert.AreEqual(booksCountBefore, booksCountAfter + 1);
            Assert.IsFalse(exists);
        }
    }
}
