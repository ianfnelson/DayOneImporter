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
    public void BuildText_TitleOnly_TextIsTitle()
    {
        // Arrange
        var sourceItem = new Post()
        {
            Timestamp = 1395045380L,
            Title = "foo"
        };
        
        // Act
        var actualText = _sut.BuildText(sourceItem);

        // Assert
        actualText.Should().Be("foo");
    }
    
    [TestMethod]
    public void BuildText_PostOnly_TextIsTitle()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L,
            Data = new List<PostDataItem>
            {
                new(){Post = "bar"}
            }
        };
        
        // Act
        var actualText = _sut.BuildText(sourceItem);

        // Assert
        actualText.Should().Be("bar");
    }
    
    [TestMethod]
    public void BuildText_TitleAndPost_TextIsConcatenationOfTitleAndPost()
    {
        // Arrange
        var sourceItem = new Post()
        {
            Timestamp = 1395045380L,
            Title = "foo",
            Data = new List<PostDataItem>
            {
                new(){Post = "bar"}
            }
        };
        
        // Act
        var actualText = _sut.BuildText(sourceItem);

        // Assert
        actualText.Should().Be("foo\n\nbar");
    }
    
    [TestMethod]
    public void BuildText_TitleAndAttachments_TextIsConcatenationOfTitleAndAttachments()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L,
            Title = "foo",
            Attachments = new List<Attachment>
            {
                new(){
                    Data = new List<AttachmentDataItem>
                {
                    new(){ExternalContext = new(){Url = "bar"}}
                }}
            }
        };
        
        // Act
        var actualText = _sut.BuildText(sourceItem);

        // Assert
        actualText.Should().Be("foo\n\nbar");
    }
    
    [TestMethod]
    public void BuildText_TitleAndText_TextIsConcatenationOfTitleAndTexts()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L,
            Title = "foo",
            Attachments = new List<Attachment>
            {
                new(){
                    Data = new List<AttachmentDataItem>
                    {
                        new(){Text = "bar"},
                        new(){Text = "wibble"}
                    }}
            }
        };
        
        // Act
        var actualText = _sut.BuildText(sourceItem);

        // Assert
        actualText.Should().Be("foo\n\nbar\n\nwibble");
    }

    [TestMethod]
    public void BuildText_WithWeirdFacebookEncoding_WeirdEncodingIsFixed()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L,
            Data = new List<PostDataItem>
            {
                new(){Post = "Just when I thought I\u00e2\u0080\u0099d seen it all, I encounter an ASP.NET web site project containing a mixture of C# and VB pages! It\u00e2\u0080\u0099s unnatural!"}
            }
        };
        
        // Act
        var actualText = _sut.BuildText(sourceItem);

        // Assert
        actualText.Should().Be("Just when I thought I’d seen it all, I encounter an ASP.NET web site project containing a mixture of C# and VB pages! It’s unnatural!");
    }

    [TestMethod]
    public void BuildLocation_HasPlace_MapsPlaceToLocation()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L,
            Title = "foo",
            Attachments = new List<Attachment>
            {
                new(){
                    Data = new List<AttachmentDataItem>
                    {
                        new(){Text = "bar"},
                        new(){Place = new Place
                            {
                                Name = "Hull Dock",
                                Coordinate = new Coordinate
                                {
                                    Latitude = 53.739455D,
                                    Longitude = -0.27011454105377D
                                }
                            }
                        }
                    }}
            }
        };
        
        // Act
        var location = _sut.BuildLocation(sourceItem);

        // Assert
        location.Longitude.Should().BeApproximately(-0.27011D, 0.00001D);
        location.Latitude.Should().BeApproximately(53.739455D, 0.00001D);
        location.PlaceName.Should().Be("Hull Dock");
    }

    [TestMethod]
    public void BuildTags_HasNullTags_MapsTagsToEmptyList()
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
        var actualTags = _sut.BuildTags(sourceItem);

        // Assert
        actualTags.Should().BeEmpty();
    }
    
    [TestMethod]
    public void BuildTags_HasTags_MapsTags()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L, 
            Data = new List<PostDataItem>
            {
                new(){Post = "foo"}
            },
            Tags = new List<Tag>()
            {
                new(){Name = "Jocelyn Nelson"},
                new(){Name = "Isla Nelson"}
            }
        };

        // Act
        var actualTags = _sut.BuildTags(sourceItem);

        // Assert
        actualTags.Should().BeEquivalentTo("Jocelyn Nelson", "Isla Nelson");
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
    
    [TestMethod]
    public void Map_03WithUrl()
    {
        // Arrange
        const string fileName = "03_withUrl.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2007, 7, 21, 23, 6, 21);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2007, 7, 21, 23, 6, 21);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Ian Nelson shared a link.\n\nhttp://del.icio.us/ianfnelson");
    }
    
    [TestMethod]
    public void Map_04WithText()
    {
        // Arrange
        const string fileName = "04_withText.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2013, 12, 20, 9, 31, 46);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2013, 12, 20, 9, 31, 46);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Ian Nelson shared a link.\n\nJulie London\n\nI'd Like You For Christmas - Remastered\n\nChristmas Classics");
    }
    
    [TestMethod]
    public void Map_05WithPlace()
    {
        // Arrange
        const string fileName = "05_withPlace.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2011, 6, 12, 18, 40, 46);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2011, 6, 12, 18, 40, 46);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Ian Nelson was with Jocelyn Nelson at Hull Dock.");
        entry.Location.Longitude.Should().BeApproximately(-0.27011D, 0.00001D);
        entry.Location.Latitude.Should().BeApproximately(53.739455D, 0.00001D);
        entry.Location.PlaceName.Should().Be("Hull Dock");
    }
    
    [TestMethod]
    public void Map_06WithWeirdFacebookEncoding()
    {
        // Arrange
        const string fileName = "06_withWeirdFacebookEncoding.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2008, 11, 20, 11, 59, 11);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2008, 11, 20, 11, 59, 11);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Just when I thought I’d seen it all, I encounter an ASP.NET web site project containing a mixture of C# and VB pages! It’s unnatural!");
    }
    
    [TestMethod]
    public void Map_07WithTags()
    {
        // Arrange
        const string fileName = "07_withTags.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2017, 2, 15, 12, 33, 9);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2017, 2, 15, 12, 33, 9);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        entry.TimeZone.Should().Be(@"Europe/London");
        entry.Tags.Should()
            .BeEquivalentTo("Colin Lowe", "Sion Harrison", "Peter Windridge-France", "Rosie Middleton Jones");
    }

    private Entry MapPostFromFile(string fileName)
    {
        using FileStream openStream = File.OpenRead("Facebook/" + fileName);
        var post = JsonSerializer.Deserialize<Post>(openStream);

        var entry = _sut.Map(post);

        return entry;
    }
}