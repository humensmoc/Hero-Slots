using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInfoController : Singleton<HoverInfoController>
{
    [SerializeField] private GameObject hoverInfoPanelPrefab;
    [SerializeField] private Canvas canvas;
    private HoverInfoPanel currentHoverInfoPanel;
    private List<HoverInfoPanel> panels = new List<HoverInfoPanel>();
    private bool isHoverTargetView = false;

    void Update(){
        UpdateHoverPanelPosition();
    }

    void UpdateHoverPanelPosition(){
        Debug.Log("UpdateHoverPanelPosition");

        if(currentHoverInfoPanel==null)return;

        if(currentHoverInfoPanel.isFocus)return;

        bool isInLeftArea = MouseUtil.GetMousePostionInScreenSpace().x<Screen.width/2;
        bool isInBottomArea = MouseUtil.GetMousePostionInScreenSpace().y<Screen.height/2;

        currentHoverInfoPanel.rectTransform.pivot = new Vector2(isInLeftArea?0:1,isInBottomArea?0:1);

        Vector2 pos = MouseUtil.GetMousePostionInScreenSpace()-new Vector2(Screen.width/2,Screen.height/2);
        Vector2 offset = new Vector2(isInLeftArea?-10:10,isInBottomArea?-10:10);
        pos += offset;
        currentHoverInfoPanel.rectTransform.anchoredPosition = pos;

    }

    public void CreateHoverInfoPanel(HoverInfoPanelData hoverInfoPanelData){
        Debug.Log("CreateHoverInfoPanel");

        HoverInfoPanel hoverInfoPanel = Instantiate(hoverInfoPanelPrefab,canvas.transform).GetComponent<HoverInfoPanel>();
        panels.Add(hoverInfoPanel);
        currentHoverInfoPanel = hoverInfoPanel;
        currentHoverInfoPanel.Init(hoverInfoPanelData);
    }

    public void DestroyHoverInfoPanel(HoverInfoPanel panel){
        Debug.Log("DestroyHoverInfoPanel");

        panels.Remove(panel);
        Destroy(panel.gameObject);
        if(panels.Count>0){
            currentHoverInfoPanel = panels[panels.Count-1];
        }else{
            currentHoverInfoPanel = null;
        }
    }

    public void MouseEnterTargetView(HoverInfoPanelData hoverInfoPanelData){
        Debug.Log("MouseEnterTargetView");

        if(!isHoverTargetView||currentHoverInfoPanel==null){
            CreateHoverInfoPanel(hoverInfoPanelData);
            isHoverTargetView = true;
        }
    }

    public void MouseExitTargetView(HoverInfoPanelData hoverInfoPanelData){
        Debug.Log("MouseExitTargetView");

        if(currentHoverInfoPanel==null)return;

        bool isFocus = false;
        foreach(HoverInfoPanel panel in panels){
            if(panel.isFocus){
                isFocus = true;
                break;
            }
        }
        if(isFocus)return;

        DestroyHoverInfoPanel(currentHoverInfoPanel);
        isHoverTargetView = false;
    }

    public void MouseEnterKeyWord(string keyWord,HoverInfoPanel panel){
        if(panel!=currentHoverInfoPanel||!panel.isFocus)return;

        HoverInfoPanelData hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Keyword,null,keyWord,"hero_Alpha description <link=keyword><b>keyword</b></link>");
        CreateHoverInfoPanel(hoverInfoPanelData);
    }

    public void MouseExitKeyWord(HoverInfoPanel panel){
        if(currentHoverInfoPanel.isFocus)return;
        
        if(panel==panels[panels.Count-2]){
            DestroyHoverInfoPanel(currentHoverInfoPanel);
        }
    }

    public void MouseExitHoverPanel(HoverInfoPanel panel){
        Debug.Log("MouseExitHoverPanel");

        if(panel!=currentHoverInfoPanel||!panel.isFocus)return;
        DestroyHoverInfoPanel(panel);

        if(panels.Count>0){
            for(int i=panels.Count-1;i>=0;i--){
                Vector3 mousePosition = Input.mousePosition;
                RectTransform rectTransform = panels[i].rectTransform;
                // 将鼠标屏幕坐标转换为UI坐标
                bool isMouseOver = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition, null);
                if (!isMouseOver)
                {
                    DestroyHoverInfoPanel(panels[i]);
                }
            }
        }
    }

    public void FocusPanel(HoverInfoPanel panel){
        Debug.Log("FocusPanel");
    }
}
