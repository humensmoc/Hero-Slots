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
    public TMP_Text coinText;
    public TMP_Text waveText;
    public TMP_Text stageText;

    void Update(){
        if(!RuntimeEffectData.isAlreadyInit) return;
        
        bloodGemValueText.text ="Blood Gem: " + RuntimeEffectData.bloodGemValue.ToString();
        electricityText.text ="Electricity: " + RuntimeEffectData.electricity.ToString();
        turnText.text="Turn : "+TurnSystem.Instance.currentTurn.ToString();
        healthText.text="Health: " + Model.currentHealth.ToString() + "/" + Model.maxHealth.ToString();
        coinText.text="Coin: " + RuntimeEffectData.coin.ToString();
        waveText.text="Wave: "+(EnemySystem.Instance.currentWaveIndex+1)+"/"+EnemySystem.Instance.currentLevelData.enemyStageDatas[EnemySystem.Instance.currentStageIndex].enemyWaveDatas.Count;
        stageText.text="Stage: "+(EnemySystem.Instance.currentStageIndex+1)+"/"+EnemySystem.Instance.currentLevelData.enemyStageDatas.Count;
    }

    public void Reset(){
        RuntimeEffectData.Reset();
    }
}