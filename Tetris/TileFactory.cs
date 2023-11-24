using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tetris;

class TileFactory
{
    public static TileFactory Instance => _instance.Value;

    private static readonly Lazy<TileFactory> _instance = new Lazy<TileFactory>(() => new TileFactory());

    private Dictionary<int, int[][][]> _patternInfos = new Dictionary<int, int[][][]>();
    private Random _random = new Random();

    public void MakeData()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\");

        var tileData = new Dictionary<int, TileData>();
        using (var reader = new StreamReader(path + "Tile.Json"))
        {
            tileData = JsonSerializer.Deserialize<List<TileData>>(reader.ReadToEnd())?.ToDictionary(t => t.Type) ?? tileData;
        }

        foreach (var td in tileData)
        {
            SetPatternsBy4Directions(td.Key, td.Value.Coords!);
        }
    }

    private void SetPatternsBy4Directions(int type, int[][] coord)
    {
        _patternInfos!.Add(type, new int[4][][]);
        var target = _patternInfos[type];
        target[0] = coord;

        for (int d = 1; d < 4; d++)
        {
            int rowSize = target[d - 1].Length;
            int colSize = target[d - 1][0].Length;

            target[d] = new int[colSize][];
            for(int idxC = 0; idxC < colSize; idxC++)
            {
                target[d][idxC] = new int[rowSize];
                for (int idxR = 0; idxR < rowSize; idxR++)
                {
                    target[d][idxC][idxR] = target[d - 1][idxR][idxC];
                }
            }
        }
    }


    public GameTile CreateGameTile()
    {
        int ranIdx = _random.Next(0, _patternInfos.Count) + 1;
        return new GameTile(_patternInfos[ranIdx]) { Type = ranIdx};
    }

}
