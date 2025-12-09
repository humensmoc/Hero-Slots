using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RelicSystem : Singleton<RelicSystem>
{
    public List<Relic> relics = new List<Relic>();
    public List<RelicView> relicViews = new List<RelicView>();
    public GameObject relicViewPrefab;
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
        relics.Add(relic);
        RelicView relicView = Instantiate(relicViewPrefab,relicParent).GetComponent<RelicView>();
        relicViews.Add(relicView);
        relicView.Init(relic);
    }

    public void RemoveRelic(RelicView relicView){
        relics.Remove(relicView.relic);
        relicViews.Remove(relicView);
        Destroy(relicView.gameObject);
    }

    public void Reset()
    {
        if(relicViews.Count == 0) return;
        if(relics.Count == 0) return;

        foreach(RelicView relicView in relicViews){
            RemoveRelic(relicView);
        }
        relics.Clear();
        relicViews.Clear();

        Init();
    }
}
