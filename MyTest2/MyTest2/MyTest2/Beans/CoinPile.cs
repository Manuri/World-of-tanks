using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;

namespace MyTest2.Beans
{
    class CoinPile : Treasure
    //class CoinPile 
    {
        /*private Point _coordinate;
        private bool _isPresent;
        private int _lifeTime;
        private int _appearedTime;
        private int _vanishingTime;
        protected Timer aTimer;*/
        private int _value;
        

        public CoinPile(int x, int y, int theValue, int theLifeTime)
        {
            Coordinate = new Point(x, y);
            Value = theValue;
            LifeTime = theLifeTime;
            IsPresent = true;
            AppearedTime = System.DateTime.Now.ToString(); 
            aTimer = new System.Timers.Timer(theLifeTime); 
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
            GC.KeepAlive(aTimer);
        }

       /* public Point Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
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
        }*/

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        //public override void atExpiry(object source, ElapsedEventArgs e) 
        private void OnTimedEvent(object source, ElapsedEventArgs e)  
        {
            this.IsPresent = false;
            GameManager.getGameManager.removeCoinsFromMap(this);
            VanishingTime = DateTime.Now.ToString();
            Console.WriteLine("from " + AppearedTime + " to " + VanishingTime);
            Console.WriteLine("lifetime " + LifeTime);
        }


    }
}
