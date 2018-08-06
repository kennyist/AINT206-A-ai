using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace ftpg
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<GameObject> gameObjects;

        private int cellSize;
        private Grid grid;
        private AI aiPath;
        private bool showEnd = false;
        private bool moving = false;
        private string looseMsg = "Game Over";
        private SpriteFont font;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private Character player;
        private Character enemy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 735; // override screen size to fit map 
            graphics.PreferredBackBufferWidth = 735; // override screen size to fit map
            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";

            gameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);


            cellSize = 35;

            grid = new Grid(this, cellSize);
            aiPath = new AI(this, grid);

            enemy = new Character(this, new Rectangle(70, 35, cellSize, cellSize), Color.Red);
            player = new Character(this, new Rectangle(630, 665, cellSize, cellSize), Color.Green);

            grid.player = grid.CellAtCoordinate(player.RectPosition.X + 35, player.RectPosition.Y + 35);
            grid.enemy = grid.CellAtCoordinate(enemy.RectPosition.X + 35, enemy.RectPosition.Y + 35);

            gameObjects.Add(grid);
            gameObjects.Add(aiPath);

            aiPath.Search();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            player.SetTexture = Content.Load<Texture2D>("image\\player");
            enemy.SetTexture = Content.Load<Texture2D>("image\\enemy");
            font = Content.Load<SpriteFont>("font\\font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            HandleKeyboardEvents();


            aiPath.Update(gameTime);
            player.Update(gameTime);
            enemy.Update(gameTime);

            if (player.IsInDistance(enemy, 0.01f))
            {
                showEnd = true;
                return;
            }

            if (aiPath.pathFound && !enemy.fixedMove)
            {
                enemy.MoveTo(aiPath.foundPath.ElementAt(0));

                if (player.IsInDistance(enemy, 0.25f))
                {
                    enemy.MaxSpeed = 1.25f;
                }
                else
                {
                    enemy.MaxSpeed = 1f;
                }

                moving = true;
                aiPath.pathFound = false;
            }

            if (moving && !enemy.fixedMove)
            {
                grid.Reset();
                aiPath.Search();
                moving = false;
            }

            grid.player = grid.CellAtCoordinate(player.RectPosition.X + 35, player.RectPosition.Y + 35);
            grid.enemy = grid.CellAtCoordinate(enemy.RectPosition.X + 35, enemy.RectPosition.Y + 35);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            grid.Draw(gameTime);
            player.Draw(gameTime);
            enemy.Draw(gameTime);

            if (showEnd)
            {
                spriteBatch.DrawString(font, looseMsg, new Vector2(10, 10), Color.Red);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Handle any keyboard button presses
        /// </summary>
        private void HandleKeyboardEvents()
        {
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else if (currentKeyboardState.IsKeyDown(Keys.W) && !player.fixedMove && !aiPath.IsActive) // Move up
            {
                grid.Reset();
                foreach (GridCell cell in grid.GetValidAdjacentCells(grid.CellAtCoordinate(player.RectPosition.X + 35, player.RectPosition.Y + 35)))
                {
                    if (cell.ScreenCoords.Y < player.RectPosition.Y)
                    {
                        player.MoveTo(cell);
                        break;
                    }
                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.A) && !player.fixedMove && !aiPath.IsActive) // Move left
            {
                grid.Reset();
                foreach (GridCell cell in grid.GetValidAdjacentCells(grid.CellAtCoordinate(player.CharacterPosition.X + 35, player.CharacterPosition.Y + 35)))
                {
                    if (cell.ScreenCoords.X < player.RectPosition.X)
                    {
                        player.MoveTo(cell);
                        break;
                    }
                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D) && !player.fixedMove && !aiPath.IsActive) // move right
            {
                grid.Reset();
                foreach (GridCell cell in grid.GetValidAdjacentCells(grid.CellAtCoordinate(player.CharacterPosition.X + 35, player.CharacterPosition.Y + 35)))
                {
                    if (cell.ScreenCoords.X > player.RectPosition.X)
                    {
                        player.MoveTo(cell);
                        break;
                    }
                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.S) && !player.fixedMove && !aiPath.IsActive) // Move down
            {
                grid.Reset();
                foreach (GridCell cell in grid.GetValidAdjacentCells(grid.CellAtCoordinate(player.CharacterPosition.X + 35, player.CharacterPosition.Y + 35)))
                {
                    if (cell.ScreenCoords.Y > player.RectPosition.Y)
                    {
                        player.MoveTo(cell);
                        break;
                    }
                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Q) && previousKeyboardState.IsKeyUp(Keys.Q) && !aiPath.IsActive) // Toggle the showing of path finding
            {
                aiPath.showPathFinding = !aiPath.showPathFinding;
                grid.ShowPath();
            }
        }
    }
}
