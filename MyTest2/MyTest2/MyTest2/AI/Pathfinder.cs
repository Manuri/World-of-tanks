using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;
using Microsoft.Xna.Framework;

namespace MyTest2.AI
{
    class Pathfinder
    {
        private static Pathfinder instance;

        // Movements is an array of various directions.        
        Point[] _movements;

        // Squares is an array of square objects.
        CompleteSquare[,] _squares;

        static Point[,] points;


        public static Pathfinder getPathFinder
        {
            get
            {
                if (instance == null)
                {
                    instance = new Pathfinder();
                }
                return instance;
            }
        }

        public CompleteSquare[,] Squares
        {
            get { return _squares; }
            set { _squares = value; }
        }

        public Pathfinder()
        {
            InitMovements(4);
          //  ClearSquares();
            points = new Point[Map.getMap.GridLength, Map.getMap.GridLength];

            for (int i = 0; i < Map.getMap.GridLength; i++)
            {
                for (int j = 0; j < Map.getMap.GridLength; j++)
                {
                    points[i, j] = new Point(i,j); 
                }
            }
        }

        public void InitMovements(int movementCount)
        {
            /*
             * 
             * Just do some initializations.
             * 
             * */
            if (movementCount == 4)
            {
                _movements = new Point[]
                {
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(0, 1),
                    new Point(-1, 0)
                };
            }
            else
            {
                _movements = new Point[]
                {
                    new Point(-1, -1),
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(1, 0),
                    new Point(1, 1),
                    new Point(0, 1),
                    new Point(-1, 1),
                    new Point(-1, 0)
                };
            }
        }
        
       /* public void ClearSquares()
        {
             //Reset every square.
             
            foreach (Point point in AllSquares())
            {
                _squares[point.X, point.Y] = new CompleteSquare();
            }
        } */

        public void ClearLogic()
        {
            /*
             * 
             * Reset some information about the squares.
             * 
             * */
            foreach (Point point in AllSquares())
            {
                int x = point.X;
                int y = point.Y;
                _squares[x, y].DistanceSteps = 10000;
                _squares[x, y].IsPath = false;
            }
        }

        #region pathfinding

        public void Pathfind()
        {
            /*
             * 
             * Find path from hero to monster. First, get coordinates
             * of hero.
             * 
             * */
            //Point startingPoint = FindCode(SquareContent.me);
           // while (true)
            //{
                Point startingPoint = Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate;

                Squares = Map.getMap.BoardBlocks;

                int heroX = startingPoint.X;
                int heroY = startingPoint.Y;
                if (heroX == -1 || heroY == -1)
                {
                    return;
                }
                /*
                 * 
                 * Hero starts at distance of 0.
                 * 
                 * */
                // Console.WriteLine("before: inside pathfind(), Map.getMap.BoardBlocks[heroX,heroY].DistanceSteps: " + Map.getMap.BoardBlocks[heroX, heroY].DistanceSteps);
                _squares[heroX, heroY].DistanceSteps = 0;
                //  Console.WriteLine("after: inside pathfind(), Map.getMap.BoardBlocks[heroX,heroY].DistanceSteps: " + Map.getMap.BoardBlocks[heroX, heroY].DistanceSteps);

                while (true)
                {
                    bool madeProgress = false;

                    /*
                     * 
                     * Look at each square on the board.
                     * 
                     * */
                    foreach (Point mainPoint in AllSquares())
                    {
                        int x = mainPoint.X;
                        int y = mainPoint.Y;

                        //Console.WriteLine("inside pathfind(), mainPoint: "+mainPoint.X+", "+mainPoint.Y);

                        /*
                         * 
                         * If the square is open, look through valid moves given
                         * the coordinates of that square.
                         * 
                         * */
                        if (SquareOpen(x, y))
                        {
                            int passHere = _squares[x, y].DistanceSteps;
                            // Console.WriteLine("inside pathfind(), pass here: "+passHere);
                            
                            //mytesting
                            /*foreach (Point movePoint in ValidMoves(x, y))
                            {
                                int newX = movePoint.X;
                                int newY = movePoint.Y;
                                int newPass = passHere + 1;
                                //Console.WriteLine("inside pathfind(), movepoint: "+movePoint.X+"' "+movePoint.Y);
                                //Console.WriteLine("inside pathfind(), newpass: " + newPass);
                                //Console.WriteLine("inside pathfind(), _squares[newX, newY].DistanceSteps: " + _squares[newX, newY].DistanceSteps);

                                if (_squares[newX, newY].DistanceSteps > newPass)
                                {
                                    _squares[newX, newY].DistanceSteps = newPass;
                                    madeProgress = true;
                                    // Console.WriteLine("madeProgress");
                                }
                            }*/

                            for (int i = 0; i < 4; i++)
                            {
                                int newX = points[x, y].X + _movements[i].X;
                                int newY = points[x, y].Y + _movements[i].Y;

                                if (ValidCoordinates(newX, newY) && SquareOpen(newX, newY))
                                {
                                    int newX1 = points[newX,newY].X;
                                    int newY1 = points[newX, newY].Y;
                                    int newPass = passHere + 1;

                                    if (_squares[newX1, newY1].DistanceSteps > newPass)
                                    {
                                        _squares[newX1, newY1].DistanceSteps = newPass;
                                        madeProgress = true;
                                        // Console.WriteLine("madeProgress");
                                    }
                                }
                                
                            }
                            //my testing over
                        }
                    }
                    if (!madeProgress)
                    {
                        break;
                    }
                }
           // }
        }

