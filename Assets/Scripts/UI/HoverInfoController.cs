using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInfoController : Singleton<HoverInfoController>
{
    public HoverInfoPanel hoverInfoPanel;
    public GameObject hoverTarget;
    private Camera mainCamera;
    private Canvas canvas;
    private GameObject currentHoverTarget;

    void Start()
    {
        // 获取主摄像机和Canvas引用
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
        }
        
        canvas = hoverInfoPanel.GetComponentInParent<Canvas>();
    }

    void Update(){
        if(hoverInfoPanel.gameObject.activeSelf && hoverTarget != null){
            UpdateHoverPanelPosition();
        }
    }

    private void UpdateHoverPanelPosition()
    {
        if (mainCamera == null || canvas == null) return;

        // 将3D世界坐标转换为屏幕坐标
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(hoverTarget.transform.position);
        
        // 获取目标对象的高度
        float targetHeight = GetTargetHeight(hoverTarget);
        float targetHeightInScreenSpace = GetScreenSpaceHeight(targetHeight);
        
        // 获取面板的RectTransform
        RectTransform panelRectTransform = hoverInfoPanel.GetComponent<RectTransform>();
        float panelHeight = panelRectTransform.rect.height;
        
        // 根据鼠标在屏幕的位置决定面板显示在上方还是下方
        Vector3 finalScreenPoint = screenPoint;
        if(Input.mousePosition.y > Screen.height / 2)
        {
            // 鼠标在屏幕上半部分，面板显示在目标下方
            finalScreenPoint.y -= (targetHeightInScreenSpace / 2 + panelHeight / 2);
        }
        else
        {
            // 鼠标在屏幕下半部分，面板显示在目标上方
            finalScreenPoint.y += (targetHeightInScreenSpace / 2 + panelHeight / 2);
        }
        
        // 将屏幕坐标转换为Canvas局部坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            finalScreenPoint, 
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, 
            out localPoint
        );
        
        // 设置面板位置
        panelRectTransform.localPosition = localPoint;
    }

    // 获取目标对象的高度，支持RectTransform和普通Transform
    private float GetTargetHeight(GameObject target)
    {
        // 首先尝试获取RectTransform（UI元素）
        RectTransform rectTransform = target.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            return rectTransform.rect.height;
        }
        
        // 如果没有RectTransform，尝试获取Renderer的bounds（3D/2D对象）
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size.y;
        }
        
        // 如果都没有，返回默认值
        return 1.0f;
    }

    // 将世界空间的高度转换为屏幕空间的高度
    private float GetScreenSpaceHeight(float worldHeight)
    {
        if (mainCamera == null) return worldHeight;
        
        // 获取目标位置的世界坐标
        Vector3 worldPos = hoverTarget.transform.position;
        
        // 计算目标上边缘和下边缘的世界坐标
        Vector3 topPoint = worldPos + Vector3.up * (worldHeight / 2);
        Vector3 bottomPoint = worldPos - Vector3.up * (worldHeight / 2);
        
        // 转换为屏幕坐标
        Vector3 topScreen = mainCamera.WorldToScreenPoint(topPoint);
        Vector3 bottomScreen = mainCamera.WorldToScreenPoint(bottomPoint);
        
        // 返回屏幕空间中的高度差
        return Mathf.Abs(topScreen.y - bottomScreen.y);
    }

    public void ShowHoverInfoPanel(HoverInfoPanelData hoverInfoPanelData, GameObject hoverTarget){
        this.hoverTarget = hoverTarget;
        hoverInfoPanel.gameObject.SetActive(true);
        hoverInfoPanel.ShowHoverInfoPanel(hoverInfoPanelData);
    }

    public void HideHoverInfoPanel(){
        hoverInfoPanel.gameObject.SetActive(false);
        hoverTarget = null;
    }

    public void MouseEnterTarget(HoverInfoPanelData hoverInfoPanelData, GameObject hoverTarget){
        ShowHoverInfoPanel(hoverInfoPanelData, hoverTarget);
    }
}
