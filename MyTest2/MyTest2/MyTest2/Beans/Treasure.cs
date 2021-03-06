﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace MyTest2.Beans
{
    class Treasure
    {
        private Point _coordinate;
        private bool _isPresent;
        private int _lifeTime;
        private DateTime _appearedTime;
        private DateTime _vanishingTime;
        protected Timer aTimer;

        public Treasure()
        {
        }

        public Treasure(int x, int y, int lifeTime)
        {
            Coordinate = new Point(x, y);
            LifeTime = lifeTime;
            IsPresent = true;
            AppearedTime = DateTime.Now;
            aTimer = new System.Timers.Timer(lifeTime);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
            GC.KeepAlive(aTimer);
        }

        public Point Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        public DateTime AppearedTime
        {
            get { return _appearedTime; }
            set { _appearedTime = value; }
        }

        public DateTime VanishingTime
        {
            get { return _vanishingTime; }
            set { _vanishingTime = value; }
        }

        public int LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value; }
        }

        public bool IsPresent
        {
            get { return _isPresent; }
            set { _isPresent = value; }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e) 
        {
            this.IsPresent = false; 
            GameManager.getGameManager.removeLifepacksFromMap(this);
            //Console.WriteLine("lifetime " + LifeTime);
            VanishingTime = DateTime.Now;
            //Console.WriteLine((VanishingTime-AppearedTime).ToString());

        }
    }
}
