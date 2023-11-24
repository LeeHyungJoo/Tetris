using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;
class TileData
{
    public int Type { get; set; }
    public int[][]? Coords { get; set; }
    public int PivotX { get; set; }
    public int PivotY { get; set; }
    public string? ImgPath { get; set; }
}