        #endregion pathfinding

        static private bool ValidCoordinates(int x, int y)
        {
            /*
             * 
             * Our coordinates are constrained between 0 and 14.
             * 
             * */
            if (x < 0)
            {
                return false;
            }
            if (y < 0)
            {
                return false;
            }
            if (x > 9)
            {
                return false;
            }
            if (y > 9)
            {
                return false;
            }
            return true;
        }

        private bool SquareOpen(int x, int y)
        {
            /*
             * 
             * A square is open if it is not a wall.
             * 
             * */
            switch (_squares[x, y].ContentCode)
            {
                case SquareContent.Empty:
                    return true;
                case SquareContent.Coinpile:
                    return true;
                case SquareContent.Healthpack:
                    return true;
                case SquareContent.Foe:
                    return true;
                case SquareContent.me:
                    return true;
                case SquareContent.Water:
                    return false;
                case SquareContent.Stone:
                    return false;
                case SquareContent.Brick:
                    return false;
                default:
                    return false;
            }
        }

       /* private Point FindCode(SquareContent contentIn)
        {
            // Find the requested code and return the point.
             
            foreach (Point point in AllSquares())
            {
                if (_squares[point.X, point.Y].ContentCode == contentIn)
                {
                    return new Point(point.X, point.Y);
                }
            }
            return new Point(-1, -1);
        }*/

        #region higlighting path

