using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class RelicSelectItemView : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public RelicData relicData;
    public Image relicImage;
    public void Init(RelicData relicData){
        this.relicData = relicData;
        relicImage.sprite = ResourcesLoader.LoadRelicSprite(relicData.RelicType.ToString());
    }

    public void OnPointerClick(PointerEventData eventData){
        RelicSelectSystem.Instance.SelectRelic(this);
    }

    public void OnPointerEnter(PointerEventData eventData){
        transform.DOScale(1.1f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.DOScale(1f, 0.15f);
    }
}
