using System.Text.Json;
using DayOneImporter.Importers.Facebook;
using DayOneImporter.Importers.Facebook.Model;
using DayOneImporter.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayOneImporterTests.Importers.Facebook;

[TestClass]
public class FacebookMapperTests
{
    [TestInitialize]
    public void Initialize()
    {
        _sut = new FacebookMapper();
    }

    private FacebookMapper _sut = null!;

    [TestMethod]
    public void BuildCreationDate_MapsFromTimestamp()
    {
        // Arrange
        var sourceItem = new Post { Timestamp = 1394389959L };

        // Act
        var actualCreationDate = FacebookMapper.BuildCreationDate(sourceItem);

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
        var actualModifiedDate = FacebookMapper.BuildModifiedDate(sourceItem);

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
        var actualModifiedDate = FacebookMapper.BuildModifiedDate(sourceItem);

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
        var actualText = FacebookMapper.BuildText(sourceItem);

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
        var actualText = FacebookMapper.BuildText(sourceItem);

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
        var actualText = FacebookMapper.BuildText(sourceItem);

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
        var actualText = FacebookMapper.BuildText(sourceItem);

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
        var actualText = FacebookMapper.BuildText(sourceItem);

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
        var actualText = FacebookMapper.BuildText(sourceItem);

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
        var location = FacebookMapper.BuildLocation(sourceItem);

        // Assert
        location?.Longitude.Should().BeApproximately(-0.27011D, 0.00001D);
        location?.Latitude.Should().BeApproximately(53.739455D, 0.00001D);
        location?.PlaceName.Should().Be("Hull Dock");
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
        var actualTags = FacebookMapper.BuildTags(sourceItem);

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
        var actualTags = FacebookMapper.BuildTags(sourceItem);

        // Assert
        actualTags.Should().BeEquivalentTo("Jocelyn Nelson", "Isla Nelson");
    }
    
    [TestMethod]
    public void BuildPhotos_HasPhotos_MapsPhotos()
    {
        // Arrange
        var sourceItem = new Post
        {
            Timestamp = 1395045380L, 
            Data = new List<PostDataItem>
            {
                new(){Post = "foo"}
            },
            Attachments = new List<Attachment>()
            {
                new Attachment(){ Data = new List<AttachmentDataItem>()
                {
                    new AttachmentDataItem(){ FbMedia = new FbMedia()
                    {
                        Uri = "one.jpg"
                    }},
                    new AttachmentDataItem(){FbMedia = new FbMedia()
                    {
                        Uri="two.jpg"
                    }},
                    new AttachmentDataItem(){FbMedia = new FbMedia()
                    {
                        Uri="three.jpg"
                    }},
                }}
            }
        };

        // Act
        var actualPhotos = FacebookMapper.BuildPhotos(sourceItem, "FaceBook/photos");

        // Assert
        var expectedPhotos = new List<Media>
        {
            new()
            {
                SourceLocation = "one.jpg",
                Md5 = "d00611d46ab66a108b1174a476ed36d4"
            },
            new() 
            {
                SourceLocation = "two.jpg",
                Md5 = "99b7634cc849e1510ce4ad7182c890eb"
            },
            new()
            {
                SourceLocation = "three.jpg",
                Md5 = "631b9ad973e16cfc7b74b3bb907e2ec9"
            }
        };
        actualPhotos.Should().BeEquivalentTo(expectedPhotos);
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
        Entry.TimeZone.Should().Be(@"Europe/London");
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
        Entry.TimeZone.Should().Be(@"Europe/London");
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
        Entry.TimeZone.Should().Be(@"Europe/London");
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
        Entry.TimeZone.Should().Be(@"Europe/London");
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
        Entry.TimeZone.Should().Be(@"Europe/London");
        entry.Text.Should().Be("Ian Nelson was with Jocelyn Nelson at Hull Dock.");
        entry.Location?.Longitude.Should().BeApproximately(-0.27011D, 0.00001D);
        entry.Location?.Latitude.Should().BeApproximately(53.739455D, 0.00001D);
        entry.Location?.PlaceName.Should().Be("Hull Dock");
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
        Entry.TimeZone.Should().Be(@"Europe/London");
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
        Entry.TimeZone.Should().Be(@"Europe/London");
        entry.Tags.Should()
            .BeEquivalentTo("Colin Lowe", "Sion Harrison", "Peter Windridge-France", "Rosie Middleton Jones");
    }
    
    [TestMethod]
    public void Map_08WithPhotos()
    {
        // Arrange
        const string fileName = "08_withPhotos.json";
        
        // Act
        var entry = MapPostFromFile(fileName);
        
        // Assert
        var expectedCreationDate = new DateTime(2018, 10, 14, 11, 40, 22);
        entry.CreationDate.Should().Be(new DateTimeOffset(expectedCreationDate));
        var expectedModifiedDate = new DateTime(2018, 10, 14, 11, 40, 22);
        entry.ModifiedDate.Should().Be(new DateTimeOffset(expectedModifiedDate));
        Entry.TimeZone.Should().Be(@"Europe/London");
        var expectedPhotos = new List<Media>
        {
            new()
            {
                SourceLocation = "one.jpg",
                Md5 = "d00611d46ab66a108b1174a476ed36d4"
            },
            new() 
            {
                SourceLocation = "two.jpg",
                Md5 = "99b7634cc849e1510ce4ad7182c890eb"
            },
            new()
            {
                SourceLocation = "three.jpg",
                Md5 = "631b9ad973e16cfc7b74b3bb907e2ec9"
            }
        };
        entry.Photos.Should().BeEquivalentTo(expectedPhotos);
    }

    private Entry MapPostFromFile(string fileName)
    {
        using var openStream = File.OpenRead("Facebook/" + fileName);
        var post = JsonSerializer.Deserialize<Post>(openStream);

        var entry = _sut.Map(post ?? throw new InvalidOperationException("Could not deserialize file to Post"), "Facebook/photos");

        return entry;
    }
}