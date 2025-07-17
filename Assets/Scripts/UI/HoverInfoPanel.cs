using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public enum HoverInfoPanelType{
    Card,Hero,Perk,Enemy
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

public class HoverInfoPanel : MonoBehaviour
{
    public HoverInfoPanelType hoverInfoPanelType;
    public Image icon;
    public TMP_Text infoName;
    public TMP_Text infoDescription;

    public void ShowHoverInfoPanel(HoverInfoPanelData hoverInfoPanelData){

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
        }
    }
    
}
