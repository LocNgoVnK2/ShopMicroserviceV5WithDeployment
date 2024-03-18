namespace Ordering.Application.IntegrationTests
{
    [CollectionDefinition(ApplicationTestFixture.ApplicationTestFixtureCollection)]
    public class ApplicationTestCollection : ICollectionFixture<ApplicationTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
