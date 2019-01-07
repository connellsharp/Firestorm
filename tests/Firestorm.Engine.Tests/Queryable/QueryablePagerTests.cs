using System.Linq;
using AutoFixture;
using Firestorm.Engine.Queryable;
using Xunit;

namespace Firestorm.Engine.Tests.Queryable
{
    public class QueryablePagerTests
    {
        private class Person {  public string Name { get; set; } }

        [Fact]
        public void PageSize_InstructionWithSize_Correct()
        {
            var pager = new QueryablePager<Person>(new PageInstruction { Size = 50 });
            Assert.Equal(50, pager.PageSize);
        }

        [Fact]
        public void PageSize_InstructionNoSize_GivesDefault()
        {
            var pager = new QueryablePager<Person>(new PageInstruction { Size = null });
            Assert.Equal(100, pager.PageSize);
        }

        [Theory]
        [InlineData(10, 50, 0, 10)]
        [InlineData(300, 50, 0, 51)]
        [InlineData(300, 200, 200, 100)]
        [InlineData(300, 200, -200, 100)]
        [InlineData(100, 10, -20, 11)]
        [InlineData(50, 50, 50, 0)]
        [InlineData(50, 50, -20, 30)]
        [InlineData(50, 50, 500, 0)]
        [InlineData(50, 50, -500, 0)]
        public void ApplyPagination_QueryableSizeAndOffset_ExpectedCount(int originalSize, int instructionSize, int instructionOffset, int expectedCount)
        {
            var fixture = new Fixture();
            var bigQueryable = fixture.CreateMany<Person>(originalSize).AsQueryable();

            var pager = new QueryablePager<Person>(new PageInstruction { Size = instructionSize, Offset = instructionOffset });
            var pagedQueryable = pager.ApplyPagination(bigQueryable);

            Assert.Equal(expectedCount, pagedQueryable.Count());
        }

        [Theory]
        [InlineData(10, 50, 1, 10)]
        [InlineData(300, 50, 1, 51)]
        [InlineData(300, 200, 2, 100)]
        [InlineData(50, 50, 2, 0)]
        [InlineData(300, 200, -1, 201)]
        [InlineData(500, 200, -1, 201)]
        public void ApplyPagination_QueryableSizeAndPageNum_ExpectedCount(int originalSize, int instructionSize, int instructionPageNum, int expectedCount)
        {
            var fixture = new Fixture();
            var bigQueryable = fixture.CreateMany<Person>(originalSize).AsQueryable();

            var pager = new QueryablePager<Person>(new PageInstruction { Size = instructionSize, PageNumber = instructionPageNum });
            var pagedQueryable = pager.ApplyPagination(bigQueryable);

            Assert.Equal(expectedCount, pagedQueryable.Count());
        }
    }
}
