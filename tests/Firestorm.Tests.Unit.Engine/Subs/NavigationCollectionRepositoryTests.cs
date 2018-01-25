using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Repositories;
using Xunit;

namespace Firestorm.Tests.Unit.Engine.Subs
{
    public class NavigationCollectionRepositoryTests
    {
        private Fixture _fixture;

        public NavigationCollectionRepositoryTests()
        {
            _fixture = new Fixture();
        }

        private static NavigationCollectionRepository<Author, ICollection<Book>, Book> GetBooksNavRepo(Author author)
        {
            var item = new AlreadyLoadedItem<Author>(author, null);
            var tools = new SubWriterTools<Author, ICollection<Book>, Book>(a => a.Books, null, null);
            var repo = new NavigationCollectionRepository<Author, ICollection<Book>, Book>(item, tools);
            return repo;
        }

        [Fact]
        public void GetAllItemsQuery_Collection_ContainsTestData()
        {
            var testBook = new Book { Title = "Test" };

            var author = new Author
            {
                Books = new List<Book>
                {
                    testBook
                }
            };

            NavigationCollectionRepository<Author, ICollection<Book>, Book> repo = GetBooksNavRepo(author);

            var bookQuery = repo.GetAllItems().ToList();
            
            Assert.Contains(testBook, bookQuery);
        }

        [Fact]
        public void MarkDeleted_Collection_RemovesTestData()
        {
            var testBook = new Book { Title = "Test" };

            var author = new Author
            {
                Books = new List<Book>
                {
                    testBook
                }
            };

            NavigationCollectionRepository<Author, ICollection<Book>, Book> repo = GetBooksNavRepo(author);

            repo.MarkDeleted(testBook);
            
            Assert.Equal(0, author.Books.Count);
        }

        [Fact]
        public void AttachNewItem_EmptyCollection_AddsItem()
        {
            var books = new List<Book>();
            var author = new Author
            {
                Books = books
            };

            NavigationCollectionRepository<Author, ICollection<Book>, Book> repo = GetBooksNavRepo(author);

            var book = repo.CreateAndAttachItem();
            
            Assert.Contains(book, books);
        }

        [Fact]
        public void AttachNewItem_NullCollection_CreatesCollection()
        {
            var author = new Author
            {
                Books = null
            };

            NavigationCollectionRepository<Author, ICollection<Book>, Book> repo = GetBooksNavRepo(author);

            var book = repo.CreateAndAttachItem();

            Assert.NotNull(author.Books);
            Assert.Contains(book, author.Books);
        }

        public class Author
        {
            public ICollection<Book> Books { get; set; }
        }

        public class Book
        {
            public string Title { get; set; }
        }
    }
}
