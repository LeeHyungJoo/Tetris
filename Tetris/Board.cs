using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    private System.Timers.Timer _tileTimer;
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

        _tileTimer = new System.Timers.Timer();
        _tileTimer.Interval = 1000;
        _tileTimer.AutoReset = true;
        _tileTimer.Enabled = false;
        _tileTimer.Elapsed += Update;

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

        _tileTimer.Start();
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
            _tileTimer.Stop();
        }
    }

    public void Turn()
    {
        if(PlacedCheckTurn((CurrentTile!.Direction + 1) % 4))
        {
            return;
        }

        CurrentTile.Direction = (CurrentTile.Direction + 1) % 4;
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

    public bool Fall()
    {
        if(CurrentTile == null || CurrentTile.State != TileState.Active)
        {
            return true;
        }

        if(PlacedCheckDown())
        {
            PlaceTile();
            return true;
        }


        ++CurrentTile.Y;
        return false;
    }

    public void HardFall()
    {
        while(!Fall()){}
        _tileTimer.Start();
    }



    private void ClearCheck()
    {
        bool isClearedLine = false;
        for (int y = Height - 1; y >= 0; y--)
        {
            if (Placed[y] == _ruleCheck[y])
            {
                isClearedLine = true;
                Placed[y] = 0;
            }
        }

        if(isClearedLine)
        {
            Array.Sort(Placed, (lhs, rhs) =>
            {
                if (lhs == 0 && rhs != 0)
                    return -1;
                if (lhs != 0 && rhs == 0)
                    return 1;
                return 0;
            });
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
        int coordX = (CurrentTile?.X + CurrentTile?.Pattern.x + dirX ?? 0 );
        if ( coordX < 0 || coordX > Width || CurrentTile?.X + dirX < 0 || CurrentTile?.X + dirX > Width)
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

    private bool PlacedCheckTurn(int dir)
    {
        var pattern = CurrentTile?.Patterns![dir]!;
        
        int coordX = (CurrentTile?.X + pattern.Value.x ?? 0);
        if (coordX < 0 || coordX > Width || CurrentTile?.X < 0 || CurrentTile?.X > Width)
        {
            return true;
        }


        int tileHeight = pattern.Value.bits.Length;
        int tileWidth = pattern.Value.x;
        for (int y = tileHeight - 1; y >= 0; y--)
        {
            int patternMask = pattern.Value.bits[y] << CurrentTile!.X;
            int placedMask = Placed[y + CurrentTile.Y];

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
}
