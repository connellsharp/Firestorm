using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.EntityFrameworkCore2
{
    internal class EntitiesContextFactory<TDbContext> : IDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FirestormEntityOptions _options;

        internal EntitiesContextFactory(IServiceProvider serviceProvider, FirestormEntityOptions options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public TDbContext Create()
        {
            var database = _serviceProvider.GetService<TDbContext>();

            if (_options.EnsureCreatedOnRequest)
                database.Database.EnsureCreated();

            return database;
        }
    }
}