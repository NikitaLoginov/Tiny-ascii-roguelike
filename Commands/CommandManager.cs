using System;
using Microsoft.Xna.Framework;
using SC_VSCode.Entities;

namespace SC_VSCode.Commands
{
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and alike.
    public class CommandManager
    {
        //stores the actor's last move action
        private Point _lastMoveActorPoint;
        private Actor _lastMoveActor;
        public CommandManager()
        {

        }

        // Move the actor BY +/- X&Y coordinates
        // returns true if the move was successful
        // and false if unable to move there
        public bool MoveActorBy(Actor actor, Point position)
        {
            // stores last actor move state
            _lastMoveActor = actor;
            _lastMoveActorPoint = position;
            return actor.MoveBy(position);
        }

        //Redo last 
        public bool RedoMoveActorBy()
        {
            // checking if actor available
            if(_lastMoveActor != null)
            {
                return _lastMoveActor.MoveBy(_lastMoveActorPoint);
            }
            else
            {
                return false;
            }
        }
        // Undo last actor move
        // clear undo so if cannot be repeated
        public bool UndoMoveActorBy()
        {
            // checking if actor is available
            if(_lastMoveActor != null)
            {
                _lastMoveActorPoint = new Point(-_lastMoveActorPoint.X, -_lastMoveActorPoint.Y);
                if(_lastMoveActor.MoveBy(_lastMoveActorPoint))
                {
                    _lastMoveActorPoint = new Point(0,0);
                    return true;
                }
                else
                {
                    _lastMoveActorPoint = new Point(0,0);
                    return false;
                }
            }
            return false;
        }
    }
}