using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Collections;
using System.Threading;
using Microsoft.Xna.Framework;
using MyTest2.Beans;

namespace MyTest2.AI
{
    class AIBrain
    {
        private static AIBrain instance;

        String[] messages;


        public AIBrain()
        {
            messages = new String[5];

            messages[0] = "UP#";
            messages[1] = "DOWN#";
            messages[2] = "RIGHT#";
            messages[3] = "LEFT#";
            messages[4] = "SHOOT#";

        }

        public static AIBrain getAI
        {
            get
            {
                if (instance == null)
                {
                    instance = new AIBrain();
                }
                return instance;
            }
        }



        public void starter()
        {
            /*  while (true)
              {
                 // if (Statistics.getStatistics.PlayerUp || Statistics.getStatistics.PlayerRight || Statistics.getStatistics.PlayerDown || Statistics.getStatistics.PlayerLeft)
                 // {
                  //    shoot();
                      //Console.WriteLine("entered starter");
                 // }
                 // else
                 // {
                      Move();
                      //Console.WriteLine("entered starter");
                 // }
                  Thread.Sleep(1000);
              }*/
            System.Timers.Timer aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
            GC.KeepAlive(aTimer);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Move();
        }
        private void shoot()
        {
            Player me = Map.getMap.AllTanks[Map.getMap.MyIndex];

            Console.WriteLine("entered shoot");
            if (Statistics.getStatistics.PlayerUp)
            {
                switch (me.Direction)
                {
                    case 0:
                        GameManager.getGameManager.sendMessage("SHOOT#");
                        Console.WriteLine("shoot");
                        break;
                    default:
                        GameManager.getGameManager.sendMessage("UP#");
                        Console.WriteLine("up");
                        break;
                }
            }
            else if (Statistics.getStatistics.PlayerRight)
            {
                switch (me.Direction)
                {
                    case 1:
                        GameManager.getGameManager.sendMessage("SHOOT#");
                        Console.WriteLine("shoot");
                        break;
                    default:
                        GameManager.getGameManager.sendMessage("RIGHT#");
                        Console.WriteLine("right");
                        break;
                }
            }
            else if (Statistics.getStatistics.PlayerDown)
            {
                switch (me.Direction)
                {
                    case 2:
                        GameManager.getGameManager.sendMessage("SHOOT#");
                        Console.WriteLine("shoot");
                        break;
                    default:
                        GameManager.getGameManager.sendMessage("DOWN#");
                        Console.WriteLine("down");
                        break;
                }
            }
            else if (Statistics.getStatistics.PlayerLeft)
            {
                switch (me.Direction)
                {
                    case 3:
                        GameManager.getGameManager.sendMessage("SHOOT#");
                        Console.WriteLine("shoot");
                        break;
                    default:
                        GameManager.getGameManager.sendMessage("LEFT#");
                        Console.WriteLine("left");
                        break;
                }
            }

        }

        private void Move()
        {
            Statistics.getStatistics.findLeastDistanceTreasures();

            if (Statistics.getStatistics._closestCoin.Cost != 10000 || Statistics.getStatistics._closestLifePack.Cost != 10000)
            {
               /* Pathfinder.getPathFinder.HighlightPath(ref Statistics.getStatistics._closestCoin);
                Pathfinder.getPathFinder.HighlightPath(ref Statistics.getStatistics._closestLifePack);

                Statistics.getStatistics.setRealCostsToPath(ref Statistics.getStatistics._closestCoin, false);
                Statistics.getStatistics.setRealCostsToPath(ref Statistics.getStatistics._closestLifePack, true);

                Statistics.getStatistics.findBestTreasureToFollow();*/

                /*
                 * First set real cost to closest coin and closest lifepack. Then find the best one to follow. 
                 * Then mark it's path.
                 */

                Statistics.getStatistics.setRealCostsToPath(ref Statistics.getStatistics._closestCoin, false);
                Statistics.getStatistics.setRealCostsToPath(ref Statistics.getStatistics._closestLifePack, true);

                Statistics.getStatistics.findBestTreasureToFollow();

                Pathfinder.getPathFinder.HighlightPath(ref Statistics.getStatistics._bestToFollow);

                Point block;
                if (Statistics.getStatistics._bestToFollow != null)
                {
                    if (Statistics.getStatistics._bestToFollow.Path != null)
                    {
                        if (Statistics.getStatistics._bestToFollow.Path.First != null)
                        {
                            block = Statistics.getStatistics._bestToFollow.Path.First.Next.Value;

                            Console.WriteLine("move to " + block.X + ", " + block.Y);
                            Point me = Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate;
                            Console.WriteLine("I'm at " + me.X + ", " + me.Y);
                            if (me.X < block.X)
                            {
                                GameManager.getGameManager.sendMessage("RIGHT#");
                                Console.WriteLine("right");
                            }
                            else if (me.X > block.X)
                            {
                                GameManager.getGameManager.sendMessage("LEFT#");
                                Console.WriteLine("left");
                            }
                            else if (me.Y < block.Y)
                            {
                                GameManager.getGameManager.sendMessage("DOWN#");
                                Console.WriteLine("down");
                            }
                            else if (me.Y > block.Y)
                            {
                                GameManager.getGameManager.sendMessage("UP#");
                                Console.WriteLine("up");
                            }
                        }
                    }
                }

            }

        }
    }
}
