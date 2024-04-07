using DayOneImporter.Importers.Twitter;
using DayOneImporter.Importers.Twitter.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayOneImporterTests.Importers.Twitter;

[TestClass]
public class TwitterMapperTests
{
    [TestMethod]
    public void BuildTweetDate_MapsFromCreatedAt()
    {
        // Arrange
        var sourceItem = new Tweet
        {
            Id = "1234",
            FullText = "Tweet text",
            CreatedAt = "Fri Apr 06 07:23:51 +0000 2007"
        };

        // Act
        var actualCreationDate = TwitterMapper.BuildTweetDate(sourceItem);
        
        // Assert
        var expectedCreationDate = new DateTime(2007, 4, 6, 7, 23, 51);
        actualCreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
    }
}