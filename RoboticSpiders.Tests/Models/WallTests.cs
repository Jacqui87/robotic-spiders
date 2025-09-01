using RoboticSpiders.Models;

namespace RoboticSpiders.Tests.Models;

public class WallTests
{
    // ----------------------------
    // Wall creation tests
    // ----------------------------

    [Fact]
    public void Wall_CreatesSuccessfully_WithValidInput()
    {
        var wall = new Wall("5 5");

        Assert.Equal(5, wall.MaxX);
        Assert.Equal(5, wall.MaxY);
        Assert.Null(wall.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("5")]
    [InlineData("x y")]
    public void Wall_ReturnsError_WithInvalidInput(string? input)
    {
        var wall = new Wall(input);

        Assert.NotNull(wall.Error);
    }

    // ----------------------------
    // Contains tests
    // ----------------------------

    [Fact]
    public void Contains_ReturnsTrueForInsideCoordinates()
    {
        var wall = new Wall("5 5");

        Assert.True(wall.Contains(0, 0));
        Assert.True(wall.Contains(3, 4));
        Assert.True(wall.Contains(5, 5));
    }

    [Fact]
    public void Contains_ReturnsFalseForOutsideCoordinates()
    {
        var wall = new Wall("5 5");

        Assert.False(wall.Contains(-1, 0));
        Assert.False(wall.Contains(0, 6));
        Assert.False(wall.Contains(6, 6));
    }

    // ----------------------------
    // AddSpider tests
    // ----------------------------

    [Fact]
    public void AddSpider_SucceedsForValidSpider()
    {
        var wall = new Wall("5 5");
        var spider = new Spider("1 1 Up");

        bool added = wall.AddSpider(spider);

        Assert.True(added);
        Assert.Equal(1, spider.X);
        Assert.Equal(1, spider.Y);
        Assert.Null(spider.Error);
    }

    [Fact]
    public void AddSpider_FailsForSpiderOutsideWall()
    {
        var wall = new Wall("5 5");
        var spider = new Spider("6 6 Up");

        bool added = wall.AddSpider(spider);

        Assert.False(added);
        Assert.NotNull(spider.Error);
        Assert.Equal("Spider position (6,6) is outside the wall.", spider.Error);
    }

    [Fact]
    public void AddSpider_FailsIfWallHasError()
    {
        var wall = new Wall(""); // invalid wall
        var spider = new Spider("1 1 Up");

        bool added = wall.AddSpider(spider);

        Assert.False(added);
        Assert.NotNull(spider.Error);
        Assert.Equal("No wall size provided.", spider.Error);
    }
}