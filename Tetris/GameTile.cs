
using System.Diagnostics;

namespace Tetris;

class GameTile
{
    public int Type {  get; set; }
    public TileState State { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Direction { get; set; }
    public (int[][] bits, int x)? Patterns { get; private init; }


    public GameTile(in (int[][] bits, int x) patterns)
    {
        Patterns = patterns!;
        Debug.WriteLine($"Create GameTile {Type}");
    }
}
