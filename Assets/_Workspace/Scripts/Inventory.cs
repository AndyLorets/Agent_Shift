using System.Collections.Generic;
using UnityEngine;

public static class Inventory 
{
    public static List<Item> items = new List<Item>();

    public static void AddItem(Item item)
    {
        items.Add(item);
    }

    public static void RemoveItem(Item item)
    {
        items.Remove(item);
    }
}
