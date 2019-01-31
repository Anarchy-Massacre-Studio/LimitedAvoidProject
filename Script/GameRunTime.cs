using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SyzygyStudio;
public class GameRunTime : MonoBehaviour
{
    public int L;
    /// <summary>
    /// 当前地图。
    /// </summary>
    static Map _map;

    /// <summary>
    /// 静态属性，分数。
    /// </summary>
    public static int Score { get; private set; }

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
            Res.PlayerT.SetActive(true);
            _map = MapData.Maps[L];
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
                                Res.Grounds.Take().position = new Vector3(i + 1, j + 1, 0);
                                break;
                            case 2:
                                Transform t = Res.BirthPoints.Take();
                                t.position = new Vector3(i + 1, j + 1, 0);
                                break;
                        }
                    }
                }
            }

            

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
        Res.PlayerT.SetActive(false);
        transform.position = new Vector3(0, 0);
        transform.localScale = Vector3.one;
        Res.Grounds.BackAll();
        Res.BirthPoints.BackAll();
        Res.Enemys.BackAll();
    }

    private void Awake()
    {
        

    }

    float time = 0;
    private void Update()
    {
        if (_map != null && gameObject.activeSelf)
        {
            time += Time.deltaTime;
            if(time >= 5f)
            {
                time -= time;
                genEnemys();
            }
        }
    }


    /// <summary>
    /// 生成敌人。
    /// </summary>
    private void genEnemys()
    {
        for (int i = 0; i < _map.GetGroup().Count; i++)
        {
            int r = Random.Range(0, _map.GetGroup()[i].Count);

            var e = Res.Enemys.Take();
            e.localPosition = _map.GetGroup()[i][r];
            e.GetComponent<Enemy>().SetAttackDirection(_map.GetDirection()[i][r]);
        }
    }
}
