using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeEffectDataView : MonoBehaviour
{
    public TMP_Text bloodGemValueText;
    public TMP_Text electricityText;
    public TMP_Text turnText;

    void Update(){
        bloodGemValueText.text ="Blood Gem: " + RuntimeEffectData.bloodGemValue.ToString();
        electricityText.text ="Electricity: " + RuntimeEffectData.electricity.ToString();
        turnText.text="Turn : "+TurnSystem.Instance.currentTurn.ToString();
    }

    public void Reset(){
        RuntimeEffectData.Reset();
    }
}