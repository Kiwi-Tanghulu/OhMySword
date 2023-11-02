using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyUI
{
    public enum UIType
    {
        Chase = 0,
        Fixed,
        Panel,
        PopUp,
        Full,
        
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public Transform ScreenCanvas { get; private set; }
        public Transform WorldCanvas { get; private set; }

        [SerializeField]
        private List<BaseUI> uiList = new();
        [SerializeField]
        private int poolingUICount = 5;

        private Dictionary<string, BaseUI> uiContainer = new();
        private Dictionary<string, List<BaseUI>> poolingUIContainer = new();
        private Stack<PanelUI> panelUIHistory = new();
        private Stack<FullUI> fullUIHistory = new();
        private FullUI currentFullUI = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            ScreenCanvas = transform.Find("ScreenCanvas");
            WorldCanvas = transform.Find("WorldCanvas");

            CreateUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                GetUI<NoticeUI>("NoticeUI").Show(2f, "Test Notice Test Notice Test Notice Test Notice");

            //if (fullUIHistory.Count <= 0)
            //{
            //    if (Input.GetKeyDown(KeyCode.B))
            //        GetUI<PanelUI>(UIType.Panel, "TestPanelUI 1").Show(Vector2.zero);
            //    if (Input.GetKeyDown(KeyCode.N))
            //        GetUI<PanelUI>(UIType.Panel, "TestPanelUI 2").Show(Vector2.zero);
            //    if (Input.GetKeyDown(KeyCode.M))
            //        GetUI<PanelUI>(UIType.Panel, "TestPanelUI 3").Show(Vector2.zero);
            //}
        }

        public T GetUI<T>(string name) where T : BaseUI
        {
            T ui = null;

            if (uiContainer.ContainsKey(name))
            {
                ui = uiContainer[name] as T;
            }
            else if (poolingUIContainer.ContainsKey(name))
            {
                ui = poolingUIContainer[name].Find(x => x.IsOpen == false) as T;

                if(ui == null)
                    ui = CreateUI(uiList.Find(x => x.name == name)) as T;
            }
            else
                Debug.Log("없는 UI");

            return ui;
        }

        //패널, 전체ui 관리용
        public void RecordHistory(BaseUI ui)
        {
            if (ui.uiType == UIType.Panel)
                panelUIHistory.Push(ui as PanelUI);
            if (ui.uiType == UIType.Full)
            {
                fullUIHistory.Push(ui as FullUI);

                if (currentFullUI != null)
                    currentFullUI.Hide();
                currentFullUI = ui as FullUI;
            }
        }
        //전체ui 관리용
        public void ShowLastUI()
        {
            // 이전 ui가 있다면 그거 꺼내고 킴
            if (fullUIHistory.Count > 0)
            {
                fullUIHistory.Pop().Hide();

                if (fullUIHistory.Count > 0)
                {
                    currentFullUI = fullUIHistory.Peek();
                    currentFullUI.Show();
                }
                else
                {
                    currentFullUI = null;
                }
            }
        }

        private void EscapeProcess()
        {
            if (panelUIHistory.Count > 0)
            {
                panelUIHistory.Pop().Hide();
            }
            else
            {
                if (fullUIHistory.Count > 0)
                {
                    ShowLastUI();
                }
                else
                {
                    GetUI<FullUI>("PauseUI").ShowAndRecord();
                }
            }
        }

        private void CreateUI()
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (uiList[i].isPooling)
                {
                    poolingUIContainer.Add(uiList[i].name, new());

                    for (int j = 0; j < poolingUICount; j++)
                        CreateUI(uiList[i]);
                }
                else
                {
                    CreateUI(uiList[i]);
                }
            }
        }
        private BaseUI CreateUI(BaseUI _ui)
        {
            BaseUI ui = Instantiate(_ui);

            if (ui.isWorld)
                ui.transform.SetParent(WorldCanvas);
            else
                ui.transform.SetParent(ScreenCanvas);

            ui.transform.localPosition = Vector3.zero;
            ui.name = ui.name.Replace("(Clone)", "");

            if(ui.isPooling)
                poolingUIContainer[ui.name].Add(ui);
            else
                uiContainer.Add(ui.name, ui);

            return ui;
        }
    }
}


