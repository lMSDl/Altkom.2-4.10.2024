﻿using ConsoleApp.Properties;

namespace ConsoleApp;
public class Garden
{
    public int Size { get; }
    private ICollection<string> Items { get; }

    public Garden(int size)
    {
        Size = size;
        Items = [];
    }

    public bool Plant(string name)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(Resources.WhitespaceNameException, nameof(name));

        if (Items.Count >= Size)
            return false;

        if (Items.Contains(name))
        {
            name += (Items.Count(x => x.StartsWith(name)) + 2);
        }

        Items.Add(name);

        return true;
    }

    public IEnumerable<string> GetPlants()
    {
        return Items.ToList();
    }
}
