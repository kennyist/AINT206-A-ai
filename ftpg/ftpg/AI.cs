using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ftpg
{
    class AI : GameObject
    {
        public bool IsActive { get; set; }
        private Grid Grid;
        private GridCell currentCell;
        private Tree<double, GridCell> openTree;

        public IEnumerable<GridCell> foundPath { get; set; }

        public bool pathFound { set; get; }
        public bool showPathFinding{get;set;}
        private bool aiMove { get; set; }

        public AI(Game game, Grid grid)
            : base(game)
        {
            Grid = grid;
            IsActive = false;
            pathFound = false;
            aiMove = false;
            showPathFinding = false;
        }

        /// <summary>
        /// Start searching for a path
        /// </summary>
        public void Search()
        {
            IsActive = true;

            openTree = new Tree<double, GridCell>();

            currentCell = Grid.enemy;
            currentCell.State = GridCellState.Closed;

            if (!showPathFinding)
            {
                StepForward();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive && showPathFinding)
            {
                StepForward();
            } 
            else
            {
                return;
            }
        }

        /// <summary>
        /// New step into the path finding, Used with update and showPathFinding to slow down, Or called by itself for realtime.
        /// </summary>
        private void StepForward()
        {
            foreach (GridCell Adjacent in Grid.GetValidAdjacentCells(currentCell))
            {
                if (Adjacent.State != GridCellState.Open)
                {
                    Adjacent.State = GridCellState.Open;
                    Adjacent.Parent = currentCell;
                    Adjacent.H = (int)Math.Abs(Adjacent.Position.X - Grid.enemy.Position.X) + Math.Abs(Adjacent.Position.Y - Grid.enemy.Position.Y) * 15; // Set heuristic, with a higher cost to allow for longer paths in some cases
                    Adjacent.H += SortTie(Adjacent); // Added to stop any ties
                    Adjacent.G = currentCell.G + (currentCell.IsOrthagonalWith(Adjacent) ? 10 : 14);
                    Adjacent.F = Adjacent.G + Adjacent.H;

                    openTree.Insert(Adjacent.F, Adjacent);
                }
            }
            if(openTree.Count == 0)
            {
                IsActive = false;
                return;
            }

            currentCell = openTree.RemoveMin();
            currentCell.State = GridCellState.Closed;

            if (Grid.player.State == GridCellState.Closed)
            {
                End(); // Show path and move Enemy
            }
            else
            {
                if (!showPathFinding)
                {
                    StepForward(); 
                }
            }
        }

        /// <summary>
        /// Once a path has been found, get path and declare as found
        /// </summary>
        private void End()
        {
            IsActive = false;
            foundPath = currentCell.GetPath();
            pathFound = true;
        }

        /// <summary>
        /// Stop any ties
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <returns>Double</returns>
        private double SortTie(GridCell cell)
        {
            int dis1X = Math.Abs(cell.Position.X - Grid.player.Position.X);
            int dis1Y = Math.Abs(cell.Position.Y - Grid.player.Position.Y);
            int dis2X = Math.Abs(Grid.enemy.Position.X - Grid.player.Position.X);
            int dis2Y = Math.Abs(Grid.enemy.Position.Y - Grid.player.Position.Y);
            return Math.Abs((dis1X * dis2Y) - (dis2X * dis1Y)) * 0.01;
        }
    }
}
