using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RelicSelectSystem : Singleton<RelicSelectSystem>
{
    public RelicSelectPanelView relicSelectPanelView;
    public GameObject relicSelectPanel;
    public GameObject relicSelectItemPrefab;
    public Button refreshButton;
    public List<RelicData> relicDatas=new();
    public int optionCount;
    
    public void Init(){
        relicSelectPanelView.Init();
        refreshButton.onClick.AddListener(ManualRefresh);
    }

    public void Refresh(){
        Debug.Log("relic select system refresh");
        relicSelectPanelView.RemoveAllRelicSelectItem();
        relicDatas.Clear();
        RelicLibrary.relicDatas.ForEach(relicData => relicDatas.Add(relicData.Clone()));

        for(int i=0;i<optionCount;i++){
            if(relicDatas.Count == 0) break; // 防止从空列表抽取
            
            RelicData relicData=relicDatas.Draw();
            relicDatas.Add(relicData);
            relicSelectPanelView.AddRelicSelectItem(relicData);
        }
    }

    public void SelectRelic(RelicSelectItemView relicSelectItemView){
        RelicSystem.Instance.AddRelic(relicSelectItemView.relicData.RelicType);
        relicSelectPanelView.RemoveRelicSelectItem(relicSelectItemView);
    }

    public void ManualRefresh(){
        Refresh();
    }
}