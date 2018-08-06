using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ftpg
{

    class TreeNodeVal<T, T1> where T:IComparable<T>
    {
        public T Key { get; set; }
        public T1 Value { get; set; }

        public TreeNodeVal(T key, T1 value)
        {
            Key = key;
            Value = value;
        }
    }

    class Tree<T, T1> where T:IComparable<T>
    {
        public Tree() : this(100) { }
        public int Count { get { return items.Count; } }
        private readonly List<TreeNodeVal<T, T1>> items;

        public Tree(int InitCapacity)
        {
            items = new List<TreeNodeVal<T, T1>>(InitCapacity);
        }

        /// <summary>
        /// Insirt cell and sell priority
        /// </summary>
        /// <param name="priority">Cell Scale</param>
        /// <param name="item">Cell</param>
        public void Insert(T priority, T1 item)
        {
            var node = new TreeNodeVal<T, T1>(priority, item);
            items.Add(node);
            Increase(items.Count - 1);
        }

        /// <summary>
        /// Removes bottom cell
        /// </summary>
        /// <returns>Bottom value</returns>
        public T1 RemoveMin()
        {
            var minValue = items[0].Value;
            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            Decrease(0);
            return minValue;
        }

        public T1 PeakMin()
        {
            if (items[0] == null)
            {
                return default(T1);
            }
            else
            {
                return items[0].Value;
            }
        }

        /// <summary>
        /// Increase a cells rank
        /// </summary>
        /// <param name="rank">Cell Rank</param>
        void Increase(int rank)
        {
            int parentRank = (int)Math.Floor(((float)(rank - 1) / 2));
            if (parentRank > 0)
            {
                TreeNodeVal<T, T1> parent = items.ElementAt(parentRank), node = items.ElementAt(rank);
                if (parent.Key.CompareTo(node.Key) >= 0)
                {
                    var temp = items[rank];
                    items[rank] = items[parentRank];
                    items[parentRank] = temp;
                    Increase(parentRank);
                }
            } 
        }

        /// <summary>
        /// Decereases a cells rank
        /// </summary>
        /// <param name="rank">Rank</param>
        void Decrease(int rank)
        {
            int minimumChildRank = MinimumRank(ChildRank(rank), ChildRank(rank) + 1);
            if (minimumChildRank > items.Count - 1 || items[minimumChildRank].Key.CompareTo(items[rank].Key) >= 0)
            {
                return;
            }
            var temp = items[rank];
            items[rank] = items[minimumChildRank];
            items[minimumChildRank] = temp;
            Decrease(minimumChildRank);
        }

        /// <summary>
        /// Get child rank
        /// </summary>
        /// <param name="rank">Rank</param>
        /// <returns>int child rank</returns>
        int ChildRank(int rank)
        {
            return (2 * rank) + 1;
        }
        
        /// <summary>
        /// Get the minimum rank of the two imputs
        /// </summary>
        /// <param name="rank1">First rank</param>
        /// <param name="rank2">Second Rank</param>
        /// <returns>Int rank</returns>
        int MinimumRank(int rank1, int rank2)
        {
            int top = items.Count - 1;
            if (rank1 > top || rank2 > top)
            {
                if (rank1 > top) { 
                    return rank2; 
                } else { 
                    return rank1; 
                }
            } else {
                return items[rank1].Key.CompareTo(items[rank2].Key) < 0 ? rank1 : rank2;
            }
        }
    }
}
