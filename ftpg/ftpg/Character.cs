using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ftpg
{
    class Character : GameObject
    {

        public bool fixedMove = false;  // If currently moving
        private Vector2 Position;       // Position of the character
        private Vector2 destination;    // Desitination for the character
        private float maxSpeed;         // Max speed the character can go
        private Texture2D texture;      // The characters texture
        private Color colour;           // the colour for the character
        private Rectangle rectPosition; // characters rectange Height width and position

        /// <summary>
        /// A game Character, Such as the player.
        /// </summary>
        /// <param name="game">The Current game</param>
        /// <param name="pos">Starting position in the map</param>
        /// <param name="bCol">The colour of the character</param>
        public Character(Game game, Rectangle pos, Color bCol)
            : base(game)
        {
            Position = new Vector2(pos.X, pos.Y);
            rectPosition = new Rectangle(pos.X, pos.Y, pos.Width, pos.Height);
            colour = bCol;
            maxSpeed = 1.0f;
        }

        /// <summary>
        /// Draw the character.
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void  Draw(GameTime gameTime)
        {
            rectPosition.X = (int)Position.X;
            rectPosition.Y = (int)Position.Y;

            SpriteBatch.Draw(texture, rectPosition, colour);
        }

        /// <summary>
        /// Find target and move this character towards it, keeping in by max speed
        /// </summary>
        /// <param name="position">The target position</param>
        private void Move(Vector2 position)
        {
            Vector2 target = position - CharacterPosition; // Find the vector from Current pos to target pos

            if (target != Vector2.Zero) { target.Normalize(); } // normalise to unit length

            CharacterPosition += target * maxSpeed; // Move chacter towards that position
        }

        public override void Update(GameTime gameTime)
        {
 	         if(fixedMove){ // if moveTo is set

                 Move(destination); // Move towards the target

                 if (Position == destination) // If at target
                 {
                     fixedMove = false; // stop moving
                 }
             }
        }

        /// <summary>
        /// Checks to see if the character is within a certain distance of another chacter.
        /// </summary>
        /// <param name="Char">The target character</param>
        /// <param name="screenPercent">Percentage of the screen size, E.G: 0.25 for 25%</param>
        /// <returns>Returns true if in distance, False if not</returns>
        public bool IsInDistance(Character Char, float screenPercent)
        {
            // Devided the screen dimensions by percentage distance
            float rangeW = ScreenWidth * screenPercent;
            float rangeH = ScreenHeight * screenPercent;

            // Get the average
            float average = (rangeH + rangeW) / 2;

            // Get distance from This to character
            float distance = Vector2.Distance(this.Position, Char.Position);

            // Return true or false depening on distance
            return (distance < average) ? true : false;
        }

        /// <summary>
        /// Starts a fixed movement from current cell to Distination cell
        /// </summary>
        /// <param name="cell">The destination</param>
        public void MoveTo(GridCell cell){
            destination = new Vector2(cell.ScreenCoords.X,cell.ScreenCoords.Y);
            fixedMove = true;
        }

        /// <summary>
        /// Set this characters Texture
        /// </summary>
        public Texture2D SetTexture
        {
            set { texture = value; }
        }

        /// <summary>
        /// Get and Set this characters max speed
        /// </summary>
        public float MaxSpeed
        {
            set { maxSpeed = value; }
            get { return maxSpeed; }
        }

        /// <summary>
        /// Get and Set this chacters position
        /// </summary>
        public Vector2 CharacterPosition
        {
            set { Position = value; }
            get { return Position; }
        }

        /// <summary>
        /// Get this characters Rectangle Position
        /// </summary>
        public Rectangle RectPosition
        {
            get { return rectPosition; }
            set { rectPosition = value; }
        }
    }
}
