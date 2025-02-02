using System;
using SadConsole;
using Microsoft.Xna.Framework;

namespace SC_VSCode
{
    // Abstract and basic
    // TileBase is the simple form
    // Of all the tiles
    public abstract class TileBase: Cell
    {
        // Movement and Line of Sight Flags
        public bool IsBlockingMove;
        public bool IsBlockingLOS;
        // Tile's name
        protected string Name;
        // TileBase is an abstract base class 
        // representing the most basic form of of all Tiles used.
        // Every TileBase has a Foreground Colour, Background Colour, and Glyph
        // IsBlockingMove and IsBlockingLOS are optional parameters, set to false by default
        // default constructor
        public TileBase(Color foreground, Color background, int glyph, bool blocksMovement=false, bool blocksLOS=false, String name="")
                        :base(foreground, background, glyph)
        {
            IsBlockingMove = blocksMovement;
            IsBlockingLOS = blocksLOS;
            Name = name;
        }
    }
}