using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Tetris;

class Board
{
    public BoardState boardState { get; private set; }
    public int Height { get; private init; }
    public int Width { get; private init; }
    public Queue<int>? TileQ { get; private set; }
    public GameTile? CurrentTile { get; private set; }

    public readonly int[] Placed;

    private readonly ReadOnlyCollection<int> _ruleCheck;

    private System.Timers.Timer _proceedTimer;
    private int startTileY;
    private int startTileX;

    public Board(int height, int width)
    {
        Height = height;
        Width = width;

        int[] ruleCheckArray = new int[height];
        Array.Fill(ruleCheckArray, (int)Math.Pow(2, width) - 1);
        _ruleCheck = new ReadOnlyCollection<int>(ruleCheckArray);

        Placed = new int[height];

        _proceedTimer = new System.Timers.Timer();
        _proceedTimer.Interval = 1000;
        _proceedTimer.AutoReset = true;
        _proceedTimer.Enabled = false;
        _proceedTimer.Elapsed += Update;

        startTileY = 0;
        startTileX = Width / 2 -1;

        Placed[height - 1] = 1024 - 2;
    }

    public void Start()
    {
        boardState = BoardState.Active;

        CurrentTile = TileFactory.Instance.CreateGameTile();
        CurrentTile.State = TileState.Active;
        CurrentTile.Y = startTileY;
        CurrentTile.X = startTileX;

        _proceedTimer.Start();
    }

    private void Update(object? sender, ElapsedEventArgs e)
    {
        Fall();
        ClearCheck();

        if (CurrentTile?.State == TileState.Placed)
        {
            CurrentTile = TileFactory.Instance.CreateGameTile();
            CurrentTile.State = TileState.Active;
            CurrentTile.Y = startTileY;
            CurrentTile.X = startTileX;
        }

        if (Placed[0] != 0)
        {
            boardState = BoardState.Finished;
            _proceedTimer.Stop();
        }
    }

    public void Turn()
    {
        CurrentTile!.Direction = (CurrentTile.Direction + 1) % 4;
    }

    public void MoveLeft()
    {
        if(PlacedCheckSides(-1))
        {
            return;
        }

        --CurrentTile!.X;
    }

    public void MoveRight()
    {
        if (PlacedCheckSides(+1))
        {
            return;
        }

        ++CurrentTile!.X;
    }

    public void Fall()
    {
        if(CurrentTile == null || CurrentTile.State != TileState.Active)
        {
            return;
        }

        if(PlacedCheckDown())
        {
            PlaceTile();
            return;
        }


        ++CurrentTile.Y;
    }

    private void ClearCheck()
    {
        List<int> clearIndices = Enumerable.Repeat(0, Height).ToList();

        for (int y = Height - 1; y >= 0; y--)
        {
            if (Placed[y] == _ruleCheck[y])
            {
                for (int sumY = y; sumY >= 0; sumY--)
                {
                    clearIndices[sumY]++;
                }
            }
        }

        for (int y = Height - 1; y >= 0; y--)
        {
            if (y - clearIndices[y] < 0)
            {
                Placed[y] = 0;
            }
            else
            {
                Placed[y] = Placed[y - clearIndices[y]];
            }
        }
    }

    private void PlaceTile()
    {
        Debug.WriteLine("PlaceTile");
        CurrentTile!.State = TileState.Placed;
        for(int y = 0; y < CurrentTile.Pattern.bits.Length; y++)
        {
            int patternMask = CurrentTile.Pattern.bits[y];
            for (int x = 0; x < CurrentTile.Pattern.x; x++)
            {
                if ((patternMask & (1 << x)) != 0)
                {
                    Placed[y + CurrentTile.Y] |= (1 << (x + CurrentTile.X));
                }
            }
        }
    }

    private bool PlacedCheckDown()
    {
        if (CurrentTile?.Y + CurrentTile?.Pattern.bits.Length + 1 > Height)
        {
            return true;
        }

        int tileHeight = CurrentTile?.Pattern.bits.Length ?? 0;
        int tileWidth = CurrentTile?.Pattern.x ?? 0;
        for (int y = tileHeight - 1; y >= 0; y--)
        {
            int patternMask = CurrentTile!.Pattern.bits[y] << CurrentTile.X;
            int placedMask = Placed[y + CurrentTile.Y + 1];

            for (int x = 0; x < tileWidth; x++)
            {
                if (((patternMask & placedMask) & (1 << (x + CurrentTile.X))) != 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool PlacedCheckSides(int dirX)
    {
        int coordX = (CurrentTile?.X + CurrentTile?.Pattern.x + dirX ?? 0 ) -1;
        if ( coordX <= 0 || coordX > Width - 1)
        {
            return true;
        }

        int tileHeight = CurrentTile?.Pattern.bits.Length ?? 0;
        int tileWidth = CurrentTile?.Pattern.x ?? 0;
        for (int y = tileHeight - 1; y >= 0; y--)
        {
            int patternMask = CurrentTile!.Pattern.bits[y] << CurrentTile.X + dirX;
            int placedMask = Placed[y + CurrentTile.Y];

            for (int x = 0; x < tileWidth; x++)
            {
                if (((patternMask & placedMask) & (1 << (x + CurrentTile.X + dirX))) != 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