        // public void HighlightPath(Point startingPoint)
        public void HighlightPath(ref ClosestTreasure treasure)
        {
           // Console.WriteLine("Highlighting path to treasure point: "+treasure.Coordinate.X+", "+treasure.Coordinate.Y);
            /*
             * 
             * Mark the path from monster to hero.
             * 
             * */
           // Point startingPoint = FindCode(goal);
            /*int pointX = startingPoint.X;
            int pointY = startingPoint.Y;*/

            int pointX = treasure.Coordinate.X;
            int pointY = treasure.Coordinate.Y;

            // Mytesting
           // Console.WriteLine("monster point: " + pointX + " " + pointY);
           // int step = 0;
           // Point[] thePath = new Point[50];
            //my testing over

            if (pointX == -1 && pointY == -1)
            {
                Console.WriteLine("inside if");
            }

            while (true)
            {
                /*
                 * 
                 * Look through each direction and find the square
                 * with the lowest number of steps marked.
                 * 
                 * */
                //Point lowestPoint = Point.Empty;
                Point lowestPoint = new Point(0, 0);
                int lowest = 10000;

                //my test
                foreach (Point movePoint in ValidMoves(pointX, pointY))
                {
                    int count = _squares[movePoint.X, movePoint.Y].DistanceSteps;
                    //Console.WriteLine("count before if "+count);
                    if (count < lowest)
                    {
                        lowest = count;
                        //Console.WriteLine("count: "+count);
                        lowestPoint.X = movePoint.X;
                        lowestPoint.Y = movePoint.Y;
                    }
                }

              /*  for (int i = 0; i < 4; i++)
                {
                    int newX = points[pointX,pointY].X + _movements[i].X;
                    int newY = points[pointX, pointY].Y + _movements[i].Y;

                    if (ValidCoordinates(newX, newY) && SquareOpen(newX, newY))
                    {
                        int count = _squares[points[newX,newY].X, points[newX,newY].Y].DistanceSteps;
                        //Console.WriteLine("count before if "+count);
                        if (count < lowest)
                        {
                            lowest = count;
                            //Console.WriteLine("count: "+count);
                            lowestPoint.X = points[newX, newY].X;
                            lowestPoint.Y = points[newX, newY].Y;
                        }
                    }
                }*/
                //my test over
                    if (lowest != 10000)
                    {
                        /*
                         * 
                         * Mark the square as part of the path if it is the lowest
                         * number. Set the current position as the square with
                         * that number of steps.
                         * 
                         * */

                        _squares[lowestPoint.X, lowestPoint.Y].IsPath = true;

                        try
                        {
                            //path.AddFirst(points[lowestPoint.X, lowestPoint.Y]);
                           // Console.WriteLine("checking whether treasure is null "+treasure.Cost);

                            if (treasure == null) Console.WriteLine("treasure is null");
                            if (treasure.Path == null) Console.WriteLine("treasure.Path is null");

                            try
                            {
                                treasure.Path.AddFirst(points[lowestPoint.X, lowestPoint.Y]);                               

                            }
                            catch (NullReferenceException nre)
                            {
                                Console.WriteLine(nre.Message);
                                treasure.Path.Clear();//to avoid hitting obstacles. I guess it happened because I didnt clear the half added path.
                                break;//to solve null reference problem. dont know whether this will work;
                            }
                           // Console.WriteLine("path highlighting "+treasure.Path.First.Value.X+" "+treasure.Path.First.Value.Y);
                        }
                        catch (OutOfMemoryException oome)
                        {
                            //Console.WriteLine(oome.Message);
                            Console.WriteLine("oome Exception at HighlightPath");
                        }

                        pointX = lowestPoint.X;
                        pointY = lowestPoint.Y;
                    }
                    else
                    {
                        Console.WriteLine("else");
                        break;
                    }

               // if (_squares[pointX, pointY].ContentCode == SquareContent.me)
                if (pointX == Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate.X && pointY == Map.getMap.AllTanks[Map.getMap.MyIndex].Coordinate.Y)
                {
                    /*
                     * 
                     * We went from monster to hero, so we're finished.
                     * 
                     * */                  
                    
                    //Console.WriteLine("path marked");
                    treasure.Path.AddLast(points[treasure.Coordinate.X,treasure.Coordinate.Y]);
                    //Console.WriteLine("path highlighting " + treasure.Path.Last.Value.X + " " + treasure.Path.Last.Value.Y);
                    break;
                }
            }
        }

        #endregion higlighting path

        private static IEnumerable<Point> AllSquares()
        {
            /*
             * 
             * Return every point on the board in order.
             * 
             * */
            for (int x = 0; x < Map.getMap.GridLength; x++)
            {
                for (int y = 0; y < Map.getMap.GridLength; y++)
                {
                    //yield return new Point(x, y);
                    yield return points[x, y];
                }
            }
        }

        private IEnumerable<Point> ValidMoves(int x, int y)
        {
            /*
             * 
             * Return each valid square we can move to.
             * 
             * */
            foreach (Point movePoint in _movements)
            {
                int newX = x + movePoint.X;
                int newY = y + movePoint.Y;

                if (ValidCoordinates(newX, newY) &&
                    SquareOpen(newX, newY))
                {
                   // yield return new Point(newX, newY);
                    yield return points[newX, newY];
                }
            }
     
        }


      
    }
    }

