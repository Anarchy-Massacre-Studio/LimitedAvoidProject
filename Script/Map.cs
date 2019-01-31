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

    /// <summary>
    /// 敌人攻击方向。
    /// </summary>
    public List<List<Vector2>> direction;

    /// <summary>
    /// 敌人出生点组。
    /// </summary>
    public List<List<Vector2>> group;

    public Map(int id, MapLevel mapLevel, int bestScore, Texture2D mipmap, bool isLock, List<List<Vector2>> direction, List<List<Vector2>> group)
    {
        this.id = id;
        this.mapLevel = mapLevel;
        this.bestScore = bestScore;
        this.mipmap = mipmap;
        this.isLock = isLock;
        this.direction = direction;
        this.group = group;
    }

    /// <summary>
    /// 获取地图，如果不存在，则返回null。
    /// </summary>
    /// <returns>地图</returns>
    public int[,] GetMap()
    {
        if (MapData.MapID2Data.ContainsKey(id)) return MapData.MapID2Data[id];
        else return null;
    }

    /// <summary>
    /// 获取敌人攻击方向。
    /// </summary>
    /// <returns></returns>
    public List<List<Vector2>> GetDirection()
    {
        return direction;
    }

    /// <summary>
    /// 获取敌人出生位置组。
    /// </summary>
    /// <returns></returns>
    public List<List<Vector2>> GetGroup()
    {
        return group;
    }
}

public static class MapData
{
    /// <summary>
    /// 对他进行设置，用于触发静态构造函数，无其他实际用处，只在特定的地方调用。
    /// </summary>
    public static bool isStart = false;
    /// <summary>
    /// 通过地图id，得到地图数据。
    /// </summary>
    public static Dictionary<int, int[,]> MapID2Data;

    public static List<Map> Maps;

    static List<int[,]> maps;

    #if UNITY_EDITOR
        static string url = @"Assets/StreamingAssets/Map.map";
#elif UNITY_ANDROID
        static string url = "jar:file:///" + Application.streamingAssetsPath + @"//Map.map";
#endif

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
        //Res.Debug_Text.text += "开始加载！";
        mapReader = new MapReader(Res.MapText);

        MapID2Data = new Dictionary<int, int[,]>();
        Maps = new List<Map>();

        for(int i = 0; i < mapReader.GetCount(); i++)
        {
            MapID2Data.Add(i + 1, mapReader.GetMap(i + 1));
            Maps.Add(new Map(i + 1, MapLevel.Easy, 0, null, false, mapReader.GetDirection(i + 1), mapReader.GetGroup(i + 1)));
        }
    }
}
