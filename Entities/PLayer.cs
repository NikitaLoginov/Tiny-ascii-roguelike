using System;
using Microsoft.Xna.Framework;

namespace SC_VSCode.Entities
{
    // Creates a new player
    // Default glyph is @
    public class Player : Actor
    {
        public Player(Color foreground, Color background):base(foreground, background,'@')
        {
            Attack = 10;
            AttackChance = 40;
            Defense = 5;
            DefenseChance = 20;
            Name = "Vladislav";
        }
    }
}