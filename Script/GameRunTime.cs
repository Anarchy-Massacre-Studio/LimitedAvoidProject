using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SyzygyStudio;
public class GameRunTime : MonoBehaviour
{
    /// <summary>
    /// 当前地图。
    /// </summary>
    static Map _map;

    /// <summary>
    /// 静态属性，分数。
    /// </summary>
    public static int Score { get; private set; }

    GameObject ground;
    GameObject birthPoint;

    /// <summary>
    /// 设置当前地图。
    /// </summary>
    /// <param name="map"></param>
    public static void SetMap(Map map)
    {
        _map = map;
    }

    private void OnEnable()
    {
        if (ResourcesManager.isLoadFinish)
        {
            Debug.Log(ResourcesManager.isLoadFinish);
            ground = Res.Ground;
            birthPoint = Res.BirthPoint;
            _map = new Map(3, MapInfo.MapLevel.Easy, 1, null, false);
            if (_map != null)
            {
                for (int i = 0; i < _map.GetMap().GetLength(1); i++)
                {
                    for (int j = 0; j < _map.GetMap().GetLength(0); j++)
                    {
                        int ij = _map.GetMap()[j, i];
                        switch (ij)
                        {
                            case 0:
                                break;
                            case 1:
                                Instantiate(ground, new Vector3(i + 1, j + 1, 0), ground.transform.rotation, gameObject.transform);
                                break;
                            case 2:
                                Instantiate(birthPoint, new Vector3(i + 1, j + 1, 0), birthPoint.transform.rotation, gameObject.transform);
                                break;
                        }
                    }
                }
            }

            Res.Enemys.Take();

            int x_c = _map.GetMap().GetLength(1) / 2; //获得地图中心位置坐标x。
            int y_c = _map.GetMap().GetLength(0) / 2; //获得地图中心位置坐标y。
            if (_map.GetMap().GetLength(1) % 2 != 0) x_c++;
            if (_map.GetMap().GetLength(0) % 2 != 0) y_c++;
            float x_c_offset = x_c / -2f;
            float y_c_offset = y_c / -2f;

            transform.position = new Vector3(x_c_offset, y_c_offset);

            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Debug.Log(_map.GetMap().GetLength(0) + "+" + _map.GetMap().GetLength(1));
        }
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    private void Awake()
    {
        

    }

    private void Update()
    {
        if (_map != null && gameObject.activeSelf)
        {

        }
    }
}
