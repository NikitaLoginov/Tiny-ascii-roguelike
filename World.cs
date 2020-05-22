using System;
using SadConsole;
using SadConsole.Components;
using Microsoft.Xna.Framework;

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
            // spawning player on first tile that doesn't block movement
            for (int i = 0; i < CurrentMap.Tiles.Length; i++)
            {
                if(!CurrentMap.Tiles[i].IsBlockingMove)
                {
                    //set player's position to the index of the current map position using GetPointFromIndex method
                    Player.Position = SadConsole.Helpers.GetPointFromIndex(i, CurrentMap.Width);
                }
            }

            // Add the ViewPort sync Component to the player
            Player.Components.Add(new EntityViewSyncComponent());
        }
    }
}