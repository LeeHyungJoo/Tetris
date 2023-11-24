
namespace Tetris;

class GameTile
{
    public int Type {  get; set; }
    public TileState State { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Direction { get; set; }
    public int[][][]? Patterns { get; private init; }

    public GameTile(in int[][][] patterns)
    {
        Patterns = patterns;
        Console.WriteLine($"Create GameTile {Type}");
    }

    public void Turn()
    {
        if(State != TileState.Proceeding)
        {
            Console.WriteLine("Invalid State");
            return;
        }

        Console.WriteLine("Turn Tile");
    }

    public void Proceed()
    {
        if (State != TileState.Proceeding)
        {
            Console.WriteLine("Invalid State");
            return;
        }

        Console.WriteLine("Proceed Tile");
    }
}
