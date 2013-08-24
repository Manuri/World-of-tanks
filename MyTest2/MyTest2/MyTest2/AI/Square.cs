using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyTest2.AI
{
    enum SquareContent
    {
        Empty,
        Brick,
        Water,
        Stone,
        Coinpile,
        Healthpack,
        Foe,
        me
    };

    class CompleteSquare
    {
        private SquareContent _contentCode = SquareContent.Empty;
        private int _distanceSteps = 10000;
        private bool _isPath = false;
        private Point _coordinate;
        private int _damageLevel ;
        private Vector2 _screenCoordinate;
        private bool _obstaclePresent = false;

        public CompleteSquare()
        {
        }

        public CompleteSquare(int x,int y,SquareContent content) 
        {
            Coordinate = new Point(x,y);
            DamageLevel = 0;
            ContentCode=content;

        }

        public SquareContent ContentCode
        {
            get { return _contentCode; }
            set { _contentCode = value; }
        }
       
        public int DistanceSteps
        {
            get { return _distanceSteps; }
            set { _distanceSteps = value; }
        }
      
        public bool IsPath
        {
            get { return _isPath; }
            set { _isPath = value; }
        }

        public Point Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        public int DamageLevel
        {
            get { return _damageLevel; }
            set { _damageLevel = value; }
        }

        public Vector2 ScreenCoordinate
        {
            get { return _screenCoordinate; }
            set { _screenCoordinate = value; }
        }

        public bool ObstaclePresent
        {
            get { return _obstaclePresent; }
            set { _obstaclePresent = value; }
        }
       /* public void FromChar(char charIn)
        {
            switch (charIn)
            {
                case 'B':
                    _contentCode = SquareContent.Brick;
                    break;
                case 'W':
                    _contentCode = SquareContent.Water;
                    break;
                case 'C':
                    _contentCode = SquareContent.Coinpile;
                    break;
                case 'H':
                    _contentCode = SquareContent.Healthpack;
                    break;
                case 'F':
                    _contentCode = SquareContent.Foe;
                    break;
                case ' ':
                default:
                    _contentCode = SquareContent.Empty;
                    break;
            }
        }*/
    }
}
