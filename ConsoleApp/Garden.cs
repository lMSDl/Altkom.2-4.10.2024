using ConsoleApp.Properties;

namespace ConsoleApp;
public class Garden
{
    public int Size { get; }
    private ICollection<string> Items { get; }
    private ILogger Logger { get; }

    public Garden(int size, ILogger logger)
    {
        if(size < 0)
            throw new ArgumentOutOfRangeException("size");
        Size = size;
        Items = [];
        Logger = logger;
    }

    public bool Plant(string name)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(Resources.WhitespaceNameException, nameof(name));

        if (Items.Count >= Size)
        {
            Logger?.Log(string.Format(Resources.NoSpaceInGardenFor, name));
            return false;
        }

        if (Items.Contains(name))
        {
            var newName = name + (Items.Count(x => x.StartsWith(name)) + 1);
            Logger?.Log(string.Format(Resources.PlantNameChanged, name, newName));
            name = newName;
        }

        Items.Add(name);
        Logger?.Log(string.Format(Resources.PlantAddedToGarden, name));


        return true;
    }

    public IEnumerable<string> GetPlants()
    {
        return Items.ToList();
    }

    public bool Remove(string name)
    {
        if(!Items.Contains(name))
            return false;

        _ = Items.Remove(name);
        Logger?.Log(string.Format(Resources.PlantRemovedFromGarden, name));
        return true;
    }

    public void Clear()
    {
        Items.Clear();
    }

    public int Count()
    {
        return Items.Count;
    }

    public string? GetLastLog()
    {
        string? log = Logger?.GetLogsAsync(DateTime.Now.AddMinutes(-1), DateTime.Now).Result;
        return log?.Split("\n").Last();
    }
}
