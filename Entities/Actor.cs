using System;
using Microsoft.Xna.Framework;

namespace SC_VSCode.Entities
{
    public abstract class Actor : Entity
    {
        private int _health; // current health
        private int _maxHealth; // maximum health
        public int Health {get{return _health;} set{_health = value;}}
        public int MaxHealth {get{return _maxHealth;} set{_maxHealth = value;}}
        protected Actor(Color foreground, Color background, int glyph, int width=1, int height=1) : base(foreground, background, glyph, width, height)
        {
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
        }
        // Moves the Actor BY positionChange tiles in any X/Y direction
        // returns true if actor was able to move, false if failed to move

        public bool MoveBy(Point positionChange)
        {
            if(SC_VSCode.GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
            {
                Position += positionChange;
                return true;
            } else
            {
                return false;
            }
        }
        // Moves the Actor TO newPosition location
        // returns true if actor was able to move, false if failed to move
        public bool MoveTo(Point newPosition)
        {
            Position = newPosition;
            return true;
        }
    }
}