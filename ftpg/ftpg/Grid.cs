using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ftpg
{
    class Grid : GameObject
    {
        public int CellSize { get; set; }
        public GridCell player { get; set; }
        public GridCell enemy { get; set; }
        public IEnumerable<GridCell> path { get; set; }

        public int Width
        {
            get { return ScreenWidth / CellSize; }
        }

        public int Height
        {
            get { return ScreenHeight / CellSize; }
        }

        private Texture2D pixel;
        private GridCell[,] cells;
        private static readonly Color lineColour = Color.Black;

        /// <summary>
        /// Grid manager for the game, Creates and displays the game level grid while managing Grid cells
        /// </summary>
        /// <param name="game">The current game</param>
        /// <param name="cellSize">Size of each cell</param>
        public Grid(Game game, int cellSize)
            : base(game)
        {
            CellSize = cellSize;
            pixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { lineColour });
            CreateCells();
            CreateMap();
        }

        /// <summary>
        /// Draws all GridCells
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (GridCell cell in cells)
            {
                cell.Draw(gameTime);
            }
        }

        /// <summary>
        /// Resets all Cells state to Notvisited
        /// </summary>
        public void Reset()
        {
            foreach (GridCell cell in cells)
            {
                cell.Reset();
            }
        }

        /// <summary>
        /// DEPRECIATED: Swap target cell to character
        /// </summary>
        /// <param name="moveCell">Target cell</param>
        /// <param name="isPlayer">For the player or enemy</param>
        public void MovePlayer(GridCell moveCell, bool isPlayer)
        {
            if (isPlayer)
            {
                player.Type = GridCellType.Usable;
                player.State = GridCellState.NotVisited;
                moveCell.Type = GridCellType.Player;
                player = moveCell;

            }
            else
            {
                enemy.Type = GridCellType.Usable;
                enemy.State = GridCellState.NotVisited;
                moveCell.Type = GridCellType.Enemy;
                enemy = moveCell;
            }
        }

        /// <summary>
        /// Sets cells to show the path once found, Toggles on and off.
        /// </summary>
        public void ShowPath(){
            foreach (GridCell cell in cells)
            {
                cell.showPath = !cell.showPath;
            }
        }

        /// <summary>
        /// Creates all cells for the grid and stores each in a local Multidemsional array
        /// </summary>
        void CreateCells()
        {
            GridCell newCell;
            cells = new GridCell[Width, Height]; // Create cell array from screen dimensions
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    newCell = new GridCell(Game, GridCellType.Usable, new Point(i, j), new Point(i * CellSize, j * CellSize), CellSize);
                    cells[i, j] = newCell; // Add cell to current position in Cell array
                }
            }
        }

        /// <summary>
        /// Creates the level walls from multidimension array data (Must be edited in class)
        /// DEPRECIATED: Also sets the pisitions for player and enemy cells
        /// </summary>
        public void CreateMap()
        {
            int[][] mapCells = new int[21][];

            mapCells[0] = new int[] { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 };
            mapCells[1] = new int[] { 0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0 };
            mapCells[2] = new int[] { 0,1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1,0 };
            mapCells[3] = new int[] { 0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0 };
            mapCells[4] = new int[] { 0,1,0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,0,1,0 };
            mapCells[5] = new int[] { 0,1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1,0 };
            mapCells[6] = new int[] { 0,1,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,1,1,0 };
            mapCells[7] = new int[] { 0,1,1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,0 };
            mapCells[8] = new int[] { 0,1,0,0,1,0,1,0,1,1,0,1,1,0,1,0,1,0,0,1,0 };
            mapCells[9] = new int[] { 0,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,0 };
            mapCells[10] = new int[] { 0,1,0,0,1,0,1,0,1,1,0,1,1,0,1,0,1,0,0,1,0 };
            mapCells[11] = new int[] { 0,1,1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,0 };
            mapCells[12] = new int[] { 0,1,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,1,0 };
            mapCells[13] = new int[] { 0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0 };
            mapCells[14] = new int[] { 0,1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1,0 };
            mapCells[15] = new int[] { 0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0 };
            mapCells[16] = new int[] { 0,1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1,0 };
            mapCells[17] = new int[] { 0,1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1,0 };
            mapCells[18] = new int[] { 0,1,0,1,1,1,1,1,1,0,1,0,1,1,1,1,1,1,0,1,0 };
            mapCells[19] = new int[] { 0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0 };
            mapCells[20] = new int[] { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 };

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    GridCell test = cells[j, i];

                    switch(mapCells[i][j]){
                        case 1: 
                            test.Type = GridCellType.Wall; 
                            break;
                        case 2:
                            test.Type = GridCellType.Player;
                            player = test;
                            break;
                        case 3:
                            test.Type = GridCellType.Enemy;
                            enemy = test;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns all cells that are next to the Currentcell
        /// </summary>
        /// <param name="currentCell">The current cell of the target</param>
        /// <returns>All adjacent cells</returns>
        public IEnumerable<GridCell> GetAdjacentCells(GridCell currentCell)
        {
            GridCell adjacentCell;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    adjacentCell = CellAtPosition(currentCell.Position.X + i, currentCell.Position.Y + j);

                    // if the adjacentCell is not null, the adjacentCell is not the Current Cell and is Orthagonal with the current cell
                    if (adjacentCell != null && adjacentCell != currentCell && currentCell.IsOrthagonalWith(adjacentCell))
                    {
                        yield return adjacentCell;
                    }
                }
            }
        }

        /// <summary>
        /// Gets and filters down Adjacent cells to ones that are Usable.
        /// </summary>
        /// <param name="cell">The originator cell</param>
        /// <returns>All usable adjacent Cells</returns>
        public List<GridCell> GetValidAdjacentCells(GridCell cell)
        {
            IEnumerable<GridCell> adjCells = GetAdjacentCells(cell).Where(c => c.State != GridCellState.Closed && c.Type != GridCellType.Wall && CellIsWalkableTo(cell, c));

            return adjCells.ToList();
        }

        /// <summary>
        /// Is destination cell usable
        /// </summary>
        /// <param name="source">The originator cell</param>
        /// <param name="destination">The destination cell</param>
        /// <returns>True or false</returns>
        public bool CellIsWalkableTo(GridCell source, GridCell destination)
        {
            return !(CellAtPosition(source.Position.X, destination.Position.Y).Type == GridCellType.Wall || CellAtPosition(destination.Position.X, source.Position.Y).Type == GridCellType.Wall);
        }

        /// <summary>
        /// Fetch the cell at specified coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Returns the cell at the position</returns>
        public GridCell CellAtCoordinate(float x, float y)
        {
            var position = PositionAtCoordinate(x, y);
            return CellAtPosition(position.X, position.Y);
        }

        /// <summary>
        /// Get the cell at position
        /// </summary>
        /// <param name="x">X position (Cell number accross)</param>
        /// <param name="y">Y position (Cell number down)</param>
        /// <returns>Cell at that position or null if out of range</returns>
        public GridCell CellAtPosition(int x, int y)
        {
            return IsPositionValid(new Point(x, y)) ? cells[x, y] : null;
        }

        /// <summary>
        /// Convert Coordinates to screen position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Point position</returns>
        Point PositionAtCoordinate(float x, float y)
        {
            return new Point((int)Math.Ceiling(Convert.ToDouble(Width * x / ScreenWidth)) - 1, (int)Math.Ceiling(Convert.ToDouble(Height * y / ScreenHeight)) - 1);
        }

        /// <summary>
        /// Is the current point in grid space
        /// </summary>
        /// <param name="position">Position to test</param>
        /// <returns>True if in grid, False if not</returns>
        bool IsPositionValid(Point position)
        {
            return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
        }
    }
}
