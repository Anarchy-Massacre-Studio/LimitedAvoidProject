using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LA.UI;
using System;

/*
  启动脚本。
     */

public class LAManage : MonoBehaviour {

	void Start () {
        Init();
        Load();
    }

    private void Init()
    {
        // 嘿嘿嘿。  
        m_pLAClient = new LAClient();


        // 初始化。
        m_pLAClient.Init();
        
    }

    private void Load()
    {
        LAClient.g_Ins.InitInfo(Canvas);
        m_pUpdate = LAClient.g_Ins.m_pUpdate;

    }

    private void Update()
    {
        if (ResourcesManager.isLoadFinish)
        {
            m_pLAClient.ShowLoading(false);
        }

        if (m_pUpdate != null)
        {
            m_pUpdate();
        }
    

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LAClient.g_Ins.UpdataMapData();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
          
        }

    }

    #region 成员变量
    /// <summary>
    /// 画布根节点。
    /// </summary>
    public GameObject Canvas;

    /// <summary>
    /// 客户端。
    /// </summary>
    private LAClient m_pLAClient;

    /// <summary>
    /// 更新。
    /// </summary>
    private Action m_pUpdate;

    #endregion
}
