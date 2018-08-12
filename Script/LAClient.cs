using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SyzygyStudio;
/*
客户端单例，用于跨脚本数据处理与传输。(比如，lv的ui方法想要调用宇的开始游戏方法，就要先进过这里，在这里进行调用)
*/

namespace LA.UI
{
    using MapLevel = MapInfo.MapLevel;
    /// <summary>
    /// 客户端。
    /// </summary>
    public class LAClient
    {
        #region 成员函数

        /// <summary>
        /// 单例
        /// </summary>
        public void Init()
        {
            g_Ins = this;
        }

        /// <summary>
        /// 初始化一切。
        /// </summary>
        /// <param name="pObject"></param>
        public void InitInfo(GameObject pObject)
        {
            // 绑定。
            m_pUpdate += Update;

            // 多语言初始化。
            m_pLALangue = new LALangue();
            m_pLALangue.InitInfo();

            // 动画处理器初始化。
            m_pLAAnimHandler = new LAAnimHandler();

            // 地图数据初始化。
            m_pLAMapData = new LAMapData();

            // UI初始化。
            m_pCanvas = pObject.transform;
            m_pUILoadingView = new UILoadingView(m_pCanvas.Find("View/LoadingView").gameObject);
            m_pUILoadingView.OnClickSwitchShow = SwitchShowOnClickItem;
            m_pUILoadingView.OnClickSetShowLangue = SetShowOnClickLangue;
            m_pUILoadingView.OnClickSetShowAudio = SetShowOnClickAudio;
            m_pUILoadingView.OnClickLeftAndRightMapLevel = RoomShowOnClickLeftAndRightMapLevel;
            m_pUILoadingView.OnClickPlayGame = RoomShowOnClickPlayGame;
            m_pUILoadingView.OnClickRoomAction = RoomShowOnClickRoomItem;

            // 设置数据。
            int Type = GetPhoneLangueMode();
            SetLangueMode(Type);
            LAClient.g_Ins.FillMapData();


            // 更新数据。
            LAClient.g_Ins.UpdataMapData();

        }

        /// <summary>
        /// 注册Update。
        /// </summary>
        /// <param name="pAction"></param>
        public void AddUpdate(Action pAction)
        {
            m_lUpdates.Add(pAction);
        }

        #region 菜单视图。(LoadingView)

        /// <summary>
        /// 显示加载动画。
        /// </summary>
        /// <param name="b"></param>
        public void ShowLoading(bool b)
        {
            m_pUILoadingView.ShowLoading(b);
        }

        #region 选择面板。(SwitchShow)

        /// <summary>
        /// 单击项目。。
        /// </summary>
        /// <param name="nIndex"></param>
        public void SwitchShowOnClickItem(int nIndex)
        {
            switch (nIndex)
            {
                case 1:
                    ShowActiveView(nIndex);
                    break;
                case 2:
                    ShowActiveView(nIndex);
                    break;
                case 3:
                    ShowActiveView(nIndex);
                    break;
                case 4:
                    Debug.LogError("当场退出。");
                    Application.Quit();
                    break;
            }

        }

        /// <summary>
        /// 激活视图。
        /// </summary>
        public void ShowActiveView(int nIndex)
        {
            m_pUILoadingView.ShowView(nIndex);

            // 特殊处理，比如播放动画。
            switch (nIndex)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:
                    break;
                case 10:


                    break;

            }
        }

        #endregion

        #region 设置面板。(SetShow)

        /// <summary>
        /// 单击切换语言。
        /// </summary>
        /// <param name="nIndex"></param>
        public void SetShowOnClickLangue(int nIndex)
        {
            if (m_nCurLangueIndex == 1)
            {
                m_nCurLangueIndex = 2;
            }
            else
            {
                m_nCurLangueIndex = 1;
            }
            SetLangueMode(m_nCurLangueIndex);
        }

        /// <summary>
        /// 单击声音。
        /// </summary>
        /// <param name="nIndex"></param>
        public void SetShowOnClickAudio(bool b)
        {
            Debug.LogError("单击声音。=>" + b);

        }

        #endregion

        #region 房间面板。(RoomShow)

