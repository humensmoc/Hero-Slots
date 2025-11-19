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
    public TMP_Text healthText;

    void Update(){
        bloodGemValueText.text ="Blood Gem: " + RuntimeEffectData.bloodGemValue.ToString();
        electricityText.text ="Electricity: " + RuntimeEffectData.electricity.ToString();
        turnText.text="Turn : "+TurnSystem.Instance.currentTurn.ToString();
        healthText.text="Health: " + Model.currentHealth.ToString() + "/" + Model.maxHealth.ToString();
    }

    public void Reset(){
        RuntimeEffectData.Reset();
    }
}