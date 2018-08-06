using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ftpg
{
    /// <summary>
    /// Types cells can be
    /// </summary>
    internal enum GridCellType
    {
        Usable,
        Wall,
        Player,
        Enemy,
        Path
    }

    /// <summary>
    /// The states Cells can be
    /// </summary>
    internal enum GridCellState
    {
        Open,
        Closed,
        NotVisited
    }

    class GridCell : GameObject
    {
        public GridCellType Type
        {
            get { return type; }
            set { var oldType = type;  type = value; }
        }

        public Point Position { get; private set; }
        public Point ScreenCoords { get; private set; }
        public int Size { get; private set; }
        public GridCellState State { get; set; }
        public GridCell Parent { get; set; }
        public int G { get; set; }
        public double H { get; set; }
        public double F { get; set; }
        public bool showPath { get; set; }

        private GridCellType type;
        private Texture2D pixel;

        public GridCell(Game game, GridCellType type, Point position, Point screenCoords, int size)
            : base(game)
        {
            Type = type;
            Position = position;
            ScreenCoords = screenCoords;
            Size = size;
            pixel = new Texture2D(game.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            State = GridCellState.NotVisited;
            showPath = false;
        }

        /// <summary>
        /// Fetch the cell colour and then draw it
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            Color colour;

            if (Type == GridCellType.Usable && State == GridCellState.Open && showPath)
            {
                colour = Color.DarkCyan;
            }
            else if (Type == GridCellType.Usable && State == GridCellState.Closed && showPath)
            {
                colour = Color.LightCyan;
            }
            else
            {
                colour = GetColour(Type);
            }

            SpriteBatch.Draw(pixel, new Rectangle(ScreenCoords.X, ScreenCoords.Y, Size, Size), colour);
        }

        /// <summary>
        /// Resets the CellType and Cell values
        /// </summary>
        public void clear() 
        {
            Reset();
            Type = GridCellType.Usable;
        }

        /// <summary>
        /// Resets the cell to initalize state
        /// </summary>
        public void Reset()
        {
            if (Type == GridCellType.Path)
            {
                type = GridCellType.Usable;
            }
            State = GridCellState.NotVisited;
            Parent = null;
            H = G = 0;
        }

        /// <summary>
        /// Puts the Cell position and coordinates to string
        /// </summary>
        /// <returns>String on position and coordinates</returns>
        public override string ToString()
        {
            return String.Format("Pos: "+Position+" :: ScreenCoordinates :"+ScreenCoords);
        }


        /// <summary>
        /// Get the colour of the cell based on type
        /// </summary>
        /// <param name="type">The cell type</param>
        /// <returns>The colour</returns>
        private Color GetColour(GridCellType type)
        {
            switch (type)
            {
                case GridCellType.Enemy: return Color.Red;
                case GridCellType.Player: return Color.Green;
                case GridCellType.Usable: return Color.Black;
                case GridCellType.Wall: return Color.Blue;
                case GridCellType.Path: if (showPath) { return Color.Orange;  } else { return Color.Black; };
                default: return Color.Black;
            }
        }

        /// <summary>
        /// Is the cell Orthagonal with the target cell
        /// </summary>
        /// <param name="otherCell">The target cell</param>
        /// <returns>True if Orthagonal, False if not</returns>
        public bool IsOrthagonalWith(GridCell otherCell)
        {
            return Position.X == otherCell.Position.X || Position.Y == otherCell.Position.Y;
        }


        /// <summary>
        /// Fetch the AI found path as cells
        /// </summary>
        /// <returns>List of the cell Path</returns>
        public IEnumerable<GridCell> GetPath()
        {
            var path = new List<GridCell>();
            GridCell currCell = this;
            while (currCell != null)
            {
                if (currCell.Type == GridCellType.Usable)
                {
                    currCell.Type = GridCellType.Path;
                }
                path.Add(currCell);
                currCell = currCell.Parent;
            }
            path.Reverse();
            path.RemoveAt(0); //remove the grid source
            return path;
        }
    }
}
