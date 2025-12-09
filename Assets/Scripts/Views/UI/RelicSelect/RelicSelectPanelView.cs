using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RelicSelectPanelView : MonoBehaviour
{
    public List<RelicSelectItemView> relicSelectItemViews;
    public Transform relicSelectItemParent;
    public GameObject relicSelectItemPrefab;
    public void Init(){
        relicSelectItemViews.Clear();

    }
    
    public void AddRelicSelectItem(RelicData relicData){
        RelicSelectItemView relicSelectItemView = Instantiate(relicSelectItemPrefab,relicSelectItemParent).GetComponent<RelicSelectItemView>();
        relicSelectItemView.Init(relicData);
        relicSelectItemViews.Add(relicSelectItemView);
    }

    public void RemoveRelicSelectItem(RelicSelectItemView relicSelectItemView){
        relicSelectItemViews.Remove(relicSelectItemView);
        Destroy(relicSelectItemView.gameObject);
    }

    public void RemoveAllRelicSelectItem(){
        foreach(RelicSelectItemView relicSelectItemView in relicSelectItemViews){
            Destroy(relicSelectItemView.gameObject);
        }
        relicSelectItemViews.Clear();
    }
    

}