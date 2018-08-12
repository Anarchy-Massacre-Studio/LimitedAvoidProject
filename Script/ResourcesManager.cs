using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SyzygyStudio;

public class ResourcesManager : MonoBehaviour
{
    public static bool isLoadFinish = false;

    private void Awake()
    {
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        #region 得到素材。
        yield return Res.Ground = Resources.Load<GameObject>("Ground");
        yield return Res.BirthPoint = Resources.Load<GameObject>("BirthPoint");
        yield return Res.Player = Resources.Load<GameObject>("Player");
        yield return Res.Enemy = Resources.Load<GameObject>("Enemy");
        yield return Res.Die = Resources.Load<GameObject>("Die");
        #endregion

        #region 生成素材。
        yield return Instantiate(Res.Player);
        #endregion

        #region 实例化组。
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

        Res.Enemys = new Group<Transform>();                        //实例化一个敌人组。

        for (int i = 0; i < 6; i++)                                 //添加敌人。
        {
            GameObject g;
            yield return g = Instantiate(Res.Enemy);
            Res.Enemys.Add(g.transform);
        }

        Res.Enemys.SetPose(new Pose(new Vector2(-3, -3), Quaternion.identity));  //设置敌人的位置和朝向。
        Res.Enemys.Back((t) =>                                                    //设置敌人的隐藏方式。
        {
            if (Mathf.Abs(t.position.x) >= 5f || Mathf.Abs(t.position.y) >= 5f)
            {
                t.gameObject.SetActive(false);
            }
        });

        #endregion


        isLoadFinish = true;
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

    public static List<GameObject> Maps = null;
}
