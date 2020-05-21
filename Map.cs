using System;
using Microsoft.Xna.Framework;

namespace SC_VSCode
{
    public class Map
    {
        TileBase[] _tiles; // contains all the objects on the map
        private int _width;
        private int _height;
        public TileBase[] Tiles { get {return _tiles;} set{_tiles = value;}}
        public int Width { get {return _width;} set {_width = value;}}
        public int Height { get {return _height;} set {_height = value;}}
        //Build a new map with a specified width and height
        public Map(int width, int height)
        {
            _width = width;
            _height = height;
            Tiles = new TileBase[width * height];
        }
        // IsTileWalkable checks
        // to see if the actor has tried
        // to walk off the map or into a non-walkable tile
        // Returns true if the tile location is walkable
        // false if tile location is not walkable or is off-map
        public bool IsTileWalkable(Point location)
        {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if(location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
            {
                return false;
            }
            return !_tiles[location.Y*Width+location.X].IsBlockingMove;
        }
    }
}