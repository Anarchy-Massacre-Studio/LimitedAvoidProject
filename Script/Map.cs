using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SyzygyStudio;

using MapLevel = SyzygyStudio.MapInfo.MapLevel;

/// <summary>
/// 新建地图，实例化这个类。
/// </summary>
public class Map
{
    /// <summary>
    /// 地图ID。唯一的，不可更改。
    /// </summary>
    public readonly int id;
    /// <summary>
    /// 地图已经解锁的难度。
    /// </summary>
    public MapLevel mapLevel;
    /// <summary>
    /// 地图的最好成绩。
    /// </summary>
    public int bestScore;
    /// <summary>
    /// 地图的缩略图显示。
    /// </summary>
    public Texture2D mipmap;
    /// <summary>
    /// 地图是否解锁。
    /// </summary>
    public bool isLock;

    public Map(int id, MapLevel mapLevel, int bestScore, Texture2D mipmap, bool isLock)
    {
        this.id = id;
        this.mapLevel = mapLevel;
        this.bestScore = bestScore;
        this.mipmap = mipmap;
        this.isLock = isLock;
    }

    /// <summary>
    /// 获取地图，如果不存在，则返回null。
    /// </summary>
    /// <returns></returns>
    public int[,] GetMap()
    {
        if (MapData.MapID2Data.ContainsKey(id)) return MapData.MapID2Data[id];
        else return null;
    }
}

public static class MapData
{
    /// <summary>
    /// 通过地图id，得到地图数据。
    /// </summary>
    public static Dictionary<int, int[,]> MapID2Data;

    public static List<Map> Maps;

    static List<int[,]> maps;

    static string url = @"Assets/Resources/Map.map";
    /*
    private static int[,] level_1 =
    {
        {0,0,2,2,2,0,0 },
        {0,0,0,0,0,0,0 },
        {2,0,1,1,1,0,2 },
        {2,0,1,1,1,0,2 },
        {2,0,1,1,1,0,2 },
        {0,0,0,0,0,0,0 },
        {0,0,2,2,2,0,0 }
    };

    private static int[,] level_2 =
    {
        {0,2,0,0,2,0,0,2,0 },
        {0,0,0,0,0,0,0,0,0 },
        {0,0,0,1,1,1,0,0,0 },
        {2,0,1,0,1,0,1,0,2 },
        {0,0,0,1,1,1,0,0,0 },
        {0,0,0,0,0,0,0,0,0 },
        {0,2,0,0,2,0,0,2,0 }
    };
    */

    static MapReader mapReader;

    static MapData()
    {
        mapReader = new MapReader(url);

        MapID2Data = new Dictionary<int, int[,]>()
        {
            {1, mapReader.GetMap(1) },
            {2, mapReader.GetMap(2) },
            {3, mapReader.GetMap(3) },
            {4, mapReader.GetMap(4) }
        };

        Maps = new List<Map>()
        {
            new Map(1,MapLevel.Easy,1,null,false),
            new Map(2,MapLevel.Hell,1,null,true),
            new Map(3,MapLevel.Easy,1,null,true),
            new Map(4,MapLevel.Easy,1,null,true)
        };
    }
}
