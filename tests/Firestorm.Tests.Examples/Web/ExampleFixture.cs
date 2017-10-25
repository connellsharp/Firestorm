using System;
using System.Net.Http;
using System.Reflection;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Responses;
using Firestorm.Engine.EntityFramework;
using Firestorm.Stems;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Naming;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Tests.Examples.Data;
using Firestorm.Tests.Integration.Http.Base;

namespace Firestorm.Tests.Examples.Web
{
    public class ExampleFixture<TTest> : IDisposable
    {
        private readonly IHttpIntegrationSuite _testSuite;

        public ExampleFixture()
        {
            _testSuite = new ExampleItegrationSuite(FirestormConfig);
            _testSuite.Start();
        }

        private FirestormConfiguration FirestormConfig => new FirestormConfiguration
        {
            EndpointConfiguration = new RestEndpointConfiguration
            {
                ResponseContentGenerator = new StatusCodeResponseContentGenerator()
            },
            StartResourceFactory = new StemsStartResourceFactory
            {
                RootResourceFactory = new DataSourceRootResourceFactory
                {
                    StemTypes = typeof(TTest).GetNestedTypes(BindingFlags.Public),
                    DataSource = new EntitiesDataSource<ExampleDataContext>(),
                },
                StemConfiguration = new DefaultStemConfiguration
                {
                    NamingConventionSwitcher = new DefaultNamingConventionSwitcher(),
                    AutoPropertyMapper = new DefaultPropertyAutoMapper()
                }
            }
        };

        public HttpClient HttpClient => _testSuite.HttpClient;

        public void Dispose()
        {
            _testSuite.Dispose();
        }
    }
}