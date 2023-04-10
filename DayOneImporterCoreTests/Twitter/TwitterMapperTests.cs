using DayOneImporterCore.Twitter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayOneImporterCoreTests.Twitter;

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
        var actualCreationDate = _sut.BuildTweetDate(sourceItem);
        
        // Assert
        var expectedCreationDate = new DateTime(2007, 4, 6, 7, 23, 51);
        actualCreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
    }
}