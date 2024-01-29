using System;
using System.Numerics;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;

public class Map
{
    private Game _theGame;
    
    private Vector2 _coordinates;

    private int[] _widthBoundaries;
    private int[] _heightBoundaries;

    private Location[] _locations;


    public Map(Game game, int width, int height)
    {
        _theGame = game;

        // Setting the width boundaries
        int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
        _widthBoundaries[1] = widthBoundary;

        // Setting the height boundaries
        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
        _heightBoundaries[0] = -heightBoundary;
        _heightBoundaries[1] = heightBoundary;

        // Setting starting coordinates
        _coordinates = new Vector2(0, 0);

        GenerateLocations();

        // Display result
        Console.WriteLine("Creating World...");
        //Console.WriteLine($"Created map with size {width}x{height}");
    }

    #region Coordinates

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public void SetCoordinates(Vector2 newCoordinates)
    {
        _coordinates = newCoordinates;
    }

    #endregion

    #region Movement

    public void MovePlayer(int x, int y)
    {
        int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

        if (!CanMoveTo(newXCoordinate, newYCoordinate))
        {
            Console.WriteLine("You can't go that way.");
            return;
        }

        _coordinates[0] = newXCoordinate;
        _coordinates[1] = newYCoordinate;
        //_coordinates = new Vector2(newXCoordinate, newYCoordinate);

        CheckForLocation(_coordinates);
    }

    private bool CanMoveTo(int x, int y)
    {
        return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
    }

    #endregion

    #region Locations

    private void GenerateLocations()
    {
        _locations = new Location[6];
        // Mossy Hollow
        Vector2 hollowLocation = new Vector2(0, 0);
        List<Item> hollowItems = new List<Item>();
        hollowItems.Add(Item.Lotus Potion);
        Location hollow = new Location("Hollow", LocationType.Forest, hollowLocation, hollowItems);
        _locations[0] = hollow;
        // Willows Grove
        Vector2 willowsgroveLocation = new Vector2(-2, 2);
        List<Item> willowsgroveitems = new List<Item>();
        willowsgroveitems.Add(Item.Crystal Charm);
        Location WillowsGrove = new Location("Willows Grove", LocationType.Forest, willowsgroveLocation, willowsgroveitems);
        _locations[1] = WillowsGrove;
        // Sycamore Sanctuary
        Vector2 sycamoresanctuaryLocation = new Vector2(1, -2);
        List<Item> sycamoresanctuaryItems = new List<Item>();
        sycamoresanctuaryItems.Add(Item.Moonstone Elixir);
        Location SycamoreSanctuary = new Location("Sycamore Sanctuary", LocationType.Forest, sycamoresanctuaryLocation, sycamoresanctuaryItems);
        _locations[2] = SycamoreSanctuary;
        // Witch's Riddle
        Vector2 FirstLocation = new Vector2(-2, 1);
        Location FirstPuzzle = new Location("Witch's Riddle", LocationType.WitchCoven, FirstLocation);
        _locations[3] = FirstPuzzle;


        Vector2 hollowLocation = new Vector2(1, 1);
        Location hollow = new Location("Hollow", LocationType.Forest, hollowLocation);
        _locations[4] = hollow;


        Vector2 secondCombatLocation = new Vector2(-1, -2);
        Location secondCombat = new Location("Willows Grove", LocationType.Forest, secondCombatLocation);
        _locations[5] = secondCombat;


    }
    public void WitchCoven(string name)
    {
        Console.WriteLine("Witch: In the heart of the witch coven's sacred space, the master witch presides, her presence commanding attention.");
        Console.WriteLine("Witch: Who's there? You are in our sacred grounds.");
        Console.WriteLine("Witch: I assume you are the one. Did you gathered all the items that we needed for the ritual.");
        Console.WriteLine($"Witch: If you want to join us, collecting items in a forest all day can't be the only requirement. {name}.");


        Console.WriteLine("Witch: Before all of these, you have to answer my riddle to proove that you're worthy for being one of us.");
        Console.WriteLine("Witch: In realms unseen, where whispers weave,");
        Console.WriteLine("Witch: Through mystic arts, I can deceive.");
        Console.WriteLine("Witch: With wands and words, I shape and sway,");
        Console.WriteLine("Witch: What am I, in the witch's play?");

        string playerAnswer = Console.ReadLine();

        if (playerAnswer.ToLower() == "spell")
        {
            Console.WriteLine("Witch: Good guess. You proved that you're like us afterall.");

            RestartApp(Process.GetCurrentProcess().Id, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
        else
        {
            Console.WriteLine("Witch: WRONG! You'll always be unworthy to become one of us.");


            Console.WriteLine("Witch: Now, prepare to be faded. This won't hurt.");


            return;
        }
        static void RestartApp(int pid, string applicationName)
        {
            // Wait for the process to terminate
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
                process.WaitForExit(1000);
            }
            catch (ArgumentException ex)
            {
                // ArgumentException to indicate that the 
                // process doesn't exist?   LAME!!
            }
            Process.Start(applicationName, "");
        }
    }
    public void CheckForLocation(Vector2 coordinates)
    {
        Console.WriteLine($"You're standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.WitchCoven)
            {
                Console.WriteLine("Get ready to solve the riddle!");
                WitchCoven(this._theGame.Player.Name);


            }
            else
            {
                Console.WriteLine($"You are in {location.Name} {location.Type}");

                if (HasItem(location))
                {
                    Console.WriteLine($"You collected {location.ItemsOnLocation[0]} . Name a direction to go to the next location.");
                }
            }
        }
    }

    private bool IsOnLocation(Vector2 coords, out Location foundLocation)
    {
        try
        {

            for (int i = 0; i < _locations.Length; i++)
            {
                if (_locations[i].Coordinates == coords)
                {
                    foundLocation = _locations[i];
                    return true;
                }
            }

        }
        catch (Exception)
        {
            Console.WriteLine("You can't go to that direction.");
        }
        foundLocation = null;
        return false;
    }

    private bool HasItem(Location location)
    {
        return location.ItemsOnLocation.Count != 0;

        // ---- THE LONG FORM ----
        //if (location.ItemsOnLocation.Count == 0)
        //{
        //	return false;
        //} else
        //{
        //	return true;
        //}
    }

    public void TakeItem(Location location)
    {

    }

    public void TakeItem(Player player, Vector2 coordinates)
    {
        if (IsOnLocation(coordinates, out Location location))
        {
            if (HasItem(location))
            {
                Item itemOnLocation = location.ItemsOnLocation[0];

                player.TakeItem(itemOnLocation);
                location.RemoveItem(itemOnLocation);
                if (itemOnLocation.ToString() == "Lotus Potion")
                {
                    Console.WriteLine("You got the Lotus Potion. It will be useful for her.");
                }
                else
                {
                    Console.WriteLine($"You took the {itemOnLocation}");
                }

                return;
            }
        }

        Console.WriteLine("There is nothing to collect here!");
    }

    public void RemoveItemFromLocation(Item item)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].ItemsOnLocation.Contains(item))
            {
                _locations[i].RemoveItem(item);
            }
        }
    }

    #endregion
}