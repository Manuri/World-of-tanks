using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MyTest2.AI;
using MyTest2.Beans;

namespace MyTest2
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D backgroundTexture;
        Texture2D[] tankTextures;
        Texture2D[] obstacleTextures;
        Texture2D coin;
        Texture2D lifePack;

        int screenWidth;
        int screenHeight;
        int numberOfPlayers = 5;
        float tankScaling;
        //int widthOfABlock;
        float widthOfABlock;
        int gridLength;
        
        CompleteSquare[,] bd;
        Player[] tanks = new Player[5];
        List<CoinPile> coinList = new List<CoinPile>();
        private List<Treasure> lifePackList = new List<Treasure>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Tank_Game";

            GameManager.getGameManager.sendMessage("JOIN#");
            GameManager.getGameManager.receiveMessage();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            device = graphics.GraphicsDevice;

            backgroundTexture = Content.Load<Texture2D>("background1");

            tankTextures = new Texture2D[numberOfPlayers];

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                tankTextures[i - 1] = Content.Load<Texture2D>(("tank" + i).ToString());
            }

            obstacleTextures = new Texture2D[3];

            obstacleTextures[0] = Content.Load<Texture2D>("brick5");
            obstacleTextures[1] = Content.Load<Texture2D>("stone5");
            obstacleTextures[2] = Content.Load<Texture2D>("water7");

            coin = Content.Load<Texture2D>("coin8");
            lifePack = Content.Load<Texture2D>("lifePack2");

            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            //Console.WriteLine(screenWidth+":"+screenHeight);
           
            //tankScaling = 0.7f;
            gridLength = Map.getMap.GridLength;
            //widthOfABlock = screenWidth / 20;
            widthOfABlock = screenWidth / gridLength;

            //bd = new CompleteSquare[20, 20];
            bd = new CompleteSquare[gridLength, gridLength];

            if (gridLength == 10) tankScaling = 1f;
            else tankScaling = 0.7f;

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            gridLength = Map.getMap.GridLength;

                for (int i = 0; i < gridLength; i++)
                {
                    for (int j = 0; j < gridLength; j++)
                    {                       
                        bd[i, j] = Map.getMap.BoardBlocks[i, j];                      
                    }
                }
               
            setUpTanks();

            spriteBatch.Begin();
            drawGrid();
            drawAllObstacles();
            drawTanks();
            drawCoinPiles();
            drawLifePacks();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawGrid()
        {
            Rectangle gridRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            spriteBatch.Draw(backgroundTexture, gridRectangle, Color.White);

        }

        private void drawObstacle(int x, int y, int type)
        {
            int width = (int)widthOfABlock;
           // Rectangle brickRectangle = new Rectangle(widthOfABlock * x, widthOfABlock * y, widthOfABlock, widthOfABlock);
            Rectangle brickRectangle = new Rectangle(width * x, width * y, width, width);

            spriteBatch.Draw(obstacleTextures[type], brickRectangle, Color.White);
        }

        private void drawAllObstacles()
        {
            int gridLength = Map.getMap.GridLength;

            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    if (bd[i,j] !=null && bd[i, j].ObstaclePresent)
                    {
                        switch (bd[i, j].ContentCode)
                        {
                            case SquareContent.Brick: drawObstacle(i, j, 0);
                                break;

                            case SquareContent.Stone: drawObstacle(i, j, 1);
                                break;

                            case SquareContent.Water: drawObstacle(i, j, 2);
                                break;

                            default: break;
                        }
                    }
                }
            }
        }

        private void drawTanks()
        {
            int i = 0;

            foreach (Player tank in tanks)
            {
                if (tank!=null && tank.IsAlive)
                {
                    Vector2 tankOrigin = new Vector2(tankTextures[i].Bounds.Center.X, tankTextures[i].Bounds.Center.Y);
                    spriteBatch.Draw(tankTextures[i], tank.ScreenPosition, null, Color.White, (MathHelper.PiOver2)*(tank.Direction), tankOrigin, tankScaling, SpriteEffects.None, 0);
                    i++;
                }
            }
        }

        private void setUpTanks()
        {
            tanks = Map.getMap.AllTanks;
            int gridLength = Map.getMap.GridLength;

            foreach (Player tank in tanks)
            {
                if (tank != null)
                {
                    //Console.WriteLine("not null");

                    //tank.ScreenPosition = new Vector2((tank.Coordinate.X) * (screenWidth/20) + 17.5f, (tank.Coordinate.Y) * (screenHeight/20) + 17.5f);
                    tank.ScreenPosition = new Vector2((tank.Coordinate.X) * widthOfABlock + widthOfABlock / 2, (tank.Coordinate.Y) * widthOfABlock + widthOfABlock / 2);

                    //Console.WriteLine(tank.Coordinate.X + ", " + tank.Coordinate.Y);
                    //Console.WriteLine(tank.ScreenPosition.X + ": " + tank.ScreenPosition.Y);
                }
            }

        }

        private void drawCoinPiles()
        {
            coinList = Map.getMap.CoinList;
            int width = (int)widthOfABlock;

            Rectangle coinRectangle; 
            /*foreach (CoinPile coinpile in coinList) 
            {
                if (coinpile.IsPresent)
                {
                    coinRectangle = new Rectangle(width * coinpile.Coordinate.X, width * coinpile.Coordinate.Y, width, width);
                    spriteBatch.Draw(coin, coinRectangle, Color.White);
                }
            }*/

            for (int i = 0; i < coinList.Count; i++)
            {
                CoinPile c = coinList.ElementAt(i);
                if (c.IsPresent)
                {
                    coinRectangle = new Rectangle(width * c.Coordinate.X, width * c.Coordinate.Y, width, width);
                    spriteBatch.Draw(coin, coinRectangle, Color.White);
                }
            }
        }

        private void drawLifePacks()
        {
            lifePackList = Map.getMap.LifePackList;
            int width = (int)widthOfABlock;

            Rectangle lifePackRectangle;
           /* foreach (Treasure lifepack in lifePackList)
            {
                if (lifepack.IsPresent)
                {
                    lifePackRectangle = new Rectangle(width * lifepack.Coordinate.X, width * lifepack.Coordinate.Y, width, width);
                    spriteBatch.Draw(lifePack, lifePackRectangle, Color.White);
                }
            }*/

            for (int i = 0; i < lifePackList.Count; i++)
            {
                Treasure lp = lifePackList.ElementAt(i);
                if (lp.IsPresent)
                {
                    lifePackRectangle = new Rectangle(width * lp.Coordinate.X, width * lp.Coordinate.Y, width, width);
                    spriteBatch.Draw(lifePack, lifePackRectangle, Color.White);
                }
            }
        }

       
    } 
}
