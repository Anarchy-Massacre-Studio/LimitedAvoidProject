using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using SyzygyStudio;
using System.IO;

public class ResourcesManager : MonoBehaviour
{
    public LAManage LAManage;
    public Transform Map;
    public Transform Pack;
    public Text View_Loading;
    public RectTransform SwitchShow;
    public static bool isLoadFinish = false;

    private TextAsset MapTextAsset;

    //public Text Debug_Text;

    private void Awake()
    {
        //Map = FindObjectOfType<GameRunTime>().transform;
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        ///*
        yield return MapTextAsset = Resources.Load<TextAsset>("Map");
        //Res.Debug_Text = Debug_Text;
        Res.MapText = MapTextAsset.bytes;

        /*
        foreach (var b in MapTextAsset.bytes)
        {
            Debug_Text.text += b;
        }
        MemoryStream memoryStream = new MemoryStream(MapTextAsset.bytes);
        StreamReader streamReader = new StreamReader(memoryStream);
        Debug_Text.text += streamReader.ReadToEnd();
        */
        MapData.isStart = true;  //触发MapData类的静态构造函数。
        /*
        streamReader.Close();
        streamReader.Dispose();
        memoryStream.Close();
        memoryStream.Dispose();
        */


        #region 得到素材。
        yield return Res.Ground = Resources.Load<GameObject>("Ground");
        yield return Res.BirthPoint = Resources.Load<GameObject>("BirthPoint");
        yield return Res.Player = Resources.Load<GameObject>("Player");
        yield return Res.Enemy = Resources.Load<GameObject>("Enemy");
        yield return Res.Die = Resources.Load<GameObject>("Die");
        #endregion
        ///* 
        #region 生成素材。
        yield return Res.PlayerT = Instantiate(Res.Player);
        Res.PlayerT.transform.SetParent(Pack);
        #endregion

        #region 实例化组。

        #region 实例化粒子效果组。
        Res.Dies = new Group<Transform>();                           //实例化一个粒子效果组。
        GameObject d;
        yield return d = Instantiate(Res.Die);
        Res.Dies.Add(d.transform);                                   //添加元素。
        Res.Dies.Back(delegate(Transform t, ref float time)                     //设置他的隐藏方法。
        {
            time += Time.deltaTime;
            if(time > 0.8f)
            {
                t.gameObject.SetActive(false);
            }
        });
        d.transform.SetParent(Pack);

        #endregion

        #region 实例化敌人组。

        Res.Enemys = new Group<Transform>();                        //实例化一个敌人组。

        for (int i = 0; i < MapReader.birthPointCountMax; i++)                                 //添加敌人。
        {
            GameObject g;
            yield return g = Instantiate(Res.Enemy, Map);
            Res.Enemys.Add(g.transform);
        }

        Res.Enemys.SetPose(new Pose(new Vector2(-3, -3), Quaternion.identity));  //设置敌人的位置和朝向。
        Res.Enemys.Back((t) =>                                                    //设置敌人的隐藏方式。
        {
            if (Mathf.Abs(t.position.x) >= 6f || Mathf.Abs(t.position.y) >= 6f)
            {
                t.gameObject.SetActive(false);
            }
        });

        #endregion

        #region 实例化地图组。
        Res.Grounds = new Group<Transform>();

        for(int i = 0; i < MapReader.groundCountMax; i++)
        {
            GameObject g;
            yield return g = Instantiate(Res.Ground, Map);
            Res.Grounds.Add(g.transform);
        }

        Res.BirthPoints = new Group<Transform>();

        for(int i = 0; i < MapReader.birthPointCountMax; i++)
        {
            GameObject g;
            yield return g = Instantiate(Res.BirthPoint, Map);
            Res.BirthPoints.Add(g.transform);
        }
        #endregion

        #endregion
        LAManage.enabled = true;
        //*/
        yield return new WaitForSecondsRealtime(1f);

        isLoadFinish = true;

        View_Loading.DOFade(0f,1f).OnComplete(() => SwitchShow.DOLocalMoveX(0,0.5f));
    }
}

public static class Res
{
    public static GameObject Ground = null;
    public static GameObject BirthPoint = null;
    public static GameObject Player = null;
    public static GameObject Enemy = null;
    public static GameObject Die = null;

    public static Group<Transform> Enemys = null;
    public static Group<Transform> Dies = null;
    public static Group<Transform> Grounds = null;
    public static Group<Transform> BirthPoints = null;

    public static GameObject PlayerT = null;

    public static List<GameObject> Maps = null;

    public static byte[] MapText;

    //public static Text Debug_Text;
}
