using Microsoft.OpenApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCinemaDiary.Application;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.ExternalApiClients;

namespace MyCinemaDiary.Tests.UnitTests.Infrastructure.ExternalApiClients;

[TestClass]
public class tvdbTest
{
    private readonly TheTvDbAPI theTvDbAPI;
    public tvdbTest(TheTvDbAPI theTvDbAPI)
    {
        this.theTvDbAPI = theTvDbAPI;
    }

    [TestMethod]
    public async Task Search()
    {
        // Arrange
        var movieTitle = "Lord of the Rings";
        var movieCount = 2;
        theTvDbAPI.initialize();

        // Act
        var results = await theTvDbAPI.Search(movieTitle, movieCount);
        var movielist = results.RootElement.GetProperty("data").EnumerateArray();

 
        // Assert
        Assert.AreEqual(movielist.Count(), 2);
    }
}
