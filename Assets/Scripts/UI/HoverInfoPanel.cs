using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum HoverInfoPanelType{
    Card,Hero,Perk,Enemy,Keyword
}

public class HoverInfoPanelData{
    public HoverInfoPanelType hoverInfoPanelType;
    public Sprite skillIcon;
    public string skillName;
    public string skillDescription;

    public HoverInfoPanelData(HoverInfoPanelType hoverInfoPanelType, Sprite skillIcon, string skillName, string skillDescription){
        this.hoverInfoPanelType = hoverInfoPanelType;
        this.skillIcon = skillIcon;
        this.skillName = skillName;
        this.skillDescription = skillDescription;
    }
}

[RequireComponent(typeof(RectTransform))]
public class HoverInfoPanel : MonoBehaviour,IPointerExitHandler
{
    [Header("Panel")]
    public HoverInfoPanelType hoverInfoPanelType;
    public Image icon;
    public TMP_Text infoName;
    public TMP_Text infoDescription;
    public RectTransform rectTransform;

    [Header("Focus")]
    public Image focusCDImage;
    public float focusCD;
    private float focusCDTimer;
    public bool isFocus = false;

    [Header("Keyword Link")]
    private Canvas _canvasToCheck;
    [SerializeField] private Camera cameraToUse;
    private int currentHoveredLinkIndex = -1;

    void OnEnable(){
        rectTransform = GetComponent<RectTransform>();

        _canvasToCheck = GetComponentInParent<Canvas>();

        if(_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay){
            cameraToUse = null;
        }

        if(cameraToUse == null){
            cameraToUse = Camera.main;
        }

        infoDescription.ForceMeshUpdate();
        
    }

    void Update(){
        if(focusCDTimer<focusCD){
            focusCDTimer += Time.deltaTime;
            focusCDImage.fillAmount = focusCDTimer/focusCD;
        }else{
            if(!isFocus){
                isFocus=true;
                HoverInfoController.Instance.FocusPanel(this);
            }
        }

        CheckMouseOverLink();
    }

    public void Init(HoverInfoPanelData hoverInfoPanelData){
        switch(hoverInfoPanelData.hoverInfoPanelType){
            case HoverInfoPanelType.Card:
                icon.sprite = hoverInfoPanelData.skillIcon;
                infoName.text = hoverInfoPanelData.skillName;
                infoDescription.text = hoverInfoPanelData.skillDescription;
                break;
            case HoverInfoPanelType.Hero:
                icon.sprite = hoverInfoPanelData.skillIcon;
                infoName.text = hoverInfoPanelData.skillName;
                infoDescription.text = hoverInfoPanelData.skillDescription;
                break;
            case HoverInfoPanelType.Keyword:
                icon.sprite = hoverInfoPanelData.skillIcon;
                infoName.text = hoverInfoPanelData.skillName;
                infoDescription.text = hoverInfoPanelData.skillDescription;
                break;
        }

        focusCDTimer = 0;
    }

    private void CheckMouseOverLink()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        // 使用改进的链接检测方法
        var linkTaggedText = FindLinkAtMousePosition(mousePosition);
        
        if(linkTaggedText != -1 && linkTaggedText != currentHoveredLinkIndex){
            currentHoveredLinkIndex = linkTaggedText;
            TMP_LinkInfo linkInfo = infoDescription.textInfo.linkInfo[linkTaggedText];
            string linkText = linkInfo.GetLinkText();
            Debug.Log("🔍 悬停到链接: " + linkText);

            EnterLink(linkText);

        }
        else if(linkTaggedText == -1 && currentHoveredLinkIndex != -1){
            Debug.Log("👋 离开链接");
            currentHoveredLinkIndex = -1;

            ExitLink();
        }
    }

    private int FindLinkAtMousePosition(Vector3 mousePosition)
    {
        // ScreenSpaceOverlay模式下使用null摄像机参数
        if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return TMP_TextUtilities.FindIntersectingLink(infoDescription, mousePosition, null);
        }
        
        // 其他模式使用原始方法
        return TMP_TextUtilities.FindIntersectingLink(infoDescription, mousePosition, cameraToUse);
    }

    void EnterLink(string linkText){
        HoverInfoController.Instance.MouseEnterKeyWord(linkText,this);
    }

    void ExitLink(){
        HoverInfoController.Instance.MouseExitKeyWord(this);
    }
    public void OnPointerExit(PointerEventData eventData){
        HoverInfoController.Instance.MouseExitHoverPanel(this);
    }
    
}
