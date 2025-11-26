using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsController : Singleton<TipsController>
{
    public Transform tipsParent;

    public void ShowTips(string tips)
    {
        GameObject tipsPrefab = Resources.Load<GameObject>("Prefabs/UI/Tips");
        GameObject tipsInstance = Instantiate(tipsPrefab, tipsParent);
        tipsInstance.GetComponent<Tips>().Init(tips);
    }
}
