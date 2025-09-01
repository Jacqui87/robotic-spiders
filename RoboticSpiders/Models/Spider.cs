using RoboticSpiders.Enums;

namespace RoboticSpiders.Models;

public interface ISpider
{
    int X { get; }
    int Y { get; }
    Direction Facing { get; }
    string? Error { get; }

    void Place(int x, int y);
    void SetError(string message);
    void ExecuteInstructions(string? instructions, IWall wall);
}

public class Spider : ISpider
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction Facing { get; private set; }
    public string? Error { get; private set; }

    public Spider(string? startingPosition)
    {
        if (string.IsNullOrWhiteSpace(startingPosition))
        {
            Error = "No spider starting position provided";
            return;
        }

        var parts = startingPosition.Trim().Split(' ');
        if (parts.Length < 3)
        {
            Error = "Spider input must have 3 parts: X Y Direction";
            return;
        }

        if (!int.TryParse(parts[0], out int x) ||
            !int.TryParse(parts[1], out int y) ||
            !Enum.TryParse(parts[2], true, out Direction facing))
        {
            Error = "Invalid spider input: X and Y must be integers, Direction must be valid";
            return;
        }

        Facing = facing;
        Place(x, y);
    }

    public void Place(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetError(string message)
    {
        Error = message;
    }

    public void ExecuteInstructions(string? instructions, IWall wall)
    {
        if (Error is not null) return;

        if (string.IsNullOrWhiteSpace(instructions))
        {
            Error = "No movement instructions provided.";
            return;
        }

        foreach (var command in instructions.Trim())
        {
            switch (command)
            {
                case 'L': TurnLeft(); break;
                case 'R': TurnRight(); break;
                case 'F': MoveForward(wall); break;
                default:
                    Error = $"Invalid command: {command}";
                    return;
            }
        }
    }

    private void TurnLeft() => Facing = Facing switch
    {
        Direction.Up => Direction.Left,
        Direction.Left => Direction.Down,
        Direction.Down => Direction.Right,
        Direction.Right => Direction.Up,
        _ => Facing
    };

    private void TurnRight() => Facing = Facing switch
    {
        Direction.Up => Direction.Right,
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        _ => Facing
    };

    private void MoveForward(IWall wall)
    {
        int newX = X, newY = Y;

        switch (Facing)
        {
            case Direction.Up: newY++; break;
            case Direction.Down: newY--; break;
            case Direction.Left: newX--; break;
            case Direction.Right: newX++; break;
        }

        if (wall.Contains(newX, newY))
        {
            X = newX;
            Y = newY;
        }
        else
        {
            Error = $"Spider cannot move outside the wall (attempted {newX},{newY}).";
        }
    }

    public override string ToString() => $"{X} {Y} {Facing}";
}