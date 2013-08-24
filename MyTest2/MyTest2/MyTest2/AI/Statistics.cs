using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyTest2.Beans;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using MyTest2.Beans;

namespace MyTest2.AI
{
    class Statistics
    {
        private static Statistics instance;
        private bool _playerUp,_playerDown,_playerRight,_playerLeft;
        private ConcurrentDictionary<Point,CoinPile> coinList;
        private ConcurrentDictionary<Point, Treasure> lifeList;
        List<Path> coinShortestPathList;
        List<Path> lifeShortestPathList;
        private Path bestPath;


        public Statistics()
        {
            _playerUp = false;
            _playerDown = false;
            _playerRight = false;
            _playerLeft = false;
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

        public Path BestPath
        {
            get { return bestPath; }
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

        private void setRealCostsToPath(ref Path path, bool isLifePack)
        {
            int myHealth = Map.getMap.AllTanks[Map.getMap.MyIndex].Health;

            switch(isLifePack)
            {
                case true:
                    if (myHealth < 49)
                    {
                        path.Cost = path.Cost * 1;
                    }
                    else if (myHealth < 79)
                    {
                        path.Cost = path.Cost * 2;
                    }
                    else if (myHealth < 99)
                    {
                        path.Cost = path.Cost * 5;
                    }
                    else if (myHealth < 150)
                    {
                        path.Cost = path.Cost * 8;
                    }
                    else
                    {
                        path.Cost = path.Cost * 9;
                    }
                    break;

                case false:
                    if (myHealth < 49)
                    {
                        path.Cost = path.Cost * 9;
                    }
                    else if (myHealth < 79)
                    {
                        path.Cost = path.Cost * 8;
                    }
                    else if (myHealth < 99)
                    {
                        path.Cost = path.Cost * 5;
                    }
                    else if (myHealth < 150)
                    {
                        path.Cost = path.Cost * 2;
                    }
                    else
                    {
                        path.Cost = path.Cost * 1;
                    }
                    break;
            }
        }

        public void findShortestPathsToTreasures()
        {
            while (true)
            {
                //Console.WriteLine("entered findShortestPathsToTreasures()");

                coinList = Map.getMap.CoinList;
                lifeList = Map.getMap.LifePackList;
                Point me = Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate;
                //Console.WriteLine("Statistics,findShortestPathsToTreasures(), Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate: "+me.X+", "+me.Y);

                /*List<Path> coinShortestPathList = new List<Path>();
                List<Path> lifeShortestPathList = new List<Path>();*/

                coinShortestPathList = new List<Path>();
                lifeShortestPathList = new List<Path>();
                //Console.WriteLine("checking coinList inside findShortestPathsToTreasures()" + coinList.Count);

                foreach (CoinPile c in coinList.Values)
                {
                    //Console.WriteLine("x coord of coinpile: " + c.Coordinate.X);
                    Path shortest = Pathfinder.getPathFinder.getShortestpath(me, c.Coordinate);
                    setRealCostsToPath(ref shortest, false);
                    coinShortestPathList.Add(shortest);
                    //Console.WriteLine("*" + shortest.Cost);
                }

                foreach (Treasure lp in lifeList.Values)
                {
                    //Console.WriteLine("x coord of lifepack: " + lp.Coordinate.X);
                    Path shortest = Pathfinder.getPathFinder.getShortestpath(me, lp.Coordinate);
                    setRealCostsToPath(ref shortest, true);
                    lifeShortestPathList.Add(shortest);
                    //Console.WriteLine("Statistics, checking lifeshortestpathlist, lifeShortestPathList.ElementAt(0).Cost: " + lifeShortestPathList.ElementAt(0).Cost);
                   // Console.WriteLine("*" + shortest.Cost);
                }

                Pathfinder.getPathFinder.sortAcoordingToCosts(ref coinShortestPathList);
                Pathfinder.getPathFinder.sortAcoordingToCosts(ref lifeShortestPathList);

                //Path bestCoinPath = coinShortestPathList.ElementAt(0);
                Path bestCoinPath = null;
                Path bestLifePath = null;
                try
                {
                    bestCoinPath = coinShortestPathList.First();
                    coinShortestPathList = null;
                    //Console.WriteLine(bestCoinPath.Cost);
                    //Path bestLifePath = lifeShortestPathList.ElementAt(0);
                }
                catch (InvalidOperationException ioe)
                {
                   // Console.WriteLine("got invalid operation in assigning best coin path");
                   // Console.WriteLine(ioe.Message);
                }
                try
                {
                    bestLifePath = lifeShortestPathList.First();
                    lifeShortestPathList = null;
                   // Console.WriteLine(bestLifePath.Cost);
                }
                catch (InvalidOperationException ioe)
                {
                   // Console.WriteLine("got invalid operation in assigning best life path");
                  //  Console.WriteLine(ioe.Message);
                }
                if (bestCoinPath != null && bestLifePath != null)
                {

                    if (bestCoinPath.Cost > bestLifePath.Cost)
                    {
                        bestPath = bestLifePath;
                    }
                    else
                    {
                        bestPath = bestCoinPath;
                    }
                }
                else if (bestCoinPath == null && bestLifePath != null) bestPath = bestLifePath;
                else if (bestCoinPath != null && bestLifePath == null) bestPath = bestCoinPath;

            }
        }
    }
}
