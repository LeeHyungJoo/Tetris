using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;

class MasterData
{
    public static MasterData Instance => _instance.Value;

    private static readonly Lazy<MasterData> _instance = new Lazy<MasterData>(()=> new MasterData());

    private TileData? _tileData { get; set; }


    public void LoadData()
    {

    }
}
