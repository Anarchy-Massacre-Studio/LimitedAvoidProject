using UnityEngine;

public delegate void BackAction<T>(T transform);
public delegate void BackAction<T,T1>(T transform, ref T1 time);

public class BackInfo : MonoBehaviour
{
    private BackAction<Transform> BackMathedT { get; set; }
    private BackAction<Transform, float> BackMathedTF { get; set; }

    float time = 0f;

    /// <summary>
    /// 通过项的Transform信息设置项的隐藏方式。
    /// </summary>
    /// <param name="action"></param>
    public void SetAction(BackAction<Transform> action)
    {
        time = 0f;
        BackMathedTF = null;
        BackMathedT = action;
    }

    /// <summary>
    /// 通过项的Transform信息和一个float计时器来设置项的隐藏方式。
    /// </summary>
    /// <param name="action"></param>
    public void SetAction(BackAction<Transform, float> action)
    {
        time = 0f;
        BackMathedT = null;
        BackMathedTF = action;
    }

    private void Update()
    {
        if (BackMathedT != null) BackMathedT(transform);
        if (BackMathedTF != null) BackMathedTF(transform, ref time);
        
    }

    private void OnEnable()
    {
        time = 0f;
    }
}
