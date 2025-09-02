using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RunTimeDataSystem : Singleton<RunTimeDataSystem>
{
    public RuntimeData runtimeData{get;private set;}

    void Init(){
        runtimeData = new RuntimeData();
    }

    public void NextTurn(){
        runtimeData.currentTurn++;
    }
}
