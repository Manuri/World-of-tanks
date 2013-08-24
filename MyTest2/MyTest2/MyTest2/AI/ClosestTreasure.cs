using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyTest2.AI
{
    class ClosestTreasure:IComparable<ClosestTreasure>
    {
        private Point _coordinate;
        private int _cost;
        private LinkedList<Point> _path;

        public ClosestTreasure()
        {
            Path = new LinkedList<Point>();
        }

        public Point Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public LinkedList<Point> Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public int CompareTo(ClosestTreasure other)
        {
            return (other._cost.CompareTo(this._cost));
        }
    }
}
