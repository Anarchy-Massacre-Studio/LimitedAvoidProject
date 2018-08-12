using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

/*
    所有的ui都写在这里。
     */

namespace LA.UI
{
    /// <summary>
    /// 加载页面。
    /// </summary>
    public class UILoadingView
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="pObject"></param>
        public UILoadingView(GameObject pObject)
        {
            m_pRoot = pObject;
            m_pContent = m_pRoot.transform.Find("Content").gameObject;
            m_pLoadingText = m_pRoot.transform.Find("Hint/Text").GetComponent<Text>();
            m_pSwitchShow = new SwitchShow(m_pContent.transform.Find("SwitchShow").gameObject);
            m_pSetShow = new SetShow(m_pContent.transform.Find("SetShow").gameObject);
            m_pRoomShow = new RoomShow(m_pContent.transform.Find("RoomShow").gameObject);
            m_pAboutShow = new AboutShow(m_pContent.transform.Find("AboutShow").gameObject);

            m_pSwitchShow.OnClickSwitchShowItem = OnClickSwitch;
            m_pSetShow.OnClickReturnBtnAction = OnClickReturn;
            m_pSetShow.OnClickLangueItemAction = OnClickLangue;
            m_pSetShow.OnClickAudioOpenAction = OnClickAudio;
            m_pRoomShow.OnClickLeftAndRightMapLevelAction = OnClickLeftAndRightMapLevelItem;
            m_pRoomShow.OnClickPlayAction = OnClickPlay;
            m_pRoomShow.OnClickReturnBtnAction = OnClickReturn;
            m_pAboutShow.OnClickReturnBtnAction = OnClickReturn;
            m_pRoomShow.OnClickRoomAction = OnClickRoomItem;

            // 注册UI
            LAClient.g_Ins.RegisterUI(UpdateUI);
            LAClient.g_Ins.AddUpdate(Update);
        }

        /// <summary>
        /// 设置启动状态。
        /// </summary>
        public void SetActive(bool b)
        {
            m_pRoot.SetActive(b);
        }

        /// <summary>
        /// 更新UI。
        /// </summary>
        private void UpdateUI()
        {
            UpdataCheckRoomInfo(LAClient.g_Ins.GetCheckRoom());
            m_pLoadingText.text = LAClient.g_Ins.GetValue("LoadingView_Loading");
            m_pSwitchShow.UpdateUI();
            m_pSetShow.UpdateUI();
            m_pRoomShow.UpdateUI();
            m_pAboutShow.UpdateUI();
        }

        /// <summary>
        /// 更新。
        /// </summary>
        private void Update()
        {
            LoadingAnim();

        }

        /// <summary>
        /// 单击选项。
        /// </summary>
        private void OnClickSwitch(int nIndex)
        {
            OnClickSwitchShow(nIndex);
        }

