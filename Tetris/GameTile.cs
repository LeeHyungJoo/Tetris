
using System.Diagnostics;

namespace Tetris;

class GameTile
{
    public int Type {  get; set; }
    public TileState State { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Direction { get; set; }
    public int[][]? Patterns { get; private init; }

    public GameTile(in int[][] patterns)
    {
        Patterns = patterns!;
        Debug.WriteLine($"Create GameTile {Type}");
    }

    public void Turn()
    {
        if(State != TileState.Active)
        {
            Debug.WriteLine("Invalid State");
            return;
        }

        Debug.WriteLine("Turn Tile");
    }

    public void Proceed()
    {
        if (State != TileState.Active)
        {
            Debug.WriteLine("Invalid State");
            return;
        }

        Y++;
        Debug.WriteLine("Proceed Tile");
    }
}
