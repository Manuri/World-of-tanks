using System;

namespace MyTest2
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
           /* AI.Pathfinder pf = new AI.Pathfinder();
            AI.CompleteSquare[,] cs = new AI.CompleteSquare[20,20];

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    cs[i,j] = new AI.CompleteSquare();
                }
            }

                cs[0, 0].ContentCode = AI.SquareContent.me;
            cs[5, 0].ContentCode = AI.SquareContent.Coinpile;
            cs[2,0].ContentCode = AI.SquareContent.Brick;

            pf.Squares = cs;

            pf.Pathfind();
            pf.HighlightPath(AI.SquareContent.Coinpile);*/

        }
    }
#endif
}

