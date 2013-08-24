using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyTest2.AI
{
    class Path : IComparable<Path>
    {
        LinkedList<Point> _aShortestPath;
        private int _cost;

        public Path(LinkedList<Point> aPath, int theCost)
        {
            _aShortestPath = aPath;
            _cost = theCost;
        }

        public LinkedList<Point> PointList
        {
            get { return _aShortestPath; }
        }

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public int CompareTo(Path other)
        {
            return (other._cost.CompareTo(this._cost));
        }
    }
}
