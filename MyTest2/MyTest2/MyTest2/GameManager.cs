using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyTest2.Beans;
using MyTest2.AI;
using Microsoft.Xna.Framework;
using MyTest2.Utilities;

namespace MyTest2
{
    class GameManager 
    {
        private static GameManager instance;

        public static GameManager getGameManager
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void sendMessage(String message)
        {
            Communicator.getCommunicator.send(message);
        }

        public void receiveMessage()
        {
            Thread receiver = new Thread(Utilities.Communicator.getCommunicator.receive);
            receiver.Start();
        }

        public void decodeMessage(Object obj)
        {
            String line = (String)obj;
            Console.WriteLine(line);
            String[] token = line.Split(':');
            char letter;
            if (token[0].Length == 1)
            {
                letter = Convert.ToChar(token[0]);
                switch (letter)
                {
                    case 'I':
                        createMyPlayerInMap(token);
                        setUpMap(token);
                        break;

                    case 'S':
                        //createMyPlayerInMap(token);
                        setUpPlayers(token);
                        startMyPlayer();
                        break;

                    case 'G':
                        updateMap(token);
                        break;

                    case 'C':
                        addCoinsToMap(token);
                        break;

                    case 'L':
                        addLifePacksToMap(token);
                        break;

                    default: 
                        break;
                }
            }
            else
            {
                //server has given an error message
                //errorHandler(line);
            }

        }

        public void setUpMap(String[] token)
        {
            /*setUpPlayers(token);
            createBlocks(token,6,SquareContent.Brick);
            createBlocks(token, 7, SquareContent.Stone);
            createBlocks(token, 8, SquareContent.Water);
            createEmptyBlocks();*/

            createBlocks(token,2,SquareContent.Brick);
            createBlocks(token, 3, SquareContent.Stone);
            createBlocks(token, 4, SquareContent.Water);
            createEmptyBlocks();
        }


        public void createMyPlayerInMap(String[] token)
        {
            int index = Convert.ToInt32(token[1].ToCharArray()[1])-48;
          /*  int x = Convert.ToInt32(token[2].ToCharArray()[1]);
            int y = Convert.ToInt32(token[2].ToCharArray()[3]);
            int direction = Convert.ToInt32(token[3].ToCharArray()[0]);*/

            Map.getMap.MyIndex = index;
            //Console.WriteLine("My tank's indexNo: " + Map.getMap.MyIndex);
        }
        
        private void setUpPlayers(String[] token)
        {
            Map.getMap.NoOfPlayers = token.Length-1;
            int noOfPlayers = Map.getMap.NoOfPlayers;

            String[] playerInfo = new String[noOfPlayers];

            for (int i = 0; i < noOfPlayers; i++)
            {
                playerInfo[i] = token[i + 1];
            }

            Player[] tanks = new Player[noOfPlayers];
            tanks = Map.getMap.AllTanks;

            for (int j = 0; j < noOfPlayers; j++)
            {
               /* int index = Convert.ToInt32(playerInfo[j].ToCharArray()[1]);
                int x = Convert.ToInt32(playerInfo[j].ToCharArray()[3]);
                int y = Convert.ToInt32(playerInfo[j].ToCharArray()[5]);
                int direction = Convert.ToInt32(playerInfo[j].ToCharArray()[7]);*/

                String[] broken = playerInfo[j].Split(';');

                int index = int.Parse(broken[0].Split('P')[1]);
                int x = int.Parse(broken[1].Split(',')[0]);
                int y = int.Parse(broken[1].Split(',')[1]);
                int direction = int.Parse(broken[2]);

                tanks[j] = new Player(index, x, y, direction);
                //Console.WriteLine("Player added. IndexNo: " + tanks[j].PlayerIndexNo + " at " + tanks[j].Coordinate.X + ", " + tanks[j].Coordinate.Y);
            }
        }

