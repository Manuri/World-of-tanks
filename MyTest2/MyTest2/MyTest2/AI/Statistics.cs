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
        private int deadCount;
        private static Player myPlayer;
        private static Player[] all;
        private static Player[] enemies;

        public Statistics()
        {
            _playerUp = false;
            _playerDown = false;
            _playerRight = false;
            _playerLeft = false;

            _closestCoin = new ClosestTreasure();
            _closestLifePack = new ClosestTreasure();

            deadCount = 0;
            myPlayer = Map.getMap.AllTanks[Map.getMap.MyIndex];
            all = Map.getMap.AllTanks;
            enemies = new Player[Map.getMap.NoOfPlayers - 1];

            int j = 0;
            for (int i = 0; i < Map.getMap.NoOfPlayers; i++)
            {
                if (all[i].Coordinate != all[Map.getMap.MyIndex].Coordinate)
                {
                    enemies[j] = all[i];
                    j++;
                }
            }
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

              //  while (true)
              //  {
            _playerUp = false;
            _playerDown = false;
            _playerLeft = false;
            _playerRight = false;

                if (deadCount != enemies.Length)
                {
                    deadCount = 0;
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (enemies[i].IsAlive)
                        {
                            if (enemies[i].Coordinate.X == myPlayer.Coordinate.X)
                            {

                                if (enemies[i].Coordinate.Y > myPlayer.Coordinate.Y)
                                {

                                    _playerDown = true;
                                }

                                else
                                    _playerUp = true;

                            }
                            if (enemies[i].Coordinate.Y == myPlayer.Coordinate.Y)
                            {

                                if (enemies[i].Coordinate.X > myPlayer.Coordinate.X )
                                {
                                    _playerRight = true;
                                }

                                else
                                     _playerLeft = true;
                            }


                        }
                        else
                        {
                            deadCount++;
                            Console.WriteLine("deadcount: "+deadCount);
                            if (deadCount == enemies.Length)
                            {
                                _playerUp = false;
                                _playerDown = false;
                                _playerLeft = false;
                                _playerRight = false;
                                // break;
                            }
                        }

                    }
                    // }
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
            int x1=0;
            int y1=0;
            int x2 = 0;
            int y2 = 0;
            foreach (CoinPile c in Map.getMap.CoinList.Values)
            {
                x1 = c.Coordinate.X;
                y1 = c.Coordinate.Y;

                if (Pathfinder.getPathFinder.Squares[x1, y1].DistanceSteps < coinMin)
                {
                    coinMin = Pathfinder.getPathFinder.Squares[x1, y1].DistanceSteps;
//                    Console.WriteLine(coinMin);
                }
            }
            _closestCoin.Coordinate = Pathfinder.getPathFinder.Squares[x1, y1].Coordinate;
            _closestCoin.Cost = coinMin;
            Console.WriteLine("closestCoin at " + _closestCoin.Coordinate.X + ", " + _closestCoin.Coordinate.Y + " cost: " + _closestCoin.Cost);

            foreach (Treasure lp in Map.getMap.LifePackList.Values)
            {
                x2 = lp.Coordinate.X;
                y2 = lp.Coordinate.Y;

                if (Pathfinder.getPathFinder.Squares[x2, y2].DistanceSteps < lifePackMin)
                {
                    lifePackMin = Pathfinder.getPathFinder.Squares[x2, y2].DistanceSteps;
//                    Console.WriteLine(lifePackMin);
                }
            }
            _closestLifePack.Coordinate = Pathfinder.getPathFinder.Squares[x2, y2].Coordinate;
            _closestLifePack.Cost = lifePackMin;
            Console.WriteLine("closestLifePack at " + _closestLifePack.Coordinate.X + ", " + _closestLifePack.Coordinate.Y + " cost: " + _closestLifePack.Cost);
        }

        public void findBestTreasureToFollow()
        {
            if (_closestCoin.CompareTo(_closestLifePack) >= 0)
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

        public void decideTheMove()
        {

                Pathfinder.getPathFinder.Pathfind();
                findLeastDistanceTreasures();

                if (_closestCoin.Cost != 10000 || _closestLifePack.Cost != 10000)
                {
                    /*
                     * First set real cost to closest coin and closest lifepack. Then find the best one to follow. 
                     * Then mark it's path.
                     */

                    setRealCostsToPath(ref _closestCoin, false);
                    setRealCostsToPath(ref _closestLifePack, true);

                    findBestTreasureToFollow();

                    _bestToFollow.Path.Clear();

                    Pathfinder.getPathFinder.HighlightPath(ref _bestToFollow);

                    Pathfinder.getPathFinder.ClearLogic();

                }
           // }
        }
        
        
    }
}
