using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyTest2.Beans
{
    class Player
    {
        private int _playerIndex;
        private Point _coordinate;
        private int _score;
        private int _coins;
        private int _health;
        private bool _isAlive;
        private bool _shot; 
        private int _direction;
        private Vector2 _screenPosition;

        public Player()
        {
        }

        public Player(int index, int x, int y, int direction )
        {
            PlayerIndexNo = index;
            Coordinate = new Point(x, y);
            Direction = direction;
            IsAlive = true;
            Health = 100;
            Shot = false;
            Coins = 0;
            Score = 0;
        }

        public int PlayerIndexNo
        {
            get { return _playerIndex; }
            set { _playerIndex = value; }
        }

        public Point Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }


        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int Coins
        {
            get { return _coins; }
            set { _coins = value; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public bool Shot
        {
            get { return _shot; }
            set { _shot = value; }
        }

        public Vector2 ScreenPosition
        {
            get { return _screenPosition; }
            set { _screenPosition = value; }
        }

        //these are used for my tank only
        public void move(string direction)
        {
        }

        public void shoot()
        {
        }
    }
}