        private void startMyPlayer()
        {
            Thread playerStatUpdater = new Thread(Statistics.getStatistics.checkPlayers);
            playerStatUpdater.Start();

            Thread treasureStatUpdater = new Thread(Statistics.getStatistics.findShortestPathsToTreasures);
            treasureStatUpdater.Start();

            Thread aiOperator = new Thread(AIBrain.getAI.starter);
            aiOperator.Start();
        }

        private void createBlocks(String[] theToken, int tokenIndex, SquareContent content)
        {
            String[] blockCoords = theToken[tokenIndex].Split(';');
            int x, y;
            foreach (String coordinate in blockCoords)
            {
                x = int.Parse(coordinate.Split(',')[0]);
                y = int.Parse(coordinate.Split(',')[1]);

                Map.getMap.BoardBlocks[x, y] = new CompleteSquare(x, y, content);
                Map.getMap.BoardBlocks[x, y].ObstaclePresent = true;
                //Console.WriteLine(Map.getMap.BoardBlocks[x, y].ContentCode + " added at (" + Map.getMap.BoardBlocks[x, y].Coordinate.X+ " ," + Map.getMap.BoardBlocks[x, y].Coordinate.Y+")");
            }
        }

        private void createEmptyBlocks()
        {
            int gridLength = Map.getMap.GridLength;

            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    if (Map.getMap.BoardBlocks[i, j] == null)
                    {
                        Map.getMap.BoardBlocks[i, j] = new CompleteSquare(i, j, SquareContent.Empty);
                        //Console.WriteLine(Map.getMap.BoardBlocks[i, j].ContentCode + " added at (" + Map.getMap.BoardBlocks[i, j].Coordinate.X + " ," + Map.getMap.BoardBlocks[i, j].Coordinate.Y + ")");
                    }
                }
            }
        }


        public void updateMap(String[] token)
        {
            updatePlayers(token);
            updateBricks(token);
        }

        private void updatePlayers(String[] token)
        {
            int noOfPlayers = Map.getMap.NoOfPlayers;

            String[] playerInfo = new String[noOfPlayers];
            for (int i = 0; i < noOfPlayers; i++)
            {
                playerInfo[i] = token[i + 1];
            }

            Player[] tanks = new Player[noOfPlayers];
            tanks = Map.getMap.AllTanks;

            for (int j = 0; j < noOfPlayers; j++)
            {
                /*int x = playerInfo[j].ToCharArray()[3];
                int y = playerInfo[j].ToCharArray()[5];
                int direction = playerInfo[j].ToCharArray()[7];
                int shot =  playerInfo[j].ToCharArray()[9];*/

                String[] broken = playerInfo[j].Split(';');

                int x = int.Parse(broken[1].Split(',')[0]);
                int y = int.Parse(broken[1].Split(',')[1]);
                int direction = int.Parse(broken[2]);
                int shot = int.Parse(broken[3]);

                int health = int.Parse(broken[4]);
                int coins = int.Parse(broken[5]);
                int points = int.Parse(broken[6]);

                tanks[j].Coordinate = new Point(x, y);
                tanks[j].Direction = direction;
                if (shot == 1)
                {
                    tanks[j].Shot = true;
                }
                else
                {
                    tanks[j].Shot = false;
                }
                tanks[j].Health = health;
                if (health == 0)
                    tanks[j].IsAlive = false;
                tanks[j].Coins = coins;
                tanks[j].Score = points;

                
                removeLifePacksIfAPlayerSteppedOn(tanks[j]);
                removeCoinsIfAPlayerSteppedOn(tanks[j]);
            }
        }


        private void removeCoinsIfAPlayerSteppedOn(Player tank)
        {
            if (Map.getMap.CoinList.ContainsKey(tank.Coordinate))
            {              
                //Map.getMap.CoinList.Remove(tank.Coordinate);
                CoinPile value;
                Map.getMap.CoinList.TryRemove(tank.Coordinate,out value);
            }
        }

        private void removeLifePacksIfAPlayerSteppedOn(Player tank)
        {
            if (Map.getMap.LifePackList.ContainsKey(tank.Coordinate))
            {
                //Map.getMap.LifePackList.Remove(tank.Coordinate);
                Treasure value;
                Map.getMap.LifePackList.TryRemove(tank.Coordinate,out value);
            }


        }

        private void updateBricks(String[] token)
        {
            String[] brickInfo = token[Map.getMap.NoOfPlayers+1].Split(';');
            int x, y,damage;
            foreach (String info in brickInfo)
            {
                String[] information = info.Split(',');

                x = int.Parse(information[0]);
                y = int.Parse(information[1]);
                damage = int.Parse(information[2]);

                Map.getMap.BoardBlocks[x, y].DamageLevel = damage;
                if (damage == 4)
                {
                    Map.getMap.BoardBlocks[x, y].ObstaclePresent = false;
                }
            }
        }


        public void addCoinsToMap(String[] token)
        {
            int x, y, lifeTime, value;

            /*for the message format given in slides
             *
             * 
            String[] coinInfo = token[1].Split(',');
            x = int.Parse(coinInfo[0]);
            y = int.Parse(coinInfo[1]);
            lifeTime = int.Parse(coinInfo[2]);
            value = int.Parse(coinInfo[3]);*/

            x = int.Parse(token[1].Split(',')[0]);
            y = int.Parse(token[1].Split(',')[1]);
            lifeTime = int.Parse(token[2]);
            value = int.Parse(token[3]);

            Map.getMap.CoinList.AddOrUpdate(new Point(x, y), new CoinPile(x, y, value, lifeTime), (k, v) => new CoinPile(x, y, value, lifeTime));
            //Console.WriteLine("Coinpile added. Value: "+value);
        }

        public void addLifePacksToMap(String[] token)
        {
            int x, y, lifeTime;

            /* for the message format given in slides
             * 
             * String[] coinInfo = token[1].Split(',');

            x = int.Parse(coinInfo[0]);
            y = int.Parse(coinInfo[1]);
            lifeTime = int.Parse(coinInfo[2]);*/

            x = int.Parse(token[1].Split(',')[0]);
            y = int.Parse(token[1].Split(',')[1]);
            lifeTime = int.Parse(token[2]);

            //Map.getMap.LifePackList.Add(new Treasure(x,y,lifeTime));
           // Map.getMap.LifePackList.Add(new Point(x,y), new Treasure(x, y, lifeTime));
            Map.getMap.LifePackList.AddOrUpdate(new Point(x, y), new Treasure(x, y, lifeTime), (k, v) => new Treasure(x, y, lifeTime));

           // Console.WriteLine("LifePack added. It's lifetime: "+lifeTime);
        }

        public void removeCoinsFromMap(CoinPile coin)
        {
            //this needs to be called when a timer expires
            CoinPile value; 
            Map.getMap.CoinList.TryRemove(coin.Coordinate, out value);
            //Console.WriteLine("coinpile removed.value: "+coin.Value);

        }

        public void removeLifepacksFromMap(Treasure lifePack)
        {
            //this needs to be called when a timer expires

            //Map.getMap.LifePackList.Remove(lifePack.Coordinate);
            Treasure value; 
            Map.getMap.LifePackList.TryRemove(lifePack.Coordinate,out value);

            //Console.WriteLine("lifepack removed.lifetime: " + lifePack.LifeTime);
        }

        private void errorHandler(String message)
        {
            /*if (message == "TOO_QUICK" || message == "CELL_OCCUPIED")
            {
               // AIBrain.getAI.theTimer.Stop();
                if (AIBrain.getAI.CommandNumber > 0)
                {
                   // AIBrain.getAI.theTimer.Stop();
                    AIBrain.getAI.CommandNumber--;

                }
               // AIBrain.getAI.theTimer.Start();
                Console.WriteLine("oops "+message);
            }*/
        }
    }
}
