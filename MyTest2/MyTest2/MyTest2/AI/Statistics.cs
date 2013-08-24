using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyTest2.Beans;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework;

namespace MyTest2.AI
{
    class Statistics
    {
        private static Statistics instance;
        private bool _playerUp,_playerDown,_playerRight,_playerLeft;
        public ClosestTreasure _closestCoin;
        public ClosestTreasure _closestLifePack;
        public ClosestTreasure _bestToFollow;

        public Statistics()
        {
            _playerUp = false;
            _playerDown = false;
            _playerRight = false;
            _playerLeft = false;

            _closestCoin = new ClosestTreasure();
            _closestLifePack = new ClosestTreasure();
        }

        public static Statistics getStatistics
        {
            get
            {
                if (instance == null)
                {
                    instance = new Statistics();
                }
                return instance;
            }
        }

        public bool PlayerUp
        {
            get{return _playerUp;}
        }

        public bool PlayerDown
        {
            get { return _playerDown; }
        }

        public bool PlayerRight
        {
            get { return _playerRight; }
        }

        public bool PlayerLeft
        {
            get { return _playerLeft; }
        }

        public void checkPlayers()//use inside a thread to update the bool variables in this class
        {
            Player myPlayer = Map.getMap.AllTanks[Map.getMap.MyIndex];
            Player[] all = Map.getMap.AllTanks;
            Player[] enemies=new Player[Map.getMap.NoOfPlayers-1];
            int j = 0;
            for (int i = 0; i < Map.getMap.NoOfPlayers;i++ )
            {
                if (all[i].Coordinate != all[Map.getMap.MyIndex].Coordinate)
                {
                    enemies[j] = all[i];
                    j++;
                }
            }

            while (true)
            {
               // foreach (Player p in Map.getMap.AllTanks)
                foreach (Player p in enemies)
                {
                    if (p.IsAlive)
                    {
                        if (p.Coordinate.X == myPlayer.Coordinate.X)
                        {
                            if (p.Coordinate.Y > myPlayer.Coordinate.Y && !_playerDown)
                            {
                                // _playerUp = true;
                                _playerDown = true;
                            }
                            else if (!_playerUp)// _playerDown = true;
                                _playerUp = true;

                        }
                        else
                        {
                            _playerUp = false;
                            _playerDown = false;
                        }
                        if (p.Coordinate.Y == myPlayer.Coordinate.Y)
                        {
                            if (p.Coordinate.X > myPlayer.Coordinate.X && !_playerRight)
                            {
                                _playerRight = true;
                            }
                            else if (!_playerLeft) _playerLeft = true;
                        }
                        else
                        {
                            _playerLeft = false;
                            _playerRight = false;
                        }

                    }
                }
            }
        }

        public void setRealCostsToPath(ref ClosestTreasure treasure, bool isLifePack)
        {
            int myHealth = Map.getMap.AllTanks[Map.getMap.MyIndex].Health;

            switch(isLifePack)
            {
                case true:
                    if (myHealth < 49)
                    {
                        treasure.Cost = treasure.Cost * 1;
                    }
                    else if (myHealth < 79)
                    {
                        treasure.Cost = treasure.Cost * 2;
                    }
                    else if (myHealth < 99)
                    {
                        treasure.Cost = treasure.Cost * 5;
                    }
                    else if (myHealth < 150)
                    {
                        treasure.Cost = treasure.Cost * 8;
                    }
                    else
                    {
                        treasure.Cost = treasure.Cost * 9;
                    }
                    break;

                case false:
                    if (myHealth < 49)
                    {
                        treasure.Cost = treasure.Cost * 9;
                    }
                    else if (myHealth < 79)
                    {
                        treasure.Cost = treasure.Cost * 8;
                    }
                    else if (myHealth < 99)
                    {
                        treasure.Cost = treasure.Cost * 5;
                    }
                    else if (myHealth < 150)
                    {
                        treasure.Cost = treasure.Cost * 2;
                    }
                    else
                    {
                        treasure.Cost = treasure.Cost * 1;
                    }
                    break;
            }
        }

        public void findLeastDistanceTreasures()
        {
            int coinMin = 10000;
            int lifePackMin = 10000;
            int x=0;
            int y=0;
            foreach (CoinPile c in Map.getMap.CoinList.Values)
            {
                x = c.Coordinate.X;
                y = c.Coordinate.Y;

                if (Pathfinder.getPathFinder.Squares[x, y].DistanceSteps < coinMin)
                {
                    coinMin = Pathfinder.getPathFinder.Squares[x, y].DistanceSteps;
                }
            }
            _closestCoin.Coordinate = Pathfinder.getPathFinder.Squares[x, y].Coordinate;
            _closestCoin.Cost = coinMin;
            Console.WriteLine("closestCoin at " + _closestCoin.Coordinate.X + ", " + _closestCoin.Coordinate.Y + " cost: " + _closestCoin.Cost);

            foreach (Treasure lp in Map.getMap.LifePackList.Values)
            {
                x = lp.Coordinate.X;
                y = lp.Coordinate.Y;

                if (Pathfinder.getPathFinder.Squares[x, y].DistanceSteps < lifePackMin)
                {
                    lifePackMin = Pathfinder.getPathFinder.Squares[x, y].DistanceSteps;                    
                }
            }
            _closestLifePack.Coordinate = Pathfinder.getPathFinder.Squares[x, y].Coordinate;
            _closestLifePack.Cost = lifePackMin;
            Console.WriteLine("closestLifePack at " + _closestLifePack.Coordinate.X + ", " + _closestLifePack.Coordinate.Y + " cost: " + _closestLifePack.Cost);
        }

        public void findBestTreasureToFollow()
        {
            if (_closestCoin.CompareTo(_closestLifePack) <= 1)
            {
                _bestToFollow = _closestCoin;

                Console.WriteLine("bestToFollow is the coin at: " + _closestCoin.Coordinate.X + ", " + _closestCoin.Coordinate.Y);               
            }
            else
            {
                _bestToFollow = _closestLifePack;
                Console.WriteLine("bestToFollow is the lifepack at: " + _closestLifePack.Coordinate.X + ", " + _closestLifePack.Coordinate.Y);
            }
        }
        
        
    }
}