        /// <summary>
        /// 单击房间左右难度。
        /// </summary>
        /// <param name="b"></param>
        public void RoomShowOnClickLeftAndRightMapLevel(bool b, Action<int, int> pBack)
        {
            LAMapData.Room Room = GetCheckRoom();

            if (!b)
            {
                Room.m_nCheckLv++;
            }
            else if (b)
            {
                Room.m_nCheckLv--;
            }

            pBack(Room.m_nCheckLv, Room.m_nMaxLv);
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        public void RoomShowOnClickPlayGame()
        {
            Debug.LogError("开始游戏嘿嘿嘿。");
        }

        /// <summary>
        /// 单击房间项目。
        /// </summary>
        /// <param name="pRoom"></param>
        private void RoomShowOnClickRoomItem(LAMapData.Room pRoom)
        {
            SetCheckRoom(pRoom);
            m_pUILoadingView.UpdataCheckRoomInfo(pRoom);
        }

        #endregion

        #endregion

        #region 多语言。
        /// <summary>
        /// 注册UI更新。
        /// </summary>
        /// <param name="pAction"></param>
        public void RegisterUI(Action pAction)
        {
            m_pLALangue.RegisterUI(pAction);
        }

        /// <summary>
        /// 通过名称获取多语言。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            return m_pLALangue.GetValue(name);
        }

        /// <summary>
        /// 获取手机默认语言类型。
        /// </summary>
        /// <returns></returns>
        public int GetPhoneLangueMode()
        {
            m_nCurLangueIndex = 2;
            string Langue = Application.systemLanguage.ToString();
            Debug.LogError("获取手机语言类型。=>" + Langue);
            bool bLangue1 = Langue.IndexOf("Chinese") != -1;
            if (bLangue1)
            {
                m_nCurLangueIndex = 1;
            }

            return m_nCurLangueIndex;
        }

        /// <summary>
        /// 设置语言类型。
        /// </summary>
        /// <param name="nLangueMode"></param>
        public void SetLangueMode(int nLangueMode)
        {
            int index = m_pLALangue.SetLangueMode(nLangueMode);
            if (index != 0)
            {
                m_nCurLangueIndex = index;
            }
        }
        #endregion

        #region 地图数据。

        /// <summary>
        /// 设置选中房间。
        /// </summary>
        /// <param name="pRoom"></param>
        public void SetCheckRoom(LAMapData.Room pRoom)
        {
            m_pRoom = pRoom;
        }

        /// <summary>
        /// 拿到选中房间。
        /// </summary>
        /// <returns></returns>
        public LAMapData.Room GetCheckRoom()
        {
            return m_pRoom;
        }

        /// <summary>
        /// 填充所有地图数据。
        /// </summary>
        public void FillMapData()
        {
            List<Map> lReMaps = MapData.Maps;
            LAMapData lMapDatas = new LAMapData();

            // 填充假数据。
            for (int i = 0; i < lReMaps.Count; i++)
            {
                LAMapData.Room Item = new LAMapData.Room();
                Item.m_nId = lReMaps[i].id;
                Item.m_pName = "关卡" + lReMaps[i].id;
                Item.m_pRoomId = "room" + lReMaps[i].id;
                Item.m_nMaxScore = lReMaps[i].bestScore;
                Item.m_nMaxLv = m_pLAMapData.MaxLvEnumTransInt(lReMaps[i].mapLevel);
                Item.m_nCheckLv = Item.m_nMaxLv;
                Item.m_bOpenLock = !lReMaps[i].isLock;
                lMapDatas.m_lRooms.Add(Item);
            }

            m_pLAMapData.FillMapData(lMapDatas);
        }

        /// <summary>
        /// 拿到所有地图数据。
        /// </summary>
        /// <returns></returns>
        public LAMapData GetMapData()
        {
            return m_pLAMapData.GetMapData();
        }

        /// <summary>
        /// 更新地图数据。
        /// </summary>
        public void UpdataMapData()
        {
            LAMapData MapData = LAClient.g_Ins.GetMapData();
            m_pUILoadingView.UpdataDataMapScroll(MapData);
        }

        /// <summary>
        /// 当前选中房间信息。
        /// </summary>
        private LAMapData.Room m_pRoom;

        #endregion

        #region 动画处理器。
        /// <summary>
        /// 注册动画到序列(需要整改，暂时禁用，已想到对策)。
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pTween"></param>
        private void RegisterAnim(string pQName, Tween pTween, float nGoNextTime = 10086)
        {
            m_pLAAnimHandler.RegisterAnim(pQName, pTween, nGoNextTime);
        }

        /// <summary>
        /// 开始播放开始！！！(需要整改，暂时禁用，已想到对策)
        /// </summary>
        private void StartPlayAnim(string pQName, Action pBack = null)
        {
            m_pLAAnimHandler.StartPlayAnim(pQName, pBack);
        }

