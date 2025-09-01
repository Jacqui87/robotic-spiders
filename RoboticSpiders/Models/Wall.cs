namespace RoboticSpiders.Models;

public interface IWall
{
    int MaxX { get; }
    int MaxY { get; }
    string? Error { get; }

    bool Contains(int x, int y);
    bool AddSpider(ISpider spider);
}

public class Wall : IWall
{
    public int MaxX { get; }
    public int MaxY { get; }
    public string? Error { get; private set; }

    private readonly List<ISpider> _spiders = new();

    public Wall(string? wallInput)
    {
        if (string.IsNullOrWhiteSpace(wallInput))
        {
            Error = "No wall size provided.";
            return;
        }

        var parts = wallInput.Trim().Split(' ');
        if (parts.Length < 2 ||
            !int.TryParse(parts[0], out int maxX) ||
            !int.TryParse(parts[1], out int maxY))
        {
            Error = "Invalid wall size input.";
            return;
        }

        MaxX = maxX;
        MaxY = maxY;
    }

    public bool Contains(int x, int y) =>
        x >= 0 && y >= 0 && x <= MaxX && y <= MaxY;

    public bool AddSpider(ISpider spider)
    {
        if (Error is not null)
        {
            spider.SetError(Error);
            return false;
        }

        if (!Contains(spider.X, spider.Y))
        {
            spider.SetError($"Spider position ({spider.X},{spider.Y}) is outside the wall.");
            return false;
        }

        _spiders.Add(spider);
        return true;
    }
}