using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using Firestorm.Engine.Deferring;
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

            var repo = new NavigationCollectionRepository<Author, ICollection<Book>, Book>(new AlreadyLoadedItem<Author>(author, null), a => a.Books);

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

            var repo = new NavigationCollectionRepository<Author, ICollection<Book>, Book>(new AlreadyLoadedItem<Author>(author, null), a => a.Books);

            repo.MarkDeleted(testBook);
            
            Assert.Equal(0, author.Books.Count);
        }

        [Fact]
        public void AttachItem_EmptyCollection_AddsItem()
        {
            var books = new List<Book>();
            var author = new Author
            {
                Books = books
            };

            var repo = new NavigationCollectionRepository<Author, ICollection<Book>, Book>(new AlreadyLoadedItem<Author>(author, null), a => a.Books);

            var book = repo.CreateAndAttachItem();
            
            Assert.Contains(book, books);
        }

        [Fact]
        public void AttachItem_NullCollection_ThrowsNotSupported()
        {
            var author = new Author
            {
                Books = null
            };

            var repo = new NavigationCollectionRepository<Author, ICollection<Book>, Book>(new AlreadyLoadedItem<Author>(author, null), a => a.Books);

            Assert.Throws<NotSupportedException>(delegate
            {
                var book = repo.CreateAndAttachItem();
            });
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
