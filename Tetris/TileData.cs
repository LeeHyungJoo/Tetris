using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;
class TileData
{
    public int Type { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int[,]? Coords { get; set; }
    public int[,]? Pivot { get; set; }
    public string? ImgPath { get; set; }
}
