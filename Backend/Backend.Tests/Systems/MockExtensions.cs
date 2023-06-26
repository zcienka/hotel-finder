using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend.Tests.Systems
{
    public static class MockExtensions
    {
        public static void SetupIQueryable<T>(this Mock<DbSet<T>> mock, IQueryable<T> queryable)
            where T : class
        {
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        }
    }
}