using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LA.UI;
using System;

/*
  启动脚本。
     */

public class LAManage : MonoBehaviour
{
    //public ResourcesManager ResourcesManager;

	void Start () 
    {   
        Init();
        Load();
    }
    private void Init()
    {
        // 嘿嘿嘿。  
        m_pLAClient = new LAClient();

        color_map.Add(0,0.138f);
        color_map.Add(1,0.485f);
        color_map.Add(2,0.205f);
        color_map.Add(3,0.342f);
        color_map.Add(4,0.768f);
        color_map.Add(5,0.733f);
        color_map.Add(6,0.096f);

        float h;

        if(color_map.TryGetValue(UnityEngine.Random.Range(0,7), out h)) colorGen = new ColorGen(h);
        else colorGen = new ColorGen(0.138f);
        
        Camera.backgroundColor = colorGen.original;

        TextTools.FindCom(Canvas.transform, ref tList);
        TextTools.FindCom(Content, ref iList);
        TextTools.FindCom(MapNode, ref sList);

        foreach (var item in tList)
		{
			item.color = new Color(colorGen.dark_3.r, colorGen.dark_3.g, colorGen.dark_3.b ,item.color.a);
		}
        foreach (var item in iList)
		{
			item.color = new Color(colorGen.dark_1.r, colorGen.dark_1.g, colorGen.dark_1.b ,item.color.a);
		}
        foreach (var item in sList)
        {
            item.color = new Color(colorGen.dark_2.r, colorGen.dark_2.g, colorGen.dark_2.b, item.color.a);
        }

        // 初始化。
        m_pLAClient.Init();
        
    }

    private void Load()
    {
        LAClient.g_Ins.InitInfo(Canvas);
        m_pUpdate = LAClient.g_Ins.m_pUpdate;
        //MapData.isStart = false;  //触发MapData类的静态构造函数。
    }

    private void Update()
    {
        //if (MapData.isStart == true)
        //{
        //ResourcesManager.enabled = true;

        m_pUpdate?.Invoke();

        if (Input.GetKeyDown(KeyCode.Q))
            {
                LAClient.g_Ins.UpdataMapData();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

            }
        //}
    }

    #region 成员变量
    /// <summary>
    /// 画布根节点。
    /// </summary>
    public GameObject Canvas;

    /// <summary>
    /// 选择关卡节点。
    /// </summary>
    public Transform Content;

    /// <summary>
    /// 地图容器根节点。
    /// </summary>
    public Transform MapNode;

    /// <summary>
    /// 摄像机。
    /// </summary>
    public Camera Camera;

    /// <summary>
    /// 生成颜色类。
    /// </summary>
    private ColorGen colorGen;

    /// <summary>
    /// 客户端。
    /// </summary>
    private LAClient m_pLAClient;

    /// <summary>
    /// 更新。
    /// </summary>
    private Action m_pUpdate;

    /// <summary>
    /// Text组件List。
    /// </summary>
    /// <typeparam name="Text"></typeparam>
    /// <returns></returns>
    private List<Text> tList = new List<Text>();
    /// <summary>
    /// 选择关卡Image组件List。
    /// </summary>
    /// <typeparam name="Image"></typeparam>
    /// <returns></returns>
    private List<Image> iList = new List<Image>();
    /// <summary>
    /// SpriteRenderer组件List。
    /// </summary>
    private List<SpriteRenderer> sList = new List<SpriteRenderer>();

    /// <summary>
    /// 颜色映射表。
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="float"></typeparam>
    /// <returns></returns>
    private Dictionary<int,float> color_map = new Dictionary<int, float>();

    #endregion
}
