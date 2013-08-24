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
        //LinkedList<Point> path = new LinkedList<Point>();
        LinkedList<Point> path;
        // Movements is an array of various directions.        
        Point[] _movements;

        // Squares is an array of square objects.
        CompleteSquare[,] _squares;

        static Point[,] points;

        int cost = 0;

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

        

        public void Pathfind(Point startingPoint)
        {
            /*
             * 
             * Find path from hero to monster. First, get coordinates
             * of hero.
             * 
             * */
            //Point startingPoint = FindCode(SquareContent.me);

            Squares = Map.getMap.BoardBlocks;
            //_squares = Map.getMap.BoardBlocks;
           // Squares[1, 1].DistanceSteps = 24;
           // _squares[1, 1].DistanceSteps = 24;

            //Console.WriteLine("checking inside pathfind, _squares[1,1].distanceSteps="+_squares[1,1].DistanceSteps);
            //Console.WriteLine("checking inside pathfind, inMap[1,1].distanceSteps="+Map.getMap.BoardBlocks[1,1].DistanceSteps);

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

                        foreach (Point movePoint in ValidMoves(x, y))
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
                        }
                    }
                }
                if (!madeProgress)
                {
                    break;
                }
            }
        }

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
                default:
                    return false;
            }
        }

        private Point FindCode(SquareContent contentIn)
        {
            /*
             * 
             * Find the requested code and return the point.
             * 
             * */
            foreach (Point point in AllSquares())
            {
                if (_squares[point.X, point.Y].ContentCode == contentIn)
                {
                    return new Point(point.X, point.Y);
                }
            }
            return new Point(-1, -1);
        }

       // public void HighlightPath(Point startingPoint)
        public Path HighlightPath(Point startingPoint)
        {
            /*
             * 
             * Mark the path from monster to hero.
             * 
             * */
           // Point startingPoint = FindCode(goal);
            int pointX = startingPoint.X;
            int pointY = startingPoint.Y;
            path = new LinkedList<Point>();
            cost = 0;

            // Mytesting
           // Console.WriteLine("monster point: " + pointX + " " + pointY);
           // int step = 0;
           // Point[] thePath = new Point[50];
            //my testing over

            if (pointX == -1 && pointY == -1)
            {
                Console.WriteLine("inside if");
                return null;
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
              /*  foreach (Point movePoint in ValidMoves(pointX, pointY))
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
                }*/

                for (int i = 0; i < 4; i++)
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
                }
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

                        //my testing
                        //Console.WriteLine("step" + step + " " + lowestPoint.X + ", " + lowestPoint.Y);
                        //thePath[step] = new Point(lowestPoint.X, lowestPoint.Y);
                        //path.AddFirst(new Point(lowestPoint.X, lowestPoint.Y)); //removed to solve outofmemoryexception
                        try
                        {
                            path.AddFirst(points[lowestPoint.X, lowestPoint.Y]);
                        }
                        catch (OutOfMemoryException oome)
                        {
                            //Console.WriteLine(oome.Message);
                            Console.WriteLine("oome Exception at HighlightPath");
                        }

                        cost += 1;
                        //Console.WriteLine("cost adding.. "+cost);
                        //step++;
                        //my testing over

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
                    //break;
                    //Console.WriteLine("path marked");
                    path.AddLast(points[startingPoint.X,startingPoint.Y]);
                    return new Path(path,cost);
                }
            }
            return null;
        }

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

        public Path getShortestpath(Point me, Point goal)
        {
            Pathfind(me);
            return HighlightPath(goal);
        }

        public List<Path> sortAcoordingToCosts(ref List<Path> pathList)
        {
            pathList.Sort();
            return pathList;
        }   
    }
    }

