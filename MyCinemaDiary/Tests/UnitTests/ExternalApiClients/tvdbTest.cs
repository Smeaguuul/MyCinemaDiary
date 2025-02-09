using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCinemaDiary.Domain.ExternalApiClients;

namespace MyCinemaDiary;

[TestClass]
public class tvdbTest
{
    [TestMethod]
    public async Task Search()
    {
        // Arrange
        var movieTitle = "Lord of the Rings";
        var movieCount = 2;
        var tvdb = new TheTvDbAPI();
        tvdb.initialize();

        // Act
        var result = await tvdb.Search(movieTitle, movieCount);

        // Assert
        Assert.AreEqual(result.RootElement.GetProperty("data").GetArrayLength(), 2);
    }
}
