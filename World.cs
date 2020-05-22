using System;
using SadConsole;
using SadConsole.Components;
using Microsoft.Xna.Framework;
using SC_VSCode.Entities;

namespace SC_VSCode
{
    // All game state data is stored in World
    // also creates and processes generators
    // for map creation
    public class World
    {
        // map creation and storage data
        private int _mapWidth = 100;
        private int _mapHeight = 100;
        private TileBase[] _mapTiles;
        private int _maxRooms = 100;
        private int _minRoomSize = 4;
        private int _maxRoomSize = 15;
        public Map CurrentMap { get; set; }
        // player data
        public Player Player { get; set; }

        // Creates a new game world and stores it in
        // publicly accessible
        public World()
        {
            //build a map
            CreateMap();

            // create an instance of a player
            CreatePlayer();

            // create monsters
            CreateMonsters();

            //spawn loot
            CreateLoot();
        }
        // Create a new map using the Map class
        // and a map generator. Uses several 
        // parameters to determine geometry
        private void CreateMap()
        {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            MapGenerator mapGen = new MapGenerator();
            CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }
        // Create a player using the Player class
        // and set its starting position
        private void CreatePlayer()
        {
            Player = new Player(Color.Yellow, Color.Transparent);
            Player.Components.Add(new EntityViewSyncComponent());

            // spawning player on first tile that doesn't block movement
            for (int i = 0; i < CurrentMap.Tiles.Length; i++)
            {
                if(!CurrentMap.Tiles[i].IsBlockingMove)
                {
                    //set player's position to the index of the current map position using GetPointFromIndex method
                    Player.Position = SadConsole.Helpers.GetPointFromIndex(i, CurrentMap.Width);
                    break;
                }
            }

            // Add the ViewPort sync Component to the player
            Player.Components.Add(new EntityViewSyncComponent());
        }

        // Create some random monsters with random attack and defense values
        // and drop them all over the map in
        // random places.
        private void CreateMonsters()
        {
            //number of monsters to create
            int numMonsters = 10;

            //random position generator
            Random rndNum = new Random();

            // Create several monsters and 
            // pick a random position on the map to place them.
            // check if the placement spot is blocking 
            // and if it is, try a new position
            for (int i = 0; i < numMonsters; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster(Color.Red, Color.Transparent);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while(CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum.Next(0, CurrentMap.Width*CurrentMap.Height);
                }

                // magic numbers for an attack and defense values
                newMonster.Defense = rndNum.Next(1,10);
                newMonster.DefenseChance = rndNum.Next(1,50);
                newMonster.Attack = rndNum.Next(1,10);
                newMonster.AttackChance = rndNum.Next(1,50);
                newMonster.Name = " an internet troll";

                // Set the monster's new position
                // Note: this fancy math will be replaced by a new helper method
                // in the next revision of SadConsole
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }
        }
        // Create some sample treasure
        // that can be picked up on the map
        private void CreateLoot()
        {
            // number of treasure drops to create
            int numLoot = 20;
            Random rndNum = new Random();

            // Produce lot up to a max of numLoot
            for (int i = 0; i < numLoot; i++)
            {
                // Create an Item with some standard attributes
                int lootPosition = 0;
                Item newLoot = new Item(Color.DarkSalmon, Color.Transparent, "fancy shirt", 'L');

                // Let SadConsole know that this Item's position be tracked on the map
                newLoot.Components.Add(new EntityViewSyncComponent());

                // Try placing the Item at lootPosition; if this fails, try random positions on the map's tile array
                while(CurrentMap.Tiles[lootPosition].IsBlockingMove)
                {
                    //pick random spot on the map
                    lootPosition = rndNum.Next(0,CurrentMap.Width * CurrentMap.Height);
                }

                // set the loot's new position
                newLoot.Position = new Point(lootPosition % CurrentMap.Width, lootPosition/CurrentMap.Height);

                // add the Item to the MultiSpatialMap
                CurrentMap.Add(newLoot);
            }
        }
    }
}