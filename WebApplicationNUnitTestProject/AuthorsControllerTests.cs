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
    class AuthorsControllerTests
    {
        private TransactionScope transactionScope;
        private AuthorsController controller;
        private BooksDataContext context;

        [SetUp]
        public void Init()
        {
            var x = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).Build();
            context = (BooksDataContext)x.Services.GetService(typeof(BooksDataContext));

            controller = new AuthorsController(context);

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
            var result = await controller.GetAuthors();

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetBookTest()
        {
            var result = await controller.GetAuthor(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value.Id, 1);
        }

        [Test]
        public async Task PutBook()
        {
            int authorsCountBefore = context.Authors.Select(x => x).Count();
            var authorToPut = new Author() { Id = 3, Name = "George R.R. Martin", Books = new List<Book>() { new Book() { Name = "Wild Cards" } } };
            var result = await controller.PutAuthor(3, authorToPut);
            int authorsCountAfter = context.Authors.Select(x => x).Count();
            var authorToFind = await context.Authors.FirstOrDefaultAsync(i => i.Id == 3);

            Assert.IsNotNull(authorToFind);
            Assert.AreEqual(authorToFind.Id, 3);
            Assert.AreEqual(authorToFind.Name, authorToPut.Name);
            Assert.AreEqual(authorToFind.Books, authorToPut.Books);
            Assert.AreEqual(authorsCountBefore, authorsCountAfter);
        }

        [Test]
        public async Task PostBook()
        {
            int authorsCountBefore = context.Authors.Select(x => x).Count();
            var authorToPost = new Author() { Name = "Ray Bradbury", Books = new List<Book>() { new Book() { Name = "Fahrenheit 451" } } };
            var result = await controller.PostAuthor(authorToPost);
            int authorsCountAfter = context.Authors.Select(x => x).Count();
            bool exists = context.Authors.Any(b => b.Name == authorToPost.Name);

            Assert.AreEqual(authorsCountBefore, authorsCountAfter - 1);
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task DeleteBook()
        {
            int authorsCountBefore = context.Authors.Select(x => x).Count();
            int authorToDeleteId = 1;
            var result = await controller.DeleteAuthor(authorToDeleteId);
            int authorsCountAfter = context.Authors.Select(x => x).Count();
            bool exists = context.Authors.Any(b => b.Id == authorToDeleteId);

            Assert.AreEqual(authorsCountBefore, authorsCountAfter + 1);
            Assert.IsFalse(exists);
        }
    }
}
