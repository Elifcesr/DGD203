﻿using System;
using System.Numerics;

public class Location
{
    #region VARIABLES


    public string Name { get; private set; }
    public Vector2 Coordinates { get; private set; }
    public LocationType Type { get; private set; }

    public List<Item> ItemsOnLocation { get; private set; }

    #endregion

    #region CONSTRUCTOR

    public Location(string locationName, LocationType type, Vector2 coordinates, List<Item> itemsOnLocation)
    {
        Name = locationName;
        Type = type;
        Coordinates = coordinates;
        ItemsOnLocation = itemsOnLocation; // The list is given by the constructor arguments
    }

    public Location(string locationName, LocationType type, Vector2 coordinates)
    {
        Name = locationName;
        Type = type;
        Coordinates = coordinates;
        ItemsOnLocation = new List<Item>(); // The list is created, but it has ZERO elements
    }

    #endregion

    #region METHODS

    public void RemoveItem(Item item)
    {
        ItemsOnLocation.Remove(item);
    }

    #endregion
}

public enum LocationType
{
    Forest,
    Witch Coven
}