        /// <summary>
        /// 单击返回按钮。
        /// </summary>
        /// <param name="nIndex"></param>
        private void OnClickReturn(int nIndex)
        {
            m_pSwitchShow.SetActive(true);

            switch (nIndex)
            {
                case 1://从设置 返回 选择面板
                    m_pSetShow.SetActive(false);
                    break;
                case 2://从房间 返回 选择面板
                    m_pRoomShow.SetActive(false);
                    break;
                case 3://从关于 返回 选择面板
                    m_pAboutShow.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// 点击语言。
        /// </summary>
        /// <param name="nIndex"></param>
        private void OnClickLangue(int nIndex)
        {
            OnClickSetShowLangue(nIndex);
        }

        /// <summary>
        /// 点击声音。
        /// </summary>
        /// <param name="nIndex"></param>
        private void OnClickAudio(bool b)
        {
            OnClickSetShowAudio(b);
        }

        /// <summary>
        /// 单击左右切换难度按钮。
        /// </summary>
        /// <param name="b"></param>
        private void OnClickLeftAndRightMapLevelItem(bool b, Action<int, int> pBack)
        {
            OnClickLeftAndRightMapLevel(b, pBack);
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        private void OnClickPlay()
        {
            OnClickPlayGame();
        }

        /// <summary>
        /// 单击房间项目。
        /// </summary>
        /// <param name="pRoom"></param>
        private void OnClickRoomItem(LAMapData.Room pRoom)
        {
            OnClickRoomAction(pRoom);
        }

        /// <summary>
        /// 跟新地图卷轴数据。
        /// </summary>
        /// <param name="pMapData"></param>
        public void UpdataDataMapScroll(LAMapData pMapData)
        {
            m_pRoomShow.UpdataDataMapScroll(pMapData);
        }

        /// <summary>
        /// 更新选中房间信息。
        /// </summary>
        public void UpdataCheckRoomInfo(LAMapData.Room pRoom)
        {
            if (pRoom != null)
            {
                m_pRoomShow.UpdataCheckRoomInfo(pRoom);
                ShowPlay(pRoom.m_bOpenLock);
            }
        }

        /// <summary>
        /// 激活视图。
        /// </summary>
        public void ShowView(int nIndex)
        {
            //m_pSwitchShow.SetActive(false);
            switch (nIndex)
            {
                case 1:// 开始游戏
                    m_pRoomShow.SetActive(true);
                    break;
                case 2:// 设置面板
                    m_pSetShow.SetActive(true);
                    break;
                case 3:// 关于面板
                    m_pAboutShow.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// 激活加载中。
        /// </summary>
        public void ShowLoading(bool b)
        {
            m_pLoadingText.gameObject.SetActive(b);
            m_bShowLoadingAnim = b;
        }

        /// <summary>
        /// 显示得分。
        /// </summary>
        /// <param name="nScore"></param>
        public void ShowScore(int nScore)
        {
            m_pRoomShow.ShowScore(nScore);
        }

        /// <summary>
        /// 是否可以玩。
        /// </summary>
        /// <param name="b"></param>
        public void ShowPlay(bool b)
        {
            m_pRoomShow.ShowPlay(b);
        }

        /// <summary>
        /// 加载动画。
        /// </summary>
        private void LoadingAnim()
        {
            if (m_bShowLoadingAnim)
            {
                m_pTime += Time.deltaTime;
                if (m_pTime > 0.2f)
                {
                    m_pTime = 0;
                    int cut = 3;
                    string Text = m_pLoadingText.text.Trim().Substring(0, m_pLoadingText.text.Trim().Length - cut);
                    string info = "";
                    info += Text;
                    for (int i = 0; i < cut; i++)
                    {
                        if (i == m_nUpDotIndex)
                        {
                            info += ",";
                        }
                        else
                        {
                            info += ".";
                        }
                    }
                    m_pLoadingText.text = info;
                    m_nUpDotIndex++;
                    if (m_nUpDotIndex >= 3)
                    {
                        m_nUpDotIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 根节点。
        /// </summary>
        private GameObject m_pRoot;

        /// <summary>
        /// 内容。
        /// </summary>
        private GameObject m_pContent;

        /// <summary>
        /// 加载中文本。
        /// </summary>
        private Text m_pLoadingText;

        /// <summary>
        /// 是否显示加载中动画。
        /// </summary>
        private bool m_bShowLoadingAnim = true;

        /// <summary>
        /// 加载中点点点小帐篷索引。
        /// </summary>
        private int m_nUpDotIndex;

        /// <summary>
        /// 更新频率秒表。
        /// </summary>
        private float m_pTime;

        /// <summary>
        /// 菜单选择面板。
        /// </summary>
        private SwitchShow m_pSwitchShow;

        /// <summary>
        /// 设置面板。
        /// </summary>
        private SetShow m_pSetShow;

        /// <summary>
        /// 房间面板。
        /// </summary>
        private RoomShow m_pRoomShow;

        /// <summary>
        /// 关于面板。
        /// </summary>
        private AboutShow m_pAboutShow;

        /// <summary>
        /// 单击菜单选择面板选项。
        /// </summary>
        public Action<int> OnClickSwitchShow;

        /// <summary>
        /// 单击设置面板语言选项。
        /// </summary>
        public Action<int> OnClickSetShowLangue;

        /// <summary>
        /// 单击设置面板声音选项。
        /// </summary>
        public Action<bool> OnClickSetShowAudio;

        /// <summary>
        /// 单击左右切换难度按钮。
        /// </summary>
        public Action<bool, Action<int, int>> OnClickLeftAndRightMapLevel;

        /// <summary>
        /// 开始游戏。
        /// </summary>
        public Action OnClickPlayGame;

        /// <summary>
        /// 单击房间。
        /// </summary>
        public Action<LAMapData.Room> OnClickRoomAction;

        /// <summary>
        /// 菜单选择面板。
        /// </summary>
        private class SwitchShow
        {
            public SwitchShow(GameObject pObject)
            {
                m_pRoot = pObject;
                m_pStart = m_pRoot.transform.Find("Scroll View1/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pSet = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pAbout = m_pRoot.transform.Find("Scroll View3/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pQuit = m_pRoot.transform.Find("Scroll View4/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pHeadTitle = m_pRoot.transform.Find("Head/Text").GetComponent<Text>();
                m_pStudioTag = m_pRoot.transform.Find("Tag/Text").GetComponent<Text>();

                m_pStart.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickItem(1);
                });
                m_pSet.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickItem(2);
                });
                m_pAbout.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickItem(3);
                });
                m_pQuit.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickItem(4);
                });

                RegisterAnim();

            }

            /// <summary>
            /// 设置启动状态。
            /// </summary>
            public void SetActive(bool b, bool bAnim = true)
            {
                tween.Kill(true);
                if (!bAnim)
                {
                    m_pRoot.SetActive(b);
                    return;
                }

                if (b)
                {

                    m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(-500, 0).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                        m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);
                    });
                }
                else
                {
                    tween = m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(-500, 0.5f).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                    });
                }
            }

            /// <summary>
            /// 更新UI。
            /// </summary>
            public void UpdateUI()
            {
                m_pStart.text = LAClient.g_Ins.GetValue("LoadingView_Start");
                m_pSet.text = LAClient.g_Ins.GetValue("LoadingView_Set");
                m_pAbout.text = LAClient.g_Ins.GetValue("LoadingView_About");
                m_pQuit.text = LAClient.g_Ins.GetValue("LoadingView_Quit");
                m_pHeadTitle.text = LAClient.g_Ins.GetValue("LoadingView_LimitedAvoid");
                m_pStudioTag.text = LAClient.g_Ins.GetValue("LoadingView_SyzygyStudio");

            }

            /// <summary>
            /// 单击项目。
            /// </summary>
            /// <param name="Index"></param>
            private void OnClickItem(int pIndex)
            {
                if (pIndex == 4)
                {
                    OnClickSwitchShowItem(pIndex);
                    return;
                }

                OnClickSwitchShowItem(pIndex);
                SetActive(false);
            }

            /// <summary>
            /// 注册动画。
            /// </summary>
            private void RegisterAnim()
            {

            }

            /// <summary>
            /// 开始文本。
            /// </summary>
            private Text m_pStart;

            /// <summary>
            /// 设置文本。
            /// </summary>
            private Text m_pSet;

            /// <summary>
            /// 关于文本。
            /// </summary>
            private Text m_pAbout;

            /// <summary>
            /// 退出文本。
            /// </summary>
            private Text m_pQuit;

            /// <summary>
            /// 标题。
            /// </summary>
            private Text m_pHeadTitle;

            /// <summary>
            /// 工作室标签。
            /// </summary>
            private Text m_pStudioTag;

            /// <summary>
            /// 根节点。
            /// </summary>
            private GameObject m_pRoot;

            /// <summary>
            /// 单击菜单选择面板。
            /// </summary>
            public Action<int> OnClickSwitchShowItem;

            private Tween tween;
        }

        /// <summary>
        /// 设置面板。
        /// </summary>
        private class SetShow
        {
            public SetShow(GameObject pObject)
            {
                m_pRoot = pObject;
                m_pReturn = m_pRoot.transform.Find("Scroll View4/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pLangue = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pAudio = m_pRoot.transform.Find("Scroll View3/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pLangueType = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show/Langue").GetComponent<Text>();
                m_pAudioYes = m_pRoot.transform.Find("Scroll View3/Viewport/Content/Item/Show/Yes").GetComponent<Button>();
                m_pAudioNo = m_pRoot.transform.Find("Scroll View3/Viewport/Content/Item/Show/No").GetComponent<Button>();
                m_pTitle = m_pRoot.transform.Find("Scroll View0/Viewport/Content/Item/Show/Text").GetComponent<Text>();

                m_pReturn.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickReturnBtn(1);
                });
                m_pLangueType.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickLangueItem(0);
                });
                m_pAudioYes.onClick.AddListener(delegate ()
                {
                    OnClickAudioOpen(true);
                });
                m_pAudioNo.onClick.AddListener(delegate ()
                {
                    OnClickAudioOpen(false);
                });


            }

            /// <summary>
            /// 设置启动状态。
            /// </summary>
            public void SetActive(bool b, bool bAnim = true)
            {
                tween.Kill(true);
                if (!bAnim)
                {
                    m_pRoot.SetActive(b);
                    return;
                }

                if (b)
                {
                    m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(500, 0).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                        m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);

                    });
                }
                else
                {
                    tween = m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(500, 0.5f).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                    });
                }
            }

            /// <summary>
            /// 更新UI。
            /// </summary>
            public void UpdateUI()
            {
                m_pLangue.text = LAClient.g_Ins.GetValue("LoadingView_Language");
                m_pLangueType.text = LAClient.g_Ins.GetValue("LoadingView_<English>");
                m_pAudio.text = LAClient.g_Ins.GetValue("LoadingView_Audio");
                m_pReturn.text = LAClient.g_Ins.GetValue("LoadingView_Return");
                m_pTitle.text = LAClient.g_Ins.GetValue("LoadingView_Setting");
            }

            /// <summary>
            /// 单击返回项目。
            /// </summary>
            /// <param name="nIndex"></param>
            public void OnClickReturnBtn(int nIndex)
            {
                OnClickReturnBtnAction(nIndex);
            }

            /// <summary>
            /// 单击语言。
            /// </summary>
            /// <param name="nIndex"></param>
            public void OnClickLangueItem(int nIndex)
            {
                OnClickLangueItemAction(nIndex);
            }

            /// <summary>
            /// 声音开关。
            /// </summary>
            /// <param name="b"></param>
            public void OnClickAudioOpen(bool b)
            {
                OnClickAudioOpenAction(b);
                m_pAudioYes.gameObject.SetActive(!b);
                m_pAudioNo.gameObject.SetActive(b);
            }

            /// <summary>
            /// 根节点。
            /// </summary>
            private GameObject m_pRoot;

            /// <summary>
            /// 标题。
            /// </summary>
            private Text m_pTitle;

            /// <summary>
            /// 返回。
            /// </summary>
            private Text m_pReturn;

            /// <summary>
            /// 语言 。
            /// </summary>
            private Text m_pLangue;

            /// <summary>
            /// 声音。
            /// </summary>
            private Text m_pAudio;

            /// <summary>
            /// 语言类型。
            /// </summary>
            private Text m_pLangueType;

            /// <summary>
            /// 开声音。
            /// </summary>
            private Button m_pAudioYes;

            /// <summary>
            /// 关声音。
            /// </summary>
            private Button m_pAudioNo;

            /// <summary>
            /// 单击返回。
            /// </summary>
            public Action<int> OnClickReturnBtnAction;

            /// <summary>
            /// 单击语言。
            /// </summary>
            public Action<int> OnClickLangueItemAction;

            /// <summary>
            /// 单击声音开关。
            /// </summary>
            public Action<bool> OnClickAudioOpenAction;

            private Tween tween;
        }

        /// <summary>
        /// 关卡选择面板。
        /// </summary>
        private class RoomShow
        {
            public RoomShow(GameObject pObject)
            {
                m_pRoot = pObject;
                m_pId = m_pRoot.transform.Find("Scroll View1/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pMapLevelShow = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show").transform;
                m_pLeft = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show/Left/Button").GetComponent<Button>();
                m_pRight = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show/Right/Button").GetComponent<Button>();
                m_pReturn = m_pRoot.transform.Find("Scroll View5/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pPlay = m_pRoot.transform.Find("Scroll View5/Viewport/Content/Item/Show/Text1").GetComponent<Text>();
                m_pTitle = m_pRoot.transform.Find("Scroll View0/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pMapScroll = new MapScroll(m_pRoot.transform.Find("Scroll View4").gameObject);
                m_pMaxScore = m_pRoot.transform.Find("Scroll View3/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pMapScrollRect = m_pRoot.transform.Find("Scroll View4").GetComponent<ScrollRect>();
                m_pLeftRightHint = m_pRoot.transform.Find("Scroll View4/Hint").GetComponent<CanvasGroup>();

                foreach (Transform Item in m_pMapLevelShow.GetComponentInChildren<Transform>())
                {
                    //if (Item.gameObject.name.Trim().Substring(0, Item.gameObject.name.Trim().Length - 1) == "Text")
                    if (Item.gameObject.name.Contains("Text"))
                    {
                        m_lMapLevel.Add(Item);
                    }
                }

                foreach (Transform Item in m_pMaxScore.transform.parent.Find("Stars").GetComponentInChildren<Transform>())
                {
                    if (Item.gameObject.name.Contains("Star"))
                    {
                        m_lStars.Add(Item);
                    }
                }

                m_pLeft.onClick.AddListener(delegate ()
                {
                    OnClickLeftAndRightMapLevelBtn(true, delegate (int nIndex, int nCount)
                    {
                        ShowMapLevel(nIndex, true);
                        CloseLeftOrRightBtn(nIndex, nCount);
                    });
                });
                m_pRight.onClick.AddListener(delegate ()
                {
                    OnClickLeftAndRightMapLevelBtn(false, delegate (int nIndex, int nCount)
                    {
                        ShowMapLevel(nIndex, false);
                        CloseLeftOrRightBtn(nIndex, nCount);
                    });
                });
                m_pMaxScore.GetComponent<Button>().onClick.AddListener(OnClickScore);

                m_pReturn.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    OnClickReturnBtn(2);
                });
                m_pPlay.GetComponent<Button>().onClick.AddListener(OnClickPlay);
                m_pLAScrollEvent = new LAScrollEvent();
                m_pLAScrollEvent.AddEvent(m_pMapScrollRect, OnDrag, EventTriggerType.Drag);
                m_pLAScrollEvent.AddEvent(m_pMapScrollRect, EndDrag, EventTriggerType.EndDrag);

                m_pMapScroll.OnClickRoomAction = OnClickRoomItem;

                RegisterAnim();
            }

            /// <summary>
            /// 设置启动状态。
            /// </summary>
            public void SetActive(bool b, bool bAnim = true)
            {
                tween.Kill(true);
                if (!bAnim)
                {
                    m_pRoot.SetActive(b);
                    return;
                }

                if (b)
                {
                    m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(500, 0).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                        m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);

                    });
                }
                else
                {
                    tween = m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(500, 0.5f).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                    });
                }
            }

            /// <summary>
            /// 更新UI。
            /// </summary>
            public void UpdateUI()
            {
                m_lMapLevel[0].GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Easy");
                m_lMapLevel[1].GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Medium");
                m_lMapLevel[2].GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Difficult");
                m_lMapLevel[3].GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Nightmare");
                m_lMapLevel[4].GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Hell");
                m_lMapLevel[5].GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Endless");
                m_pReturn.text = LAClient.g_Ins.GetValue("LoadingView_Return");
                // m_pPlay.text = LAClient.g_Ins.GetValue("LoadingView_Play");
                m_pTitle.text = LAClient.g_Ins.GetValue("LoadingView_Room");
                m_pMapScroll.UpdataUI();
            }

            /// <summary>
            /// 开始游戏。
            /// </summary>
            private void OnClickPlay()
            {
                OnClickPlayAction();
            }

            /// <summary>
            /// 单击返回项目。
            /// </summary>
            /// <param name="nIndex"></param>
            public void OnClickReturnBtn(int nIndex)
            {
                OnClickReturnBtnAction(nIndex);
            }

            /// <summary>
            /// 单击左右切换难度按钮。
            /// </summary>
            /// <param name="b"></param>
            private void OnClickLeftAndRightMapLevelBtn(bool b, Action<int, int> pBack)
            {
                if (m_pMapLevelAnim != null)
                {
                    if (!m_pMapLevelAnim.IsPlaying())
                    {
                        OnClickLeftAndRightMapLevelAction(b, pBack);
                    }
                }
                else
                {
                    OnClickLeftAndRightMapLevelAction(b, pBack);
                }
            }

            /// <summary>
            /// 单击房间项目。
            /// </summary>
            /// <param name="pRoom"></param>
            private void OnClickRoomItem(LAMapData.Room pRoom)
            {
                if (pRoom != null)
                {
                    if (pRoom != LAClient.g_Ins.GetCheckRoom())
                    {
                        if (m_pMapLevelAnim != null)
                        {
                            if (!m_pMapLevelAnim.IsPlaying())
                            {
                                OnClickRoomAction(pRoom);
                            }
                        }
                        else
                        {
                            OnClickRoomAction(pRoom);
                        }
                    }
                }
            }

            /// <summary>
            /// 单击积分。
            /// </summary>
            private void OnClickScore()
            {
                foreach (Transform Item in m_lStars)
                {
                    Item.gameObject.SetActive(true);
                    Vector2 V2 = new Vector2(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50));
                    Item.DOLocalMove(V2, 0.3f).OnComplete(delegate ()
                    {
                        Item.gameObject.SetActive(false);
                        Item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    });
                }
            }

            /// <summary>
            /// 滑动。
            /// </summary>
            /// <param name="eventData"></param>
            public void OnDrag(PointerEventData eventData)
            {
                if (LeftRightHintTime == 0)
                {
                    m_pLeftRightHint.DOFade(0, 0.5f);
                }
                LeftRightHintTime++;
                if (LeftRightHintTime == 50)
                {
                    LeftRightHintTime = 0;
                }
            }

            /// <summary>
            /// 结束滑动。
            /// </summary>
            /// <param name="eventData"></param>
            public void EndDrag(PointerEventData eventData)
            {
                m_pLeftRightHint.DOFade(1, 0.5f);
                LeftRightHintTime = 0;
            }

            /// <summary>
            /// 跟新地图卷轴数据。
            /// </summary>
            /// <param name="pMapData"></param>
            public void UpdataDataMapScroll(LAMapData pMapData)
            {
                m_pMapScroll.UpdataData(pMapData);
                OnClickRoomItem(pMapData.m_lRooms[0]);
            }

            /// <summary>
            /// 更新选中房间信息。
            /// </summary>
            public void UpdataCheckRoomInfo(LAMapData.Room pRoom)
            {
                if (pRoom != null)
                {
                    m_pId.text = pRoom.m_nId + "";
                    ShowMapLevel(pRoom.m_nCheckLv, true);
                    ShowScore(pRoom.m_nMaxScore);
                    CloseLeftOrRightBtn(pRoom.m_nCheckLv, pRoom.m_nMaxLv);
                }
            }

            /// <summary>
            /// 注册动画。
            /// </summary>
            private void RegisterAnim()
            {

            }

            /// <summary>
            /// 显示得分。
            /// </summary>
            /// <param name="nScore"></param>
            public void ShowScore(int nScore)
            {
                m_pMaxScore.text = "★" + nScore;
            }

            /// <summary>
            /// 是否可以玩。
            /// </summary>
            /// <param name="b"></param>
            public void ShowPlay(bool b)
            {
                if (b)
                {
                    m_pPlay.text = LAClient.g_Ins.GetValue("LoadingView_Play");
                    m_pPlay.DOColor(Color.black, 1f);
                    m_pPlay.GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        OnClickPlayAction();
                    });
                }
                else
                {
                    m_pPlay.text = LAClient.g_Ins.GetValue("LoadingView_Locked");
                    m_pPlay.DOColor(Color.gray, 1f);
                    m_pPlay.GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }

            /// <summary>
            /// 关闭左或右按钮。
            /// </summary>
            /// <param name="nIndex"></param>
            private void CloseLeftOrRightBtn(int nIndex, int nCount)
            {
                int RefIndex = nIndex;
                int Count = nCount + 1;
                if (1 == Count && RefIndex == 0)
                {
                    m_pLeft.gameObject.SetActive(false);
                    m_pRight.gameObject.SetActive(false);
                }
                else if (RefIndex == 0)
                {
                    m_pLeft.gameObject.SetActive(false);
                    if (Count > 1)
                    {
                        m_pRight.gameObject.SetActive(true);
                    }
                }
                else if (RefIndex + 1 == Count)
                {
                    m_pRight.gameObject.SetActive(false);
                    m_pLeft.gameObject.SetActive(true);
                }
                else
                {
                    m_pLeft.gameObject.SetActive(true);
                    m_pRight.gameObject.SetActive(true);
                }
            }

            /// <summary>
            /// 显示难度。
            /// </summary>
            private void ShowMapLevel(int nIndex, bool bDir)
            {
                for (int i = 0; i < m_lMapLevel.Count; i++)
                {
                    int index = i;
                    if (i == nIndex)
                    {
                        if (m_nOldCheckLevel != nIndex)
                        {
                            if (bDir)
                            {
                                m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0f).SetEase(Ease.InExpo).OnComplete(delegate ()
                                   {
                                       m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.5f);
                                   });
                                m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DOLocalMoveX(-60, 0).SetEase(Ease.InExpo).OnComplete(delegate ()
                                {
                                    m_lMapLevel[index].gameObject.SetActive(true);
                                    m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);
                                });
                            }
                            else
                            {
                                m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0f).SetEase(Ease.InExpo).OnComplete(delegate ()
                                {
                                    m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.5f);
                                });
                                m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DOLocalMoveX(60, 0).SetEase(Ease.InExpo).OnComplete(delegate ()
                                {
                                    m_lMapLevel[index].gameObject.SetActive(true);
                                    m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);
                                });
                            }
                        }
                        m_nOldCheckLevel = index;
                    }
                    else
                    {
                        if (bDir)
                        {
                            m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0f).SetEase(Ease.InExpo).OnComplete(delegate ()
                            {
                                m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0.5f);
                            });
                            m_pMapLevelAnim = m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DOLocalMoveX(60, 0.5f).OnComplete(delegate ()
                            {
                                m_lMapLevel[index].gameObject.SetActive(false);
                            });
                        }
                        else
                        {
                            m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0f).SetEase(Ease.InExpo).OnComplete(delegate ()
                            {
                                m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0.5f);
                            });
                            m_pMapLevelAnim = m_lMapLevel[index].gameObject.GetComponent<RectTransform>().DOLocalMoveX(-60, 0.5f).OnComplete(delegate ()
                            {
                                m_lMapLevel[index].gameObject.SetActive(false);
                            });
                        }
                    }
                }

            }

            /// <summary>
            /// 根节点。
            /// </summary>
            private GameObject m_pRoot;

            /// <summary>
            /// 标题。
            /// </summary>
            private Text m_pTitle;

            /// <summary>
            /// 关卡。
            /// </summary>
            private Text m_pId;

            /// <summary>
            /// 积分。
            /// </summary>
            private Text m_pMaxScore;

            /// <summary>
            /// 地图难度面板。
            /// </summary>
            private Transform m_pMapLevelShow;

            /// <summary>
            /// 所有难度。
            /// </summary>
            private List<Transform> m_lMapLevel = new List<Transform>();

            /// <summary>
            /// 小星星。
            /// </summary>
            private List<Transform> m_lStars = new List<Transform>();

            /// <summary>
            /// 按钮左。
            /// </summary>
            private Button m_pLeft;

            /// <summary>
            /// 按钮右。
            /// </summary>
            private Button m_pRight;

            /// <summary>
            /// 返回按钮。
            /// </summary>
            private Text m_pReturn;

            /// <summary>
            /// 开始游戏。
            /// </summary>
            private Text m_pPlay;

            /// <summary>
            /// 卷轴物体。
            /// </summary>
            private ScrollRect m_pMapScrollRect;

            /// <summary>
            /// 左右提示。
            /// </summary>
            private CanvasGroup m_pLeftRightHint;

            /// <summary>
            /// 左右提示秒表。
            /// </summary>
            private int LeftRightHintTime;

            /// <summary>
            /// 之前选中难度。
            /// </summary>
            private int m_nOldCheckLevel;

            /// <summary>
            /// 单击左右切换难度按钮,(返回选中索引，总数)。
            /// </summary>
            public Action<bool, Action<int, int>> OnClickLeftAndRightMapLevelAction;

            /// <summary>
            /// 开始游戏。
            /// </summary>
            public Action OnClickPlayAction;

            /// <summary>
            /// 单击返回。
            /// </summary>
            public Action<int> OnClickReturnBtnAction;

            /// <summary>
            /// 单击房间。
            /// </summary>
            public Action<LAMapData.Room> OnClickRoomAction;

            /// <summary>
            /// 动画。
            /// </summary>
            public Tween tween;

            /// <summary>
            /// 难度切换动画。
            /// </summary>
            private Tween m_pMapLevelAnim;

            /// <summary>
            /// 滑动事件监听器。
            /// </summary>
            private LAScrollEvent m_pLAScrollEvent;

            /// <summary>
            /// 地图卷轴。
            /// </summary>
            private MapScroll m_pMapScroll;

            /// <summary>
            /// 地图卷轴。
            /// </summary>
            public class MapScroll
            {
                public MapScroll(GameObject pObject)
                {
                    m_pRoot = pObject;
                    m_pPrefab = m_pRoot.transform.Find("Viewport/Content/Item").gameObject;

                    m_pPrefab.SetActive(false);
                }

                /// <summary>
                /// 更新UI。
                /// </summary>
                public void UpdataUI()
                {
                    foreach (MapItem Item in m_lMapItems)
                    {
                        Item.UpdataUI();
                    }
                }

                /// <summary>
                /// 更新数据。
                /// </summary>
                public void UpdataData(LAMapData pMapData)
                {
                    if (m_lMapItems.Count == 0)
                    {
                        for (int i = 0; i < pMapData.m_lRooms.Count; i++)
                        {
                            MapItem Item = new MapItem(m_pPrefab);
                            Item.FillData(pMapData.m_lRooms[i]);
                            Item.OnClickMapItemRoomAction = OnClickRoomItem;
                            m_lMapItems.Add(Item);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pMapData.m_lRooms.Count; i++)
                        {
                            m_lMapItems[i].FillData(pMapData.m_lRooms[i]);
                        }
                    }
                }

                /// <summary>
                /// 单击房间项目。
                /// </summary>
                /// <param name="pRoom"></param>
                private void OnClickRoomItem(LAMapData.Room pRoom)
                {
                    OnClickRoomAction(pRoom);
                }

                /// <summary>
                /// 根物体。
                /// </summary>
                private GameObject m_pRoot;

                /// <summary>
                /// 预制体。
                /// </summary>
                private GameObject m_pPrefab;

                /// <summary>
                /// 单击房间。
                /// </summary>
                public Action<LAMapData.Room> OnClickRoomAction;

                /// <summary>
                /// 所有关卡。
                /// </summary>
                private List<MapItem> m_lMapItems = new List<MapItem>();

                /// <summary>
                /// 地图项。
                /// </summary>
                public class MapItem
                {
                    public MapItem(GameObject pObject)
                    {
                        m_pRoot = GameObject.Instantiate(pObject);
                        m_pRoot.transform.SetParent(pObject.transform.parent, false);
                        m_pItems.Add(this);
                        int Index = m_pItems.Count;
                        m_pRoot.name = pObject.name + Index;
                        m_pImage = m_pRoot.transform.Find("Show/Image").GetComponent<Image>();
                        m_pName = m_pRoot.transform.Find("Show/Text").GetComponent<Text>();
                        m_pLock = m_pRoot.transform.Find("Show/Lock").gameObject;

                        m_pRoot.SetActive(true);
                    }

                    /// <summary>
                    /// 更新UI。
                    /// </summary>
                    public void UpdataUI()
                    {
                        if (m_pName.text.Contains("Room"))
                        {
                            m_pName.text = m_pName.text.Replace("Room", LAClient.g_Ins.GetValue("LoadingView_Checkpoint"));
                        }
                        else
                        {
                            m_pName.text = m_pName.text.Replace("关卡", LAClient.g_Ins.GetValue("LoadingView_Checkpoint"));
                        }
                    }

                    /// <summary>
                    /// 单击房间项目。
                    /// </summary>
                    /// <param name="pRoom"></param>
                    private void OnClickRoomItem(LAMapData.Room pRoom)
                    {
                        OnClickMapItemRoomAction(pRoom);
                    }

                    /// <summary>
                    /// 填充数据。
                    /// </summary>
                    public void FillData(LAMapData.Room pRoomData)
                    {
                        m_pName.gameObject.SetActive(pRoomData.m_bOpenLock);
                        m_pLock.SetActive(!pRoomData.m_bOpenLock);
                        m_pImage.sprite = null;
                        m_pName.text = pRoomData.m_pName;
                        LAMapData.Room Room = pRoomData;
                        m_pName.GetComponent<Button>().onClick.RemoveAllListeners();
                        m_pLock.GetComponent<Button>().onClick.RemoveAllListeners();
                        m_pName.GetComponent<Button>().onClick.AddListener(delegate ()
                        {
                            OnClickRoomItem(Room);
                        });
                        m_pLock.GetComponent<Button>().onClick.AddListener(delegate ()
                        {
                            OnClickRoomItem(Room);
                        });
                    }

                    /// <summary>
                    /// 根节点。
                    /// </summary>
                    private GameObject m_pRoot;

                    /// <summary>
                    /// 图片。
                    /// </summary>
                    private Image m_pImage;

                    /// <summary>
                    /// 地图名。
                    /// </summary>
                    private Text m_pName;

                    /// <summary>
                    /// 锁。
                    /// </summary>
                    private GameObject m_pLock;

                    /// <summary>
                    /// 所有项。
                    /// </summary>
                    private static List<MapItem> m_pItems = new List<MapItem>();

                    /// <summary>
                    /// 单击房间。
                    /// </summary>
                    public Action<LAMapData.Room> OnClickMapItemRoomAction;

                }

            }
        }

        /// <summary>
        /// 关于面板。
        /// </summary>
        private class AboutShow
        {
            public AboutShow(GameObject pObject)
            {
                m_pRoot = pObject;
                m_pReturn = m_pRoot.transform.Find("Scroll View2/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                m_pTitle = m_pRoot.transform.Find("Scroll View0/Viewport/Content/Item/Show/Text").GetComponent<Text>();
                foreach (Transform Item in m_pRoot.transform.Find("Scroll View1/Viewport/Content").GetComponentInChildren<Transform>())
                {
                    m_lAbouts.Add(Item);
                }

                m_pReturn.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                OnClickReturnBtn(3);
            });
            }

            /// <summary>
            /// 设置启动状态。
            /// </summary>
            public void SetActive(bool b, bool bAnim = true)
            {
                tween.Kill(true);
                if (!bAnim)
                {
                    m_pRoot.SetActive(b);
                    return;
                }

                if (b)
                {
                    m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(500, 0).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                        m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);

                    });
                }
                else
                {
                    tween = m_pRoot.GetComponent<RectTransform>().DOLocalMoveX(500, 0.5f).OnComplete(delegate ()
                    {
                        m_pRoot.SetActive(b);
                    });
                }
            }

            /// <summary>
            /// 更新UI。
            /// </summary>
            public void UpdateUI()
            {
                m_lAbouts[0].Find("Show/Text").GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Studio");
                m_lAbouts[1].Find("Show/Text").GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_GamePlayProgrammer");
                m_lAbouts[2].Find("Show/Text").GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_UI");
                m_lAbouts[3].Find("Show/Text").GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Planner");
                m_lAbouts[4].Find("Show/Text").GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Artist");
                m_lAbouts[5].Find("Show/Text").GetComponent<Text>().text = LAClient.g_Ins.GetValue("LoadingView_Versions");
                m_pReturn.text = LAClient.g_Ins.GetValue("LoadingView_Return");
                m_pTitle.text = LAClient.g_Ins.GetValue("LoadingView_About");
            }

            /// <summary>
            /// 单击返回项目。
            /// </summary>
            /// <param name="nIndex"></param>
            public void OnClickReturnBtn(int nIndex)
            {
                OnClickReturnBtnAction(nIndex);
            }

            /// <summary>
            /// 标题。
            /// </summary>
            private Text m_pTitle;

            /// <summary>
            /// 所有关于信息。
            /// </summary>
            private List<Transform> m_lAbouts = new List<Transform>();

            /// <summary>
            /// 返回文本。
            /// </summary>
            private Text m_pReturn;

            /// <summary>
            /// 根节点。
            /// </summary>
            private GameObject m_pRoot;

            /// <summary>
            /// 单击返回。
            /// </summary>
            public Action<int> OnClickReturnBtnAction;

            private Tween tween;
        }

    }
}


