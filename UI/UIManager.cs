using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using SC_VSCode.Entities;

namespace SC_VSCode.UI
{
    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole
    {
        public ScrollingConsole MapConsole;
        public Window MapWindow;
        public MessageLogWindow MessageLog;
        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = SadConsole.Global.CurrentScreen;
        }
        // Creates all child consoles to be managed
        // make sure they are added as children
        // so they are updated and drawn
        public void CreateConsoles()
        {
            // Temporarily create a console with *no* tile data that will later be replaced with map data
            MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight);
        }
        public override void Update(TimeSpan timeElapsed)
        {
            CheckKeyboard();
            base.Update(timeElapsed);
        }
        // centers the viewport camera on an Actor
        public void CenterOnActor(Actor actor)
        {
            MapConsole.CenterViewPortOnPoint(actor.Position);
        }
        // Scans the SadConsole's Global KeyboardState and triggers behaviour
        // based on the button pressed.
         private void CheckKeyboard()
        {
            if(SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.A))
            {
                SadConsole.Settings.ToggleFullScreen();
            }
            //keyboard movement for Player character : Up arrow
            if(SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0,-1));
                    CenterOnActor(GameLoop.World.Player);
                }
            // Keyboard movement for Player character: Down arrow
            // Increment player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player,new Point(0, 1));
                CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Left arrow
            // Decrement player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 0));
                CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Right arrow
            // Increment player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 0));
                CenterOnActor(GameLoop.World.Player);
            }
            // Redo last command: X
            if(SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.X))
            {
                GameLoop.CommandManager.RedoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
            }
            //Undo last command : Z
            if(SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                GameLoop.CommandManager.UndoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
            }
        }
        // Creates a window that encloses a map console
        // of a specified height and width
        // and displays a centered window title
        // make sure it is added as a child of the UIManager
        // so it is updated and drawn
        public void CreateMapWindow(int width, int height, string title)
        {
            MapWindow = new Window(width, height);
            MapWindow.CanDrag = true;

            //make console short enough to show the window title
            //and borders, and position it away from borders
            int mapConsoleWidth = width - 2;
            int mapConsoleHeight = height - 2;

            // Resize the Map Console's ViewPort to fit inside of the window's borders snugly
            MapConsole.ViewPort = new Rectangle(0, 0, mapConsoleWidth, mapConsoleHeight);

            //reposition the MapConsole so it doesnt overlap with the left/top window edges
            MapConsole.Position = new Point(1, 1);

            //close window button
            Button closeButton = new Button(3, 1);
            closeButton.Position = new Point(0, 0);
            closeButton.Text = "[X]";

            //Add the close button to the Window's list of UI elements
            MapWindow.Add(closeButton);

            // Centre the title text at the top of the window
            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);

            //add the map viewer to the window
            MapWindow.Children.Add(MapConsole);

            // The MapWindow becomes a child console of the UIManager
            Children.Add(MapWindow);

            // Add the player to the MapConsole's render list
            MapConsole.Children.Add(GameLoop.World.Player);

            // Without this, the window will never be visible on screen
            MapWindow.Show();
        }
        // Initializes all windows and consoles
        public void Init()
        {
            CreateConsoles();

            //MessageLog initialization
            MessageLog = new MessageLogWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Message Log");
            Children.Add(MessageLog);
            MessageLog.Show();
            MessageLog.Position = new Point(0, GameLoop.GameHeight / 2);

            MessageLog.Add("Testing 123");
            MessageLog.Add("Testing 1224");
            MessageLog.Add("Testing 123");
            MessageLog.Add("Testing 12543");
            MessageLog.Add("Testing 123");
            MessageLog.Add("Testing 1253");
            MessageLog.Add("Testing 1212");
            MessageLog.Add("Testing 1");
            MessageLog.Add("Testing");
            MessageLog.Add("Testing 122");
            MessageLog.Add("Testing 51");
            MessageLog.Add("Testing");
            MessageLog.Add("Testing 162");
            MessageLog.Add("Testing 16");
            MessageLog.Add("Testing Last");

            //Load the map into MapConsole
            LoadMap(GameLoop.World.CurrentMap);

            //After MapConsole is ready - build a window
            CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Game Map");
            UseMouse = true;

            //start the game with viewport focused on the player
            CenterOnActor(GameLoop.World.Player);
        }

        // Adds the entire list of entities found in the
        // World.CurrentMap's Entities SpatialMap to the
        // MapConsole, so they can be seen onscreen
        private void SyncMapEntities(Map map)
        {
            // remove all Entities from the console first
            MapConsole.Children.Clear();

            //pull all of the entities into MapConsole in bulk
            foreach(Entity entity in map.Entities.Items) // access SpatialMap objects via Items
            {
                MapConsole.Children.Add(entity);
            }
            // Subscribe to the Entities ItemAdded listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemAdded += OnMapEntityAdded;

            // Subscribe to the Entities ItemRemoved listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemRemoved += OnMapEntityRemoved;
        }

        // Remove an Entity from the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Remove(args.Item);
        }

        // Add an Entity from the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Add(args.Item);
        }

        // Loads a Map into the MapConsole
        private void LoadMap(Map map)
        {
            // First load the map's tiles into the console
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height,Global.FontDefault,
                                                        new Rectangle(0,0,GameLoop.GameWidth, GameLoop.GameHeight), map.Tiles);
            
            // Now Sync all of the map's entities
            SyncMapEntities(map);
        }
    }
}