using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicView : MonoBehaviour
{
    public Relic relic;
    public Image relicImage;
    public void Init(Relic relic){
        this.relic = relic;
        relicImage.sprite = ResourcesLoader.LoadRelicSprite(relic.relicData.RelicType.ToString());
    }
}