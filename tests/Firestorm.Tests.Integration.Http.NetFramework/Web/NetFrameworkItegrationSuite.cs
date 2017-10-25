using System;
using System.Net.Http;
using Firestorm.Tests.Integration.Http.Base;
using Microsoft.Owin.Hosting;

namespace Firestorm.Tests.HttpWebStacks.Web
{
    internal class NetFrameworkItegrationSuite<TStartup> : IHttpIntegrationSuite
    {
        private readonly string _baseAddress;

        public NetFrameworkItegrationSuite(int localPortNumber)
            : this("http://localhost:" + localPortNumber)
        { }

        public NetFrameworkItegrationSuite(string baseAddress)
        {
            _baseAddress = baseAddress;

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        protected IDisposable WebApplication { get; private set; }

        public HttpClient HttpClient { get; private set; }

        public void Start()
        {
            WebApplication = WebApp.Start<TStartup>(_baseAddress);
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            WebApplication?.Dispose();
        }
    }
}