        #endregion

        // 内部。

        /// <summary>
        /// 每帧更新。
        /// </summary>
        private void Update()
        {
            foreach (Action action in m_lUpdates)
            {
                action();
            }
        }

        #endregion

        #region 成员变量
        private UILoadingView m_pUILoadingView;

        /// <summary>
        /// 根画布。
        /// </summary>
        private Transform m_pCanvas;

        /// <summary>
        /// 多语言。
        /// </summary>
        private LALangue m_pLALangue;

        /// <summary>
        /// 动画处理器。
        /// </summary>
        private LAAnimHandler m_pLAAnimHandler;

        /// <summary>
        /// 当前语言索引(对应语言枚举列表)。
        /// </summary>
        public int m_nCurLangueIndex = 1;

        /// <summary>
        /// update托管。
        /// </summary>
        private List<Action> m_lUpdates = new List<Action>();

        /// <summary>
        /// 更新。
        /// </summary>
        public Action m_pUpdate;

        /// <summary>
        /// 地图数据。
        /// </summary>
        private LAMapData m_pLAMapData;

        /// <summary>
        /// 地图。
        /// </summary>
        private Map m_pMap;
        #endregion

        #region 单例
        public static LAClient g_Ins;
        #endregion
    }

    /// <summary>
    /// 多语言，不是单例！
    /// </summary>
    public class LALangue
    {
        #region 成员函数

        /// <summary>
        /// 初始化一切。
        /// </summary>
        public void InitInfo()
        {
            m_pLangueMode = LangueMode.CN;
            FillLangueData();
        }

        /// <summary>
        /// 填充多语言数据(想要进行多语言来这里啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊)。
        /// </summary>
        private void FillLangueData()
        {
            // 中文
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Start", LangueMode.CN, "开始游戏"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Set", LangueMode.CN, "设置"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Quit", LangueMode.CN, "退出游戏"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_LimitedAvoid", LangueMode.CN, "极限躲避"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Loading", LangueMode.CN, "加载中..."));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_SyzygyStudio", LangueMode.CN, "@Syzygy Studio"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Language", LangueMode.CN, "语言类型:"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_<English>", LangueMode.CN, "<中文>"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Audio", LangueMode.CN, "声音:"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Return", LangueMode.CN, "返回"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_About", LangueMode.CN, "关于"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Studio", LangueMode.CN, "工作室:Syzygy Studio"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Developer", LangueMode.CN, "开发者:宇,lv1,McKay"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Versions", LangueMode.CN, "版本:1.0"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Easy", LangueMode.CN, "简单"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Medium", LangueMode.CN, "正常"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Difficult", LangueMode.CN, "困难"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Nightmare", LangueMode.CN, "噩梦"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Hell", LangueMode.CN, "地狱"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Endless", LangueMode.CN, "无尽"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Play", LangueMode.CN, "开始"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Setting", LangueMode.CN, "设置"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Room", LangueMode.CN, "选择房间"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Checkpoint", LangueMode.CN, "关卡"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Locked", LangueMode.CN, "未解锁"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_GamePlayProgrammer", LangueMode.CN, "游戏逻辑程序:宇"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_UI", LangueMode.CN, "界面:lv1"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Planner", LangueMode.CN, "策划:mckay"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Artist", LangueMode.CN, "美术:Kevin Y. Wong "));
            // 英文
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Start", LangueMode.EN, "Start"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Set", LangueMode.EN, "Set"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Quit", LangueMode.EN, "Quit"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_LimitedAvoid", LangueMode.EN, "Limited Avoid"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Loading", LangueMode.EN, "Loading..."));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_SyzygyStudio", LangueMode.EN, "@Syzygy Studio"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Language", LangueMode.EN, "Language:"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_<English>", LangueMode.EN, "<English>"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Audio", LangueMode.EN, "Audio:"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Return", LangueMode.EN, "Return"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_About", LangueMode.EN, "About"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Studio", LangueMode.EN, "Studio:Syzygy Studio"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Developer", LangueMode.EN, "Developer:宇,lv1,McKay"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Versions", LangueMode.EN, "Versions:1.0"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Easy", LangueMode.EN, "Easy"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Medium", LangueMode.EN, "Medium"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Difficult", LangueMode.EN, "Difficult"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Nightmare", LangueMode.EN, "Nightmare"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Hell", LangueMode.EN, "Hell"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Endless", LangueMode.EN, "Endless"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Return", LangueMode.EN, "Return"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Play", LangueMode.EN, "Play"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Setting", LangueMode.EN, "Setting"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Room", LangueMode.EN, "Room"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Checkpoint", LangueMode.EN, "Room"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Locked", LangueMode.EN, "Locked"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_GamePlayProgrammer", LangueMode.EN, "GamePlay Programmer:宇"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_UI", LangueMode.EN, "UI:lv1"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Planner", LangueMode.EN, "Planner:mckay"));
            m_lLangueInfo.Add(new LangueInfo("LoadingView_Artist", LangueMode.EN, "Artist:Kevin Y. Wong "));

        }

        /// <summary>
        /// 通过名称获取多语言。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            LangueInfo langueInfo = m_lLangueInfo.Find(delegate (LangueInfo Item)
            {
                if (name == Item.Name && m_pLangueMode == Item.ID)
                {
                    return true;
                }
                return false;
            });
            if (langueInfo == null)
            {
                return "未注册语言";
            }
            return langueInfo.Value;
        }

        /// <summary>
        /// 注册UI。
        /// </summary>
        /// <param name="pAction"></param>
        public void RegisterUI(Action pAction)
        {
            lActions.Add(pAction);
        }

        /// <summary>
        /// 更新所有UI。
        /// </summary>
        private void UpdateUIAll()
        {
            foreach (Action action in lActions)
            {
                action();
            }
        }

        /// <summary>
        /// 设置语言类型,返回语言索引。
        /// </summary>
        /// <param name="nLangueMode"></param>
        public int SetLangueMode(int nLangueMode)
        {
            int Index = 0;
            bool b = false;
            foreach (LangueMode item in Enum.GetValues(typeof(LangueMode)))
            {
                if (nLangueMode == GetLangueIndex(item))
                {
                    m_pLangueMode = item;
                    b = true;
                    Index = GetLangueIndex(item);
                    break;
                }
            }
            if (b)
            {
                UpdateUIAll();
            }
            return Index;
        }

        /// <summary>
        /// 通过枚举获取index。
        /// </summary>
        /// <param name="tEnum"></param>
        /// <returns></returns>
        private int GetLangueIndex(LangueMode tEnum)
        {
            int nIndex = 0;
            switch (tEnum)
            {
                case LangueMode.CN:
                    nIndex = 1;
                    break;
                case LangueMode.EN:
                    nIndex = 2;
                    break;
            }
            return nIndex;
        }
        #endregion

        #region 成员变量
        public enum LangueMode
        {
            CN = 1,
            EN = 2
        }

        /// <summary>
        /// 语言类型。
        /// </summary>
        private LangueMode m_pLangueMode;

        /// <summary>
        /// 所有UI。
        /// </summary>
        private List<Action> lActions = new List<Action>();

        /// <summary>
        /// 所有语言信息。
        /// </summary>
        private List<LangueInfo> m_lLangueInfo = new List<LangueInfo>();

        /// <summary>
        /// 语言信息。
        /// </summary>
        private class LangueInfo
        {
            public LangueInfo(string Name, LangueMode ID, string Value)
            {
                this.Name = Name;
                this.ID = ID;
                this.Value = Value;
            }
            public string Name;
            public LangueMode ID;
            public string Value;
        }

        #endregion

    }

    /// <summary>
    /// 动画序列处理。
    /// </summary>
    public class LAAnimHandler
    {
        #region 成员函数

        public LAAnimHandler()
        {

        }

        /// <summary>
        /// 注册动画到序列。
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pTween"></param>
        public void RegisterAnim(string pQName, Tween pTween, float nGoNextTime)
        {
            AnimQueueInfo ItemQ = m_lAnimQInfo.Find(delegate (AnimQueueInfo Item)
            {
                return pQName == Item.Name;
            });

            if (ItemQ != null)
            {
                int Id = ItemQ.lAnim.Count;
                Action<AnimQueueInfo, int, Action> NextAction = PlayNextAnim;
                float GoNextTime = nGoNextTime;
                ItemQ.lAnim.Add(new AnimInfo(Id, pTween, NextAction, GoNextTime));
            }
            else
            {
                int Id = 0;
                Action<AnimQueueInfo, int, Action> NextAction = PlayNextAnim;
                float GoNextTime = nGoNextTime;
                m_lAnimQInfo.Add(new AnimQueueInfo(pQName, new List<AnimInfo>() { new AnimInfo(Id, pTween, NextAction, GoNextTime) }, false));
            }
        }

        /// <summary>
        /// 开始播放开始！！！
        /// </summary>
        public void StartPlayAnim(string pQName, Action pBack)
        {
            AnimQueueInfo ItemQ = m_lAnimQInfo.Find(delegate (AnimQueueInfo Item)
            {
                return pQName == Item.Name;
            });
            if (ItemQ != null)
            {
                if (ItemQ.bActive)
                {
                    Debug.LogError("动画正在播放");
                    return;
                }
                ItemQ.bActive = true;
                PlayNextAnim(ItemQ, 0, pBack);
            }
        }

        /// <summary>
        /// 下一个动画。
        /// </summary>
        private void PlayNextAnim(AnimQueueInfo pAnimQ, int nIndex, Action pBack)
        {
            if (nIndex == pAnimQ.lAnim.Count)
            {
                Debug.LogError("动画播放完啦！");
                pAnimQ.bActive = false;
                ResetAnimQ(pAnimQ);
                pBack();
                return;
            }
            if (pAnimQ.lAnim[nIndex].GoNextTime < 100)// 延迟播放。
            {
                pAnimQ.lAnim[nIndex].Anim.Play();
                float time = 0;
                DOTween.To(() => time, x => time = x, 1, pAnimQ.lAnim[nIndex].GoNextTime).OnComplete(delegate ()
                {
                    PlayNextAnim(pAnimQ, nIndex + 1, pBack);
                });
            }
            else// 播放完当前动画播放。
            {
                pAnimQ.lAnim[nIndex].Anim.Play().OnComplete(delegate ()
                {
                    PlayNextAnim(pAnimQ, nIndex + 1, pBack);
                });
            }
        }

        /// <summary>
        /// 重置动画。
        /// </summary>
        private void ResetAnimQ(AnimQueueInfo pAnimQ)
        {
            foreach (AnimInfo AnimInfo in pAnimQ.lAnim)
            {
                //AnimInfo.Anim.Restart(false,1f);
                //AnimInfo.Anim.Pause();
            }
        }

        #endregion

        #region 成员变量

        /// <summary>
        /// 所有动画队列信息。
        /// </summary>
        private List<AnimQueueInfo> m_lAnimQInfo = new List<AnimQueueInfo>();

        /// <summary>
        /// 动画队列信息。
        /// </summary>
        public class AnimQueueInfo
        {
            public AnimQueueInfo(string Name, List<AnimInfo> lAnim, bool bActive)
            {
                this.Name = Name;
                this.lAnim = lAnim;
                this.bActive = bActive;
            }
            public string Name;
            public List<AnimInfo> lAnim;
            public bool bActive;
        }

        /// <summary>
        /// 动画信息
        /// </summary>
        public class AnimInfo
        {
            public AnimInfo(int Id, Tween Anim, Action<AnimQueueInfo, int, Action> NextAction, float GoNextTime)
            {
                this.Id = Id;
                this.Anim = Anim;
                this.NextAction = NextAction;
                this.GoNextTime = GoNextTime;

            }
            public int Id;
            public Tween Anim;
            public Action<AnimQueueInfo, int, Action> NextAction;
            public float GoNextTime;
        }

        private Action AnimPlayEndAction;
        #endregion
    }

    /// <summary>
    /// 地图数据。
    /// </summary>
    public class LAMapData
    {
        public LAMapData()
        {

        }

        /// <summary>
        /// 填充所有地图数据。
        /// </summary>
        public void FillMapData(LAMapData pLAMapData)
        {
            m_pLAMapData = pLAMapData;
        }

        /// <summary>
        /// 难度转换,索引转枚举。
        /// </summary>
        public MapLevel MaxLvIntTransEnum(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return MapLevel.Easy;
                case 1:
                    return MapLevel.Medium;
                case 2:
                    return MapLevel.Difficult;
                case 3:
                    return MapLevel.Nightmare;
                case 4:
                    return MapLevel.Hell;
                case 5:
                    return MapLevel.Endless;
            }


            return MapLevel.Easy;
        }

        /// <summary>
        /// 难度转换,枚举转索引。
        /// </summary>
        public int MaxLvEnumTransInt(MapLevel eLv)
        {
            switch (eLv)
            {
                case MapLevel.Easy:
                    return 0;
                case MapLevel.Medium:
                    return 1;
                case MapLevel.Difficult:
                    return 2;
                case MapLevel.Nightmare:
                    return 3;
                case MapLevel.Hell:
                    return 4;
                case MapLevel.Endless:
                    return 5;
            }
            return 0;
        }

        /// <summary>
        /// 获取所有地图数据。
        /// </summary>
        /// <returns></returns>
        public LAMapData GetMapData()
        {
            return m_pLAMapData;
        }

        /// <summary>
        /// 地图数据。
        /// </summary>
        private LAMapData m_pLAMapData;

        /// <summary>
        /// 所有房间。
        /// </summary>
        public List<Room> m_lRooms = new List<Room>();

        /// <summary>
        /// 房间信息。
        /// </summary>
        public class Room
        {
            /// <summary>
            /// 房间索引,关卡。
            /// </summary>
            public int m_nId;

            /// <summary>
            /// 房间唯一编号
            /// </summary>
            public string m_pRoomId;

            /// <summary>
            /// 房间类型。
            /// </summary>
            public int m_nType;

            /// <summary>
            /// 房间名。
            /// </summary>
            public string m_pName;

            /// <summary>
            /// 最高积分。
            /// </summary>
            public int m_nMaxScore;

            /// <summary>
            /// 最高难度。
            /// </summary>
            public int m_nMaxLv;

            /// <summary>
            /// 选中难度。
            /// </summary>
            public int m_nCheckLv;

            /// <summary>
            /// 封面图相对路径。
            /// </summary>
            public string m_pImgUrl;

            /// <summary>
            /// 封面图。
            /// </summary>
            public Texture2D m_pImgTex;

            /// <summary>
            /// 是否解锁。
            /// </summary>
            public bool m_bOpenLock;
        }

    }

    /// <summary>
    /// 滑动事件监听器。
    /// </summary>
    public class LAScrollEvent
    {
        public LAScrollEvent()
        {
            m_pOnDragEvetn = new Event(EventTriggerType.Drag);
            m_pEndDragEvetn = new Event(EventTriggerType.EndDrag);
        }

        /// <summary>
        /// 注册事件。
        /// </summary>
        /// <param name="pObject"></param>
        /// <param name="action"></param>
        /// <param name="eEvent"></param>
        public void AddEvent(ScrollRect pObject, Action<PointerEventData> action, EventTriggerType eEvent)
        {
            if (pObject.GetComponent<EventTrigger>() == null)
            {
                m_pTrigger = pObject.gameObject.AddComponent<EventTrigger>();
            }
            else
            {
                m_pTrigger = pObject.GetComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            switch (eEvent)
            {
                case EventTriggerType.Drag:
                    m_pOnDragEvetn.EventAction = action;
                    entry.eventID = EventTriggerType.Drag;
                    entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
                    m_pTrigger.triggers.Add(entry);
                    break;
                case EventTriggerType.EndDrag:
                    m_pEndDragEvetn.EventAction = action;
                    entry.eventID = EventTriggerType.EndDrag;
                    entry.callback.AddListener((data) => { EndDrag((PointerEventData)data); });
                    m_pTrigger.triggers.Add(entry);
                    break;
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            m_pOnDragEvetn.EventAction(eventData);
        }

        public void EndDrag(PointerEventData eventData)
        {
            m_pEndDragEvetn.EventAction(eventData);
        }

        public EventTrigger m_pTrigger;

        private Event m_pOnDragEvetn;

        private Event m_pEndDragEvetn;

        /// <summary>
        /// 事件。
        /// </summary>
        private class Event
        {
            public Event(EventTriggerType eEventType)
            {
                m_eEventType = eEventType;
            }

            private EventTriggerType m_eEventType;

            public Action<PointerEventData> EventAction;
        }

    }

    /// <summary>
    ///  文本处理工具。
    /// </summary>
    public class TextTools
    {
        /// <summary>
        ///  切割屁股后面的字符。
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="size">切几个</param>
        /// <returns></returns>
        public static string[] CutTextEndDot(string str, int size)
        {
            string[] Text;
            if (str.Length <= size)
            {
                Debug.LogError("截你个头啊");
                Text = new string[1];
                Text[0] = str;
                return Text;
            }
            Text = new string[size + 1];
            Text[0] = str.Trim().Substring(0, str.Length - size);
            for (int i = 1; i < Text.Length; i++)
            {
                Text[i] = str.Trim().Substring(str.Length - size, 1);
            }
            return Text;
        }

    }
}