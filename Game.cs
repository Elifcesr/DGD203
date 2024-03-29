﻿using System.IO;
using System.Numerics;

namespace The_Ritual
{
    public class Game
    {
        #region VARIABLES

        #region Game Constants

        private const int _defaultMapWidth = 5;
        private const int _defaultMapHeight = 5;

        #endregion

        #region Game Variables

        #region Player Variables

        public Player Player { get; private set; }

        private string _playerName;
        private List<Item> _loadedItems;

        #endregion

        #region World Variables

        private Location[] _locations;

        #endregion

        private bool _gameRunning;
        private Map _gameMap;
        private string _playerInput;

        #endregion

        #endregion

        #region METHODS

        #region Initialization

        public void StartGame(Game gameInstanceReference)
        {
            // Generate game environment
            CreateNewMap();

            // Load game
            LoadGame();

            // Deal with player generation
            CreatePlayer();


            InitializeGameConditions();

            _gameRunning = true;
            StartGameLoop();

        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, _defaultMapWidth, _defaultMapHeight);
        }

        private void CreatePlayer()
        {
            if (_playerName == null)
            {
                GetPlayerName();
            }

            Player = new Player(_playerName, _loadedItems);
        }

        private void GetPlayerName()
        {
            Console.WriteLine("Hello there, welcome.");
            Console.WriteLine("Can you tell me your name, please?");
            _playerName = Console.ReadLine();

            if (_playerName == "Robert")
            {
                Console.WriteLine($"There you are {_playerName}!!");
            }
            else if (_playerName == "")
            {
                Console.WriteLine("Player name not entered, giving the name Jenny");
                _playerName = "Jenny";
            }
            else
            {
                Console.WriteLine($"Welcome again {_playerName}, let me tell you what you seek. You'll end up in a forest and you have to gather some items for a ritual. Be quick. You don't have much time. Oh by the way, It's a pleasure to meet you.");
            }
        }

        private void InitializeGameConditions()
        {
            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }


        #endregion

        #region Game Loop

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                GetInput();
                ProcessInput();
            }
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
        }

        private void ProcessInput()
        {
            if (_playerInput == "" || _playerInput == null)
            {
                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput)
            {
                case "N":
                    _gameMap.MovePlayer(0, 1);
                    break;
                case "S":
                    _gameMap.MovePlayer(0, -1);
                    break;
                case "E":
                    _gameMap.MovePlayer(1, 0);
                    break;
                case "W":
                    _gameMap.MovePlayer(-1, 0);
                    break;
                case "exit":
                    Console.WriteLine("We hope you enjoyed our game!");
                    _gameRunning = false;
                    break;
                case "save":
                    SaveGame();
                    Console.WriteLine("Game saved");
                    break;
                case "load":
                    LoadGame();
                    Console.WriteLine("Game loaded");
                    break;
                case "help":
                    Console.WriteLine(HelpMessage());
                    break;
                case "where":
                    _gameMap.CheckForLocation(_gameMap.GetCoordinates());
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "who":
                    Console.WriteLine($"You are {Player.Name}, the collector.");
                    break;
                case "take":
                    _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                    break;
                case "inventory":
                    Player.CheckInventory();
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    break;
            }
        }

        #endregion

        #region Save Management

        private void LoadGame()
        {
            string path = SaveFilePath();

            if (!File.Exists(path)) return;

            // Reading the file contents
            string[] saveContent = File.ReadAllLines(path);

            // Set the player name
            _playerName = saveContent[0];

            // Set player coordinates
            List<int> coords = saveContent[1].Split(',').Select(int.Parse).ToList();
            Vector2 coordArray = new Vector2(coords[0], coords[1]);

            // Set player inventory
            _loadedItems = new List<Item>();

            List<string> itemStrings = saveContent[2].Split(',').ToList();

            for (int i = 0; i < itemStrings.Count; i++)
            {
                if (Enum.TryParse(itemStrings[i], out Item result))
                {
                    Item item = result;
                    _loadedItems.Add(item);
                    _gameMap.RemoveItemFromLocation(item);
                }
            }

            _gameMap.SetCoordinates(coordArray);

        }

        private void SaveGame()
        {
            // Player Coordinates
            string xCoord = _gameMap.GetCoordinates()[0].ToString();
            string yCoord = _gameMap.GetCoordinates()[1].ToString();
            string playerCoords = $"{xCoord},{yCoord}";

            // Player inventory
            List<Item> items = Player.Inventory.Items;
            string playerItems = "";
            for (int i = 0; i < items.Count; i++)
            {
                playerItems += items[i].ToString();

                if (i != items.Count - 1)
                {
                    playerItems += ",";
                }
            }

            string saveContent = $"{_playerName}{Environment.NewLine}{playerCoords}{Environment.NewLine}{playerItems}";

            string path = SaveFilePath();

            File.WriteAllText(path, saveContent);
        }

        private string SaveFilePath()
        {
            // Get the save file path
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory + @"\save.txt";

            return path;
        }

        #endregion

        #region Miscellaneous

        private string HelpMessage()
        {
            return @"Here are the current commands:
N: go north
S: go south
W: go west
E: go east
load: Load saved game
save: save current game
exit: exit the game";

        }

        #endregion

        #endregion
    }
}