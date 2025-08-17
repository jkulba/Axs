using Domain.Common;
using Xunit;

namespace Domain.Tests.Common;

public class ObjectTypeTests
{
    [Fact]
    public void All_Object_Types_Should_Have_Unique_Values()
    {
        // Arrange
        var allTypes = ObjectType.List;

        // Act
        var values = allTypes.Select(x => x.Value).ToList();
        var distinctValues = values.Distinct().ToList();

        // Assert
        Assert.Equal(values.Count, distinctValues.Count);
    }

    [Fact]
    public void All_Object_Types_Should_Have_Unique_Names()
    {
        // Arrange
        var allTypes = ObjectType.List;

        // Act
        var names = allTypes.Select(x => x.Name).ToList();
        var distinctNames = names.Distinct().ToList();

        // Assert
        Assert.Equal(names.Count, distinctNames.Count);
    }

    [Fact]
    public void All_Object_Types_Should_Have_Unique_Display_Names()
    {
        // Arrange
        var allTypes = ObjectType.List;

        // Act
        var displayNames = allTypes.Select(x => x.DisplayName).ToList();
        var distinctDisplayNames = displayNames.Distinct().ToList();

        // Assert
        Assert.Equal(displayNames.Count, distinctDisplayNames.Count);
    }

    [Fact]
    public void All_Should_Have_Correct_Properties()
    {
        // Act & Assert
        Assert.Equal("All", ObjectType.All.Name);
        Assert.Equal("ALL", ObjectType.All.DisplayName);
        Assert.Equal(0, ObjectType.All.Value);
    }

    [Fact]
    public void RrdAssembly_Should_Have_Correct_Properties()
    {
        // Act & Assert
        Assert.Equal("RrdAssembly", ObjectType.RrdAssembly.Name);
        Assert.Equal("rrd_assembly", ObjectType.RrdAssembly.DisplayName);
        Assert.Equal(1, ObjectType.RrdAssembly.Value);
    }

    [Fact]
    public void PdmsBaseDoc_Should_Have_Correct_Properties()
    {
        // Act & Assert
        Assert.Equal("PdmsBaseDoc", ObjectType.PdmsBaseDoc.Name);
        Assert.Equal("pdms_base_doc", ObjectType.PdmsBaseDoc.DisplayName);
        Assert.Equal(176, ObjectType.PdmsBaseDoc.Value);
    }

    [Fact]
    public void RrdJobFolder_Should_Have_Correct_Properties()
    {
        // Act & Assert
        Assert.Equal("RrdJobFolder", ObjectType.RrdJobFolder.Name);
        Assert.Equal("rrd_job_folder", ObjectType.RrdJobFolder.DisplayName);
        Assert.Equal(241, ObjectType.RrdJobFolder.Value);
    }

    [Fact]
    public void FromDisplayName_Should_Return_Correct_ObjectType_Case_Insensitive()
    {
        // Act
        var result1 = ObjectType.FromDisplayName("ALL");
        var result2 = ObjectType.FromDisplayName("all");
        var result3 = ObjectType.FromDisplayName("All");

        // Assert
        Assert.Equal(ObjectType.All, result1);
        Assert.Equal(ObjectType.All, result2);
        Assert.Equal(ObjectType.All, result3);
    }

    [Fact]
    public void FromDisplayName_Should_Return_Correct_ObjectType_For_Rrd_Types()
    {
        // Act
        var assembly = ObjectType.FromDisplayName("rrd_assembly");
        var browser = ObjectType.FromDisplayName("rrd_browser");
        var control = ObjectType.FromDisplayName("rrd_control");

        // Assert
        Assert.Equal(ObjectType.RrdAssembly, assembly);
        Assert.Equal(ObjectType.RrdBrowser, browser);
        Assert.Equal(ObjectType.RrdControl, control);
    }

    [Fact]
    public void FromDisplayName_Should_Return_Correct_ObjectType_For_Pdms_Types()
    {
        // Act
        var baseDoc = ObjectType.FromDisplayName("pdms_base_doc");
        var rendition = ObjectType.FromDisplayName("pdms_rendition");

        // Assert
        Assert.Equal(ObjectType.PdmsBaseDoc, baseDoc);
        Assert.Equal(ObjectType.PdmsRendition, rendition);
    }

    [Fact]
    public void FromDisplayName_Should_Throw_For_Invalid_Display_Name()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ObjectType.FromDisplayName("invalid_display_name"));

        Assert.Contains("invalid_display_name", exception.Message);
        Assert.Equal("displayName", exception.ParamName);
    }

    [Fact]
    public void FromDisplayName_Should_Throw_For_Null_Display_Name()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            ObjectType.FromDisplayName(null!));
    }

    [Fact]
    public void FromDisplayName_Should_Throw_For_Empty_Display_Name()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            ObjectType.FromDisplayName(string.Empty));
    }

    [Fact]
    public void SmartEnum_FromValue_Should_Work()
    {
        // Act
        var all = ObjectType.FromValue(0);
        var assembly = ObjectType.FromValue(1);
        var pdmsBaseDoc = ObjectType.FromValue(176);

        // Assert
        Assert.Equal(ObjectType.All, all);
        Assert.Equal(ObjectType.RrdAssembly, assembly);
        Assert.Equal(ObjectType.PdmsBaseDoc, pdmsBaseDoc);
    }

    [Fact]
    public void SmartEnum_FromName_Should_Work()
    {
        // Act
        var all = ObjectType.FromName("All");
        var assembly = ObjectType.FromName("RrdAssembly");
        var pdmsBaseDoc = ObjectType.FromName("PdmsBaseDoc");

        // Assert
        Assert.Equal(ObjectType.All, all);
        Assert.Equal(ObjectType.RrdAssembly, assembly);
        Assert.Equal(ObjectType.PdmsBaseDoc, pdmsBaseDoc);
    }

    [Theory]
    [InlineData("All", "ALL", 0)]
    [InlineData("RrdAssembly", "rrd_assembly", 1)]
    [InlineData("RrdBrowser", "rrd_browser", 2)]
    [InlineData("RrdControl", "rrd_control", 3)]
    [InlineData("PdmsBaseDoc", "pdms_base_doc", 176)]
    [InlineData("PdmsRendition", "pdms_rendition", 192)]
    [InlineData("RrdJobFolder", "rrd_job_folder", 241)]
    public void ObjectType_Properties_Should_Match_Expected_Values(string expectedName, string expectedDisplayName, int expectedValue)
    {
        // Act
        var objectType = ObjectType.FromName(expectedName);

        // Assert
        Assert.Equal(expectedName, objectType.Name);
        Assert.Equal(expectedDisplayName, objectType.DisplayName);
        Assert.Equal(expectedValue, objectType.Value);
    }
}
