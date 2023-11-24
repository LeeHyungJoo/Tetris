using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;

class Board
{
    public int Height { get; private set; }
    public int Width { get; private set; }
    public Queue<int>? TileQ { get; private set; }
    public GameTile? CurrentTile { get; private set; }

    private readonly ReadOnlyCollection<int> _ruleCheck;
    private readonly int[] _placed;


    public Board(int height, int width)
    {
        Height = height;
        Width = width;

        int[] makeRuleCheckArray = new int[height];
        Array.Fill(makeRuleCheckArray, (int)Math.Pow(2, width) - 1);
        _ruleCheck = new ReadOnlyCollection<int>(makeRuleCheckArray);

        _placed = new int[height];
    }

    public void Update()
    {
    }



}
