using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RelicSystem : Singleton<RelicSystem>
{
    GameObject _relicViewPrefab;
    public Transform relicParent;
    public void Init()
    {
        AddRelic(RelicType.A);
        AddRelic(RelicType.B);
        AddRelic(RelicType.C);
        AddRelic(RelicType.D);
        AddRelic(RelicType.E);
    }

    public void AddRelic(RelicType relicType){
        RelicData relicData = RelicLibrary.GetRelicData(relicType);
        Relic relic = new Relic(relicData);
        Model.Relics.Add(relic);
        if(_relicViewPrefab==null){
            _relicViewPrefab = ResourcesLoader.LoadUIPrefab("RelicItem");
        };
        RelicView relicView = Instantiate(_relicViewPrefab,relicParent).GetComponent<RelicView>();
        Model.RelicViews.Add(relicView);
        relicView.Init(relic);
    }

    public void RemoveRelic(RelicView relicView){
        Model.Relics.Remove(relicView.relic);
        Model.RelicViews.Remove(relicView);
        Destroy(relicView.gameObject);
    }

    public void Reset()
    {
        if(Model.RelicViews.Count == 0) return;
        if(Model.Relics.Count == 0) return;

        foreach(RelicView relicView in Model.RelicViews){
            RemoveRelic(relicView);
        }
        Model.Relics.Clear();
        Model.RelicViews.Clear();

        Init();
    }
}
