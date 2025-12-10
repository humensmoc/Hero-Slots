using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class RelicView : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Relic relic;
    public Image relicImage;
    public HoverInfoPanelData hoverInfoPanelData;
    public void Init(Relic relic){
        this.relic = relic;
        relicImage.sprite = ResourcesLoader.LoadRelicSprite(relic.relicData.RelicType.ToString());
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Relic, relicImage.sprite, relic.relicData.RelicType.ToString(), relic.relicData.Description);
    }

    public void OnPointerEnter(PointerEventData eventData){
        transform.DOScale(1.1f, 0.15f);
        HoverInfoController.Instance.MouseEnterTargetView(hoverInfoPanelData);
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.DOScale(1f, 0.15f);
        HoverInfoController.Instance.MouseExitTargetView(hoverInfoPanelData);
    }
}