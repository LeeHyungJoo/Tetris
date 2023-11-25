using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Tetris;

class Board
{
    public int Height { get; private set; }
    public int Width { get; private set; }
    public Queue<int>? TileQ { get; private set; }
    public GameTile? CurrentTile { get; private set; }

    public readonly int[] Placed;

    private readonly ReadOnlyCollection<int> _ruleCheck;

    private System.Timers.Timer _timer;

    public Board(int height, int width)
    {
        Height = height;
        Width = width;

        int[] ruleCheckArray = new int[height];
        Array.Fill(ruleCheckArray, (int)Math.Pow(2, width) - 1);
        _ruleCheck = new ReadOnlyCollection<int>(ruleCheckArray);

        Placed = new int[height];

        _timer = new System.Timers.Timer();
        _timer.Interval = 1000;
        _timer.AutoReset = true;
        _timer.Enabled = false;
        _timer.Elapsed += Update;
    }

    public void Start(int tileY, int tileX)
    {
        CurrentTile = TileFactory.Instance.CreateGameTile();
        CurrentTile.State = TileState.Active;
        CurrentTile.Y = tileY;
        CurrentTile.X = tileX;
        _timer.Start();
    }

    private void Update(object? sender, ElapsedEventArgs e)
    {
        if (CurrentTile != null)
        {
            CurrentTile.Proceed();
        }
    }
}
