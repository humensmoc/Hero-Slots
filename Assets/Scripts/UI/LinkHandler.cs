using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(TMP_Text))]
public class LinkHandler : MonoBehaviour
{
    public static event Action<string> OnLinkClickedEvent;
    public static event Action<string> OnLinkHoveredEvent;
    public static event Action OnLinkExitEvent;
    
    private TMP_Text _tmpTextBox;
    private Canvas _canvasToCheck;
    [SerializeField] private Camera cameraToUse;
    private int currentHoveredLinkIndex = -1;
    
    void Awake()
    {
        _tmpTextBox = GetComponent<TMP_Text>();
        _canvasToCheck = GetComponentInParent<Canvas>();

        if(_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay){
            cameraToUse = null;
        }

        if(cameraToUse == null){
            cameraToUse = Camera.main;
        }
    }
    
    void Start()
    {
        // 强制更新文本以确保linkInfo被正确生成
        _tmpTextBox.ForceMeshUpdate();
    }
    
    void Update()
    {
        // 持续检测鼠标悬停
        CheckMouseOverLink();
    }
    
    private void CheckMouseOverLink()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        // 使用改进的链接检测方法
        var linkTaggedText = FindLinkAtMousePosition(mousePosition);
        
        if(linkTaggedText != -1 && linkTaggedText != currentHoveredLinkIndex){
            currentHoveredLinkIndex = linkTaggedText;
            TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];
            string linkText = linkInfo.GetLinkText();
            Debug.Log("🔍 悬停到链接: " + linkText);
            OnLinkHoveredEvent?.Invoke(linkText);
        }
        else if(linkTaggedText == -1 && currentHoveredLinkIndex != -1){
            Debug.Log("👋 离开链接");
            currentHoveredLinkIndex = -1;
            OnLinkExitEvent?.Invoke();
        }
    }
    
    private int FindLinkAtMousePosition(Vector3 mousePosition)
    {
        // ScreenSpaceOverlay模式下使用null摄像机参数
        if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, null);
        }
        
        // 其他模式使用原始方法
        return TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, cameraToUse);
    }
}
