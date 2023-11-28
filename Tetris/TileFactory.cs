using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
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

    private Dictionary<int, (int[] bits, int x)[]> _patternInfos = new Dictionary<int, (int[] bits, int x)[]>();
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
            int[][][] target = new int[4][][];
            target[0] = td.Value.Coords!;

            for (int d = 1; d < 4; d++)
            {
                int sizeR = target[d - 1].Length;
                int sizeC = target[d - 1][0].Length;

                target[d] = new int[sizeC][];
                for (int idxC = 0; idxC < sizeC; idxC++)
                {
                    target[d][idxC] = new int[sizeR];
                    for (int idxR = 0; idxR < sizeR; idxR++)
                    {
                        target[d][idxC][idxR] = target[d - 1][idxR][sizeC - idxC - 1];
                    }
                }
            }

           _patternInfos.Add(td.Key, new (int[], int)[4]);
            var pattern = _patternInfos[td.Key];

            for (int d = 0; d < 4; d++)
            {
                int sizeR = target[d].Length;
                int sizeC = target[d][0].Length;

                pattern[d].bits = new int[sizeR];
                for (int idxR = 0; idxR < sizeR; idxR++)
                {
                    int value = 0;

                    for (int idxC = 0; idxC < sizeC; idxC++)
                    {
                        if (target[d][idxR][idxC] == 0)
                        {
                            continue;
                        }
                        value |= (1 << idxC);
                    }

                    pattern[d].bits[idxR] = value;
                }
                pattern[d].x = sizeC;
            }
        }
    }

    public GameTile CreateGameTile()
    {
        int randType = _random.Next(0, _patternInfos.Count) + 1;
        return new GameTile(randType, _patternInfos[randType]);
    }
}
