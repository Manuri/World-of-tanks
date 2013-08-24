using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Collections;
using System.Threading;
using Microsoft.Xna.Framework;

namespace MyTest2.AI
{
    class AIBrain
    {
        private static AIBrain instance;

        String[] messages;
        List<int> commandArray;
        int _commandNumber=0;


        public AIBrain()
        {
            messages = new String[5];
            commandArray = new List<int>();

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

        public int CommandNumber
        {
            get{return _commandNumber;}
            set { _commandNumber = value; }
        }

        public void starter()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true; 
            /*while (true)
            {
                GameManager.getGameManager.sendMessage(messages[commandArray.ElementAt(_commandNumber)]);
                _commandNumber++;
                Thread.Sleep(2000);
            }*/
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (commandArray.Count>=CommandNumber+1)
            {
                if (checkAround())
                {
                    GameManager.getGameManager.sendMessage(messages[commandArray.ElementAt(_commandNumber)]);
                    _commandNumber++;
                    Console.WriteLine(CommandNumber);
                }
            }
        }

        public void nextMessage()
        {
            while (true)
            {
                try
                {
                    commandArray.Add(2);
                    Thread.Sleep(10);
                    commandArray.Add(1);
                    Thread.Sleep(10);
                    commandArray.Add(3);
                    Thread.Sleep(10);
                    commandArray.Add(0);                   
                    Thread.Sleep(10);
                }
                catch (OutOfMemoryException e)
                {
                    //Console.WriteLine(commandArray.Count);
                }
            }
        }

        private bool checkAround()
        {
            Console.WriteLine("My index: "+Map.getMap.MyIndex);
            Point p = Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate;
            switch (CommandNumber)
            {
                case 0: if (Map.getMap.BoardBlocks[p.X, p.Y+1].ContentCode == SquareContent.Empty)
                    {
                        return true;
                    }
                    break;

                case 1: if (Map.getMap.BoardBlocks[p.X+1, p.Y].ContentCode == SquareContent.Empty)
                    {
                        return true;
                    }
                    break;
                case 2: if (Map.getMap.BoardBlocks[p.X, p.Y - 1].ContentCode == SquareContent.Empty)
                    {
                        return true;
                    }
                    break;
                case 3: if (Map.getMap.BoardBlocks[p.X - 1, p.Y].ContentCode == SquareContent.Empty)
                    {
                        return true;
                    }
                    break;
            }
            return true;
        }




    }
}
