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
using System.Collections.Concurrent;

using MyTest2.AI;
using MyTest2.Beans;

namespace MyTest2
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont font;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D grid;
        Texture2D backgroundTexture;
        Texture2D[] tankTextures;
        Texture2D[] obstacleTextures;
        Texture2D coin;
        Texture2D lifePack;
    
        int screenWidth;
        int screenHeight;
        int numberOfPlayers=5;
        float tankScaling;
        float widthOfABlock;
        int gridLength;
        
        CompleteSquare[,] bd;
        Player[] tanks=new Player[5];

        List<CoinPile> pilesToDraw ;
        List<Treasure> lifePacksToDraw ;

        //private Dictionary<Point, CoinPile> coinList = new Dictionary<Point, CoinPile>();
        //private ConcurrentDictionary<Point, CoinPile> coinList = new ConcurrentDictionary<Point, CoinPile>();

        //private Dictionary<Point, Treasure> lifePackList = new Dictionary<Point, Treasure>();
        //private ConcurrentDictionary<Point, Treasure> lifePackList = new ConcurrentDictionary<Point, Treasure>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1100;
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
            font = Content.Load<SpriteFont>("myFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            device = graphics.GraphicsDevice;

            backgroundTexture = Content.Load<Texture2D>("background1");

            grid = Content.Load<Texture2D>("background");

            tankTextures = new Texture2D[numberOfPlayers];

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                tankTextures[i - 1] = Content.Load<Texture2D>(("tank" + i).ToString());
            }

            obstacleTextures = new Texture2D[3];

            obstacleTextures[0] = Content.Load<Texture2D>("brick5");
            obstacleTextures[1] = Content.Load<Texture2D>("stone5");
            obstacleTextures[2] = Content.Load<Texture2D>("water10");

            coin = Content.Load<Texture2D>("coin8");
            lifePack = Content.Load<Texture2D>("lifePack2");

            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
           
            //tankScaling = 0.7f;
            gridLength = Map.getMap.GridLength;
            //widthOfABlock = screenWidth / 20;
            widthOfABlock = 700 / gridLength;

            //bd = new CompleteSquare[20, 20];
            bd = new CompleteSquare[gridLength, gridLength];
            pilesToDraw = new List<CoinPile>();
            lifePacksToDraw = new List<Treasure>();

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
            drawScenary();
            drawAllObstacles();
            drawTanks();
            drawCoinPiles();
            drawLifePacks();
            DrawText();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawGrid()
        {
            Rectangle gridRectangle = new Rectangle(345, 0, screenWidth, screenHeight);

            spriteBatch.Draw(grid, gridRectangle, Color.White);

        }

        private void drawScenary()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, 700, 700);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
        }

        private void drawObstacle(int x, int y, int type)
        {
            int width = (int)widthOfABlock;
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

            if (tanks != null)
            {
                foreach (Player tank in tanks)
                {
                    if (tank != null && tank.IsAlive)
                    {
                        Vector2 tankOrigin = new Vector2(tankTextures[i].Bounds.Center.X, tankTextures[i].Bounds.Center.Y);
                        spriteBatch.Draw(tankTextures[i], tank.ScreenPosition, null, Color.White, (MathHelper.PiOver2) * (tank.Direction), tankOrigin, tankScaling, SpriteEffects.None, 0);
                        i++;
                    }
                }
            }
        }

        private void setUpTanks()
        {
            tanks = Map.getMap.AllTanks;
            int gridLength = Map.getMap.GridLength;
            if (tanks != null)
            {
                foreach (Player tank in tanks)
                {
                    if (tank != null)
                    {
                        tank.ScreenPosition = new Vector2((tank.Coordinate.X) * widthOfABlock + widthOfABlock / 2, (tank.Coordinate.Y) * widthOfABlock + widthOfABlock / 2);
                    }
                }
            }

        }

        private void drawCoinPiles()
        {

           // coinList = Map.getMap.CoinList;
            int width = (int)widthOfABlock;

            Rectangle coinRectangle;

           /* foreach(var pair in coinList)
            {
                if (pair.Value.IsPresent)
                {
                    coinRectangle = new Rectangle(width * pair.Value.Coordinate.X, width * pair.Value.Coordinate.Y, width, width);
                    spriteBatch.Draw(coin, coinRectangle, Color.White);
                }
            }*/

            //var pilesToDraw = new List<CoinPile>();
           
            //pilesToDraw = coinList.Values.Where(p => p.IsPresent).ToList();
            pilesToDraw = Map.getMap.CoinList.Values.Where(p => p.IsPresent).ToList();

            foreach (var c in pilesToDraw)
            {
                coinRectangle = new Rectangle(width * c.Coordinate.X, width * c.Coordinate.Y, width, width);
                spriteBatch.Draw(coin, coinRectangle, Color.White);
            }
            pilesToDraw = null; //to avoid OOM Exception. dont know whether this will work
        }

        private void drawLifePacks()
        {
            //lifePackList = Map.getMap.LifePackList;
            int width = (int)widthOfABlock;

            Rectangle lifePackRectangle;

            /*foreach (var pair in lifePackList)
            {
               if (pair.Value.IsPresent)
               {
                  lifePackRectangle = new Rectangle(width * pair.Value.Coordinate.X, width * pair.Value.Coordinate.Y, width, width);
                  spriteBatch.Draw(lifePack, lifePackRectangle, Color.White);
               }
            }*/            

            //lifePacksToDraw = lifePackList.Values.Where(lp => lp.IsPresent).ToList();
            lifePacksToDraw = Map.getMap.LifePackList.Values.Where(lp => lp.IsPresent).ToList();

            foreach (var lp in lifePacksToDraw)
            {
                lifePackRectangle = new Rectangle(width * lp.Coordinate.X, width * lp.Coordinate.Y, width, width);
                spriteBatch.Draw(lifePack, lifePackRectangle, Color.White);
            }
            lifePacksToDraw = null;
        }

        private void DrawText()
        {
            tanks = Map.getMap.AllTanks;
            spriteBatch.DrawString(font, "PlayerID", new Vector2(750, 400), Color.White);
            spriteBatch.DrawString(font, "Coins", new Vector2(865,400), Color.White);
            spriteBatch.DrawString(font, "Health", new Vector2(935, 400), Color.White);
            spriteBatch.DrawString(font, "Points", new Vector2(1015, 400), Color.White);
            spriteBatch.DrawString(font, "P0", new Vector2(750, 430), Color.White);
            spriteBatch.DrawString(font, "P1", new Vector2(750, 450), Color.White);
            spriteBatch.DrawString(font, "P2", new Vector2(750, 470), Color.White);
            spriteBatch.DrawString(font, "P3", new Vector2(750, 490), Color.White);
            spriteBatch.DrawString(font, "P4", new Vector2(750, 510), Color.White);
            int i = 430;

            for (int j = 0; j < Map.getMap.NoOfPlayers;j++ )
            {
                if (tanks[j] != null)
                {
                    spriteBatch.DrawString(font, tanks[j].Score.ToString(), new Vector2(1015, i), Color.White);
                    spriteBatch.DrawString(font, tanks[j].Health.ToString(), new Vector2(935, i), Color.White);
                    spriteBatch.DrawString(font, tanks[j].Coins.ToString(), new Vector2(865, i), Color.White);
                    i = i + 20;
                }
            }
        }

    } 
}
