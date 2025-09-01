using NSubstitute;
using RoboticSpiders.Enums;
using RoboticSpiders.Models;

namespace RoboticSpiders.Tests.Models;

public class SpiderTests
{
    [Fact]
    public void Spider_Returns_5_7_facing_Right()
    {
        var wall = new Wall("7 15");
        var spider = new Spider("4 10 Left");
        wall.AddSpider(spider);
        spider.ExecuteInstructions("FLFLFRFFLF", wall);

        Assert.Equal("5 7 Right", spider.ToString());
        Assert.Equal(5, spider.X);
        Assert.Equal(7, spider.Y);
        Assert.Equal(Direction.Right, spider.Facing);
        Assert.Null(spider.Error);
    }

    // ------------------------
    // Spider Constructor Tests
    // ------------------------
    [Fact]
    public void Constructor_WithValidInput_ShouldSetProperties()
    {
        var spider = new Spider("1 2 Up");
        Assert.Equal(1, spider.X);
        Assert.Equal(2, spider.Y);
        Assert.Equal(Direction.Up, spider.Facing);
        Assert.Null(spider.Error);
    }

    [Theory]
    [InlineData(null, "No spider starting position provided")]
    [InlineData("", "No spider starting position provided")]
    [InlineData("1 2", "Spider input must have 3 parts: X Y Direction")]
    [InlineData("a b Up", "Invalid spider input: X and Y must be integers, Direction must be valid")]
    [InlineData("1 2 InvalidDir", "Invalid spider input: X and Y must be integers, Direction must be valid")]
    public void Constructor_WithInvalidInput_ShouldSetError(string? input, string output)
    {
        var spider = new Spider(input);
        Assert.Equal(output, spider.Error);
    }

    // ------------------------
    // Spider Movement Tests
    // ------------------------
    [Fact]
    public void TurnLeft_ShouldRotateCorrectly()
    {
        var wall = Substitute.For<IWall>();
        wall.Contains(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

        var spider = new Spider("0 0 Up");
        spider.ExecuteInstructions("L", wall);
        Assert.Equal(Direction.Left, spider.Facing);

        spider.ExecuteInstructions("L", wall);
        Assert.Equal(Direction.Down, spider.Facing);

        spider.ExecuteInstructions("L", wall);
        Assert.Equal(Direction.Right, spider.Facing);

        spider.ExecuteInstructions("L", wall);
        Assert.Equal(Direction.Up, spider.Facing);
    }

    [Fact]
    public void TurnRight_ShouldRotateCorrectly()
    {
        var wall = Substitute.For<IWall>();
        wall.Contains(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

        var spider = new Spider("0 0 Up");
        spider.ExecuteInstructions("R", wall);
        Assert.Equal(Direction.Right, spider.Facing);

        spider.ExecuteInstructions("R", wall);
        Assert.Equal(Direction.Down, spider.Facing);

        spider.ExecuteInstructions("R", wall);
        Assert.Equal(Direction.Left, spider.Facing);

        spider.ExecuteInstructions("R", wall);
        Assert.Equal(Direction.Up, spider.Facing);
    }

    [Fact]
    public void MoveForward_WithinWall_ShouldUpdatePosition()
    {
        var wall = Substitute.For<IWall>();
        wall.Contains(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

        var spider = new Spider("1 1 Up");
        spider.ExecuteInstructions("F", wall);
        Assert.Equal(1, spider.X);
        Assert.Equal(2, spider.Y);

        spider.ExecuteInstructions("RFRF", wall);
        Assert.Equal(2, spider.X);
        Assert.Equal(1, spider.Y);
    }

    [Fact]
    public void MoveForward_OutsideWall_ShouldSetError()
    {
        var wall = Substitute.For<IWall>();
        wall.Contains(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

        var spider = new Spider("0 0 Up");
        spider.ExecuteInstructions("F", wall);
        Assert.NotNull(spider.Error);
        Assert.Equal("Spider cannot move outside the wall (attempted 0,1).", spider.Error);
    }

    [Fact]
    public void ExecuteInstructions_WithNullOrEmptyInstructions_ShouldSetError()
    {
        var wall = Substitute.For<IWall>();
        wall.Contains(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

        var spider = new Spider("1 1 Up");
        spider.ExecuteInstructions(null, wall);
        Assert.Equal("No movement instructions provided.", spider.Error);

        spider = new Spider("1 1 Up");
        spider.ExecuteInstructions("", wall);
        Assert.Equal("No movement instructions provided.", spider.Error);
    }

    [Fact]
    public void ExecuteInstructions_WithInvalidCommand_ShouldSetError()
    {
        var wall = Substitute.For<IWall>();
        wall.Contains(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

        var spider = new Spider("1 1 Up");
        spider.ExecuteInstructions("X", wall);
        Assert.Equal("Invalid command: X", spider.Error);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        var spider = new Spider("1 2 Right");
        Assert.Equal("(1,2) facing: Right", spider.ToString());
    }
}