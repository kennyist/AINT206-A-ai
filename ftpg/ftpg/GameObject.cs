using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ftpg
{
    /// <summary>
    /// Global class for use on every Object for the game, Keeps the sprite back update and draw methods for use on others.
    /// </summary>
    abstract class GameObject
    {
        protected Game Game { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        protected int ScreenWidth
        {
            get { return Game.Window.ClientBounds.Width; }
        }
        protected int ScreenHeight
        {
            get { return Game.Window.ClientBounds.Height; }
        }

        protected GameObject(Game game)
        {
            Game = game;
            SpriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        public virtual void Draw(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
    }
}
