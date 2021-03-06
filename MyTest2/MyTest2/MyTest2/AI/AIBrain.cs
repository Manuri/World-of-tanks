﻿using System;
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
        private int previousGlobalUpdate;


        public AIBrain()
        {

            previousGlobalUpdate = 0;

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
              while (true)
              {
                  if (GameManager.getGameManager.globalUpdateCounter > previousGlobalUpdate)
                 {
                      Statistics.getStatistics.checkPlayers();
                      if (Statistics.getStatistics.PlayerUp || Statistics.getStatistics.PlayerRight || Statistics.getStatistics.PlayerDown || Statistics.getStatistics.PlayerLeft)
                      {
                          shoot();
                                    
                      }

                      else
                      {
                          Console.WriteLine("previousGlobalUpdate " + previousGlobalUpdate);
                          Console.WriteLine("globalUpdateCounter " + GameManager.getGameManager.globalUpdateCounter);
                          Statistics.getStatistics.decideTheMove();
                          Move();
                          previousGlobalUpdate++;
                      }
                  }
              }
       

        private void shoot()
        {
            Player me = Map.getMap.AllTanks[Map.getMap.MyIndex];

           // Console.WriteLine("entered shoot");
            if (Statistics.getStatistics.PlayerUp)
            {


                if (me.Direction == 0)
                {
                    GameManager.getGameManager.sendMessage("SHOOT#");
                    Console.WriteLine("shooting");
                }
                else if (GameManager.getGameManager.globalUpdateCounter > previousGlobalUpdate)
                {
                    GameManager.getGameManager.sendMessage("UP#");
                    previousGlobalUpdate++;
                    Console.WriteLine("up");
                }
            }
            else if (Statistics.getStatistics.PlayerRight)
            {

                if (me.Direction == 1)
                {
                    GameManager.getGameManager.sendMessage("SHOOT#");
                    Console.WriteLine("shooting");
                }
                else if (GameManager.getGameManager.globalUpdateCounter > previousGlobalUpdate)
                {
                    GameManager.getGameManager.sendMessage("RIGHT#");
                    previousGlobalUpdate++;
                    Console.WriteLine("right");
                }
            }
            else if (Statistics.getStatistics.PlayerDown)
            {

                if (me.Direction == 2)
                {
                    GameManager.getGameManager.sendMessage("SHOOT#");
                    Console.WriteLine("shooting");
                }
                else if (GameManager.getGameManager.globalUpdateCounter > previousGlobalUpdate)
                {
                    GameManager.getGameManager.sendMessage("DOWN#");
                    previousGlobalUpdate++;
                    Console.WriteLine("down");
                }
            }
            else if (Statistics.getStatistics.PlayerLeft)
            {

                if (me.Direction == 3)
                {
                    GameManager.getGameManager.sendMessage("SHOOT#");
                    Console.WriteLine("shooting");
                }
                else if (GameManager.getGameManager.globalUpdateCounter > previousGlobalUpdate)
                {
                    GameManager.getGameManager.sendMessage("LEFT#");
                    previousGlobalUpdate++;
                    Console.WriteLine("left");
                }
            }

        }

        public void Move()
        {
                Point block;
                if (Statistics.getStatistics._bestToFollow != null)
                {
                    if (Statistics.getStatistics._bestToFollow.Path != null)
                    {
                        if (Statistics.getStatistics._bestToFollow.Path.First != null)
                        {
                            if (Statistics.getStatistics._bestToFollow.Path.First.Next != null)
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
