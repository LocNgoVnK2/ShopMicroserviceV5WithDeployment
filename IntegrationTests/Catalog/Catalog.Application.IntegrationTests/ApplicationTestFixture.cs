using MongoDB.Driver.Core.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.IntegrationTests
{
    public class ApplicationTestFixture : IAsyncLifetime
    {
        public const string ApplicationTestFixtureCollection =
           $"Catalog.{nameof(ApplicationTestFixture)}";
        
        private readonly Feature _fakeData;
     
        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
