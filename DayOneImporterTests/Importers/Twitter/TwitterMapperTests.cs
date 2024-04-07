using DayOneImporter.Importers.Twitter;
using DayOneImporter.Importers.Twitter.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayOneImporterTests.Importers.Twitter;

[TestClass]
public class TwitterMapperTests
{
    [TestInitialize]
    public void Initialize()
    {
        _sut = new TwitterMapper();
    }

    private TwitterMapper _sut;

    [TestMethod]
    public void BuildTweetDate_MapsFromCreatedAt()
    {
        // Arrange
        var sourceItem = new Tweet
        {
            CreatedAt = "Fri Apr 06 07:23:51 +0000 2007"
        };

        // Act
        var actualCreationDate = TwitterMapper.BuildTweetDate(sourceItem);
        
        // Assert
        var expectedCreationDate = new DateTime(2007, 4, 6, 7, 23, 51);
        actualCreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
    }
}