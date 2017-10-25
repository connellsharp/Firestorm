using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Additives;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Tests.Integration.Data.Base.Models;

namespace Firestorm.Tests.Integration.Data.Base
{
    public class TestEngineContext : IEngineContext<Artist>
    {
        public TestEngineContext(IDataTransaction transaction, IEngineRepository<Artist> repository)
        {
            Transaction = transaction;
            Repository = repository;
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<Artist> Repository { get; }

        public IIdentifierProvider<Artist> Identifiers { get; } = new SingleIdentifierProvider<Artist>(new ExpressionIdentifierInfo<Artist,int>(a => a.ArtistID));

        public IFieldProvider<Artist> Fields { get; } = new ExpressionListFieldProvider<Artist>
        {
            { "id", a => a.ArtistID },
            { "name", a => a.Name },
        };

        public IAuthorizationChecker<Artist> AuthorizationChecker { get; } = new AllowAllAuthorizationChecker<Artist>();
    }
}