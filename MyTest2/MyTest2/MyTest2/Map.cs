using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyTest2.Beans;
using MyTest2.AI;

namespace MyTest2
{
    class Map
    {
        private static Map instance;

        //private MyPlayer _myTank;
        private Player[] _allTanks;
        private List<CoinPile> _coinList;
        private List<Treasure> _lifePackList;
        private CompleteSquare[,] _boardBlocks;
        private int _myIndex;
        private int _noOfPlayers;
        private int _gridLength;

        public Map()
        {
            _allTanks = new Player[5];
            _coinList = new List<CoinPile>();
            _lifePackList = new List<Treasure>();
            _gridLength = 10;
            _boardBlocks = new CompleteSquare[GridLength,GridLength];
            _noOfPlayers = 0;
        }

        public static Map getMap
        {
            get
            {
                if (instance == null)
                {
                    instance = new Map();
                }
                return instance;
            }
        }

       /* public MyPlayer MyTank
        {
            get { return _myTank; }
            set { _myTank = value; }
        }*/

        public int GridLength
        {
            get { return _gridLength; }
            set { _gridLength = value; }
        }

        public int NoOfPlayers
        {
            get { return _noOfPlayers; }
            set { _noOfPlayers = value; }
        }

        public int MyIndex
        {
            get { return _myIndex; }
            set { _myIndex = value; }
        }

        public Player[] AllTanks
        {
            get { return _allTanks; }
            set { _allTanks = value; }
        }

        public CompleteSquare[,] BoardBlocks
        {
            get { return _boardBlocks; }
            set { _boardBlocks = value; }
        }

        public List<CoinPile> CoinList
        {
            get { return _coinList; }
        }

        public List<Treasure> LifePackList
        {
            get { return _lifePackList; }
        }

        public IEnumerable<CompleteSquare> allBlocks()
        {
            for (int x = 0; x < GridLength; x++)
            {
                for (int y = 0; y < GridLength; y++)
                {
                    yield return _boardBlocks[x,y];
                }
            }
        }
        public IEnumerable<CompleteSquare> allBricks()
        {
            for (int x = 0; x < GridLength; x++)
            {
                for (int y = 0; y < GridLength; y++)
                {
                    if(_boardBlocks[x,y].ContentCode==SquareContent.Brick)
                        yield return _boardBlocks[x, y];
                }
            }
        }
    }
}
