using System.Text.Json;
using DayOneImporterCore;
using DayOneImporterCore.Facebook;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayOneImporterCoreTests.Facebook;

[TestClass]
public class FacebookMapperTests
{
    [TestInitialize]
    public void Initialize()
    {
        _sut = new FacebookMapper();
    }

    private FacebookMapper _sut;

    [TestMethod]
    public void BuildCreationDate_MapsFromTimestamp()
    {
        // Arrange
        var sourceItem = new Post { Timestamp = 1394389959L };

        // Act
        var actualCreationDate = _sut.BuildCreationDate(sourceItem);

        // Assert
        var expectedCreationDate = new DateTime(2014, 3, 9, 18, 32, 39);
        actualCreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
    }
    
    [TestMethod]
    public void BuildModifiedDate_UpdateTimestampExists_MapsFromUpdateTimestamp()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L, 
            Data = new List<PostDataItem>
            {
                new(){Post = "foo"},
                new(){UpdateTimestamp = 1395045893L}
            }
        };

        // Act
        var actualModifiedDate = _sut.BuildModifiedDate(sourceItem);

        // Assert
        var expectedModifiedDate = new DateTime(2014, 3, 17, 08, 44, 53);
        actualModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
    }
    
    [TestMethod]
    public void BuildModifiedDate_NoUpdateTimestampExists_MapsFromTimestamp()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L, 
            Data = new List<PostDataItem>
            {
                new(){Post = "foo"}
            }
        };

        // Act
        var actualModifiedDate = _sut.BuildModifiedDate(sourceItem);

        // Assert
        var expectedModifiedDate = new DateTime(2014, 3, 17, 08, 36, 20);
        actualModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
    }
    
    [TestMethod]
    public void Map_01SimpleStatusUpdate()
    {
        // Arrange
        const string fileName = "01_simple.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2007, 6, 16, 9, 35, 20);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Ian Nelson updated his status.\n\nat home");
    }
    
    [TestMethod]
    public void Map_02WithUpdateTimestamp()
    {
        // Arrange
        const string fileName = "02_withUpdateTimestamp.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2007, 6, 16, 9, 35, 20);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2007, 6, 16, 9, 36, 20);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Ian Nelson updated his status.\n\nat home");
    }

    private Entry MapPostFromFile(string fileName)
    {
        using FileStream openStream = File.OpenRead("Facebook/" + fileName);
        var post = JsonSerializer.Deserialize<Post>(openStream);

        var entry = _sut.Map(post);

        return entry;
    }
}