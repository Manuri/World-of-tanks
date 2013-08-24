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
        /*
         * 
         * Movements is an array of various directions.
         * 
         * */
        Point[] _movements;

        /*
         * 
         * Squares is an array of square objects.
         * 
         * */
        CompleteSquare[,] _squares = new CompleteSquare[20, 20];
        public CompleteSquare[,] Squares
        {
            get { return _squares; }
            set { _squares = value; }
        }

        public Pathfinder()
        {
            InitMovements(4);
            ClearSquares();
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

        public void ClearSquares()
        {
            /*
             * 
             * Reset every square.
             * 
             * */
            foreach (Point point in AllSquares())
            {
                _squares[point.X, point.Y] = new CompleteSquare();
            }
        }

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
            _squares[heroX, heroY].DistanceSteps = 0;

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

                    /*
                     * 
                     * If the square is open, look through valid moves given
                     * the coordinates of that square.
                     * 
                     * */
                    if (SquareOpen(x, y))
                    {
                        int passHere = _squares[x, y].DistanceSteps;

                        foreach (Point movePoint in ValidMoves(x, y))
                        {
                            int newX = movePoint.X;
                            int newY = movePoint.Y;
                            int newPass = passHere + 1;

                            if (_squares[newX, newY].DistanceSteps > newPass)
                            {
                                _squares[newX, newY].DistanceSteps = newPass;
                                madeProgress = true;
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
            if (x > 19)
            {
                return false;
            }
            if (y > 19)
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

        public void HighlightPath(Point startingPoint)
        {
            /*
             * 
             * Mark the path from monster to hero.
             * 
             * */
           // Point startingPoint = FindCode(goal);
            int pointX = startingPoint.X;
            int pointY = startingPoint.Y;

            // Mytesting
            Console.WriteLine("monster point: " + pointX + " " + pointY);
            int step = 0;
            Point[] thePath = new Point[50];
            //my testing over

            if (pointX == -1 && pointY == -1)
            {
                Console.WriteLine("inside if");
                return;
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

                foreach (Point movePoint in ValidMoves(pointX, pointY))
                {
                    int count = _squares[movePoint.X, movePoint.Y].DistanceSteps;
                    if (count < lowest)
                    {
                        lowest = count;
                        lowestPoint.X = movePoint.X;
                        lowestPoint.Y = movePoint.Y;
                    }
                }
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
                    Console.WriteLine("step" + step + " " + lowestPoint.X + ", " + lowestPoint.Y);
                    thePath[step] = new Point(lowestPoint.X, lowestPoint.Y);
                    step++;
                    //my testing over

                    pointX = lowestPoint.X;
                    pointY = lowestPoint.Y;
                }
                else
                {
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
                    break;
                }
            }
        }

        private static IEnumerable<Point> AllSquares()
        {
            /*
             * 
             * Return every point on the board in order.
             * 
             * */
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    yield return new Point(x, y);
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
                    yield return new Point(newX, newY);
                }
            }
        }
    }
    }

