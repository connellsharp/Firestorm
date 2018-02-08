using System;
using Firestorm.EntityFrameworkCore2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Extensions.AspNetCore
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