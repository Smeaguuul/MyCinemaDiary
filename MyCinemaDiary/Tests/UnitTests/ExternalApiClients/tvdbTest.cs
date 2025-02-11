using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCinemaDiary.Infrastructure.ExternalApiClients;

namespace MyCinemaDiary;

[TestClass]
public class tvdbTest
{
    [TestMethod]
    public async Task Search(TheTvDbAPI theTvDbAPI)
    {
        // Arrange
        var movieTitle = "Lord of the Rings";
        var movieCount = 2;
        var tvdb = theTvDbAPI;
        tvdb.initialize();

        // Act
        var result = await tvdb.Search(movieTitle, movieCount);

        // Assert
        Assert.AreEqual(result.RootElement.GetProperty("data").GetArrayLength(), 2);
    }
}
