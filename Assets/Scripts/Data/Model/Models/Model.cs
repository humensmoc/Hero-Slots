
public static class Model
{
    public static RuntimeData runtimeData=new RuntimeData();

    public static int MaxHealth{get=>Config.MAX_HEALTH;}
    
    //敌人移动几次之后对我方造成伤害
    public static int EnemyDamageTrun{get=>Config.ENEMY_DAMAGE_TURN;}

    public static int CoinPerEnemy{get=>Config.COIN_PER_ENEMY;}

    public static int DeleteCardCost{get=>Config.DELETE_CARD_COST;}

    public static int RefreshCardCost{get=>Config.REFRESH_CARD_COST;}

    public static int RefreshHeroCost{get=>Config.REFRESH_HERO_COST;}
    

    public static int CurrentHealth{
        get=>runtimeData.currentHealth;
        set{
            runtimeData.currentHealth=value;
            UISystem.Instance.runtimeEffectDataView.healthText.text="Health: "+value.ToString()+"/"+Model.MaxHealth.ToString();
        }
    }
    public static int CurrentTurn{
        get=>runtimeData.currentTurn;
        set{
            runtimeData.currentTurn=value;
            UISystem.Instance.runtimeEffectDataView.turnText.text="Turn: "+value.ToString();
        }
    }
    public static int Electricity{
        get=>runtimeData.electricity;
        set{
            runtimeData.electricity=value;
            UISystem.Instance.runtimeEffectDataView.electricityText.text="Electricity: "+value.ToString();
        }
    }
    public static int Coin{
        get=>runtimeData.coin;
        set{
            runtimeData.coin=value;
            UISystem.Instance.runtimeEffectDataView.coinText.text="Coin: "+value.ToString();
        }
    }
    public static int BloodGemValue{
        get=>runtimeData.bloodGemValue;
        set{
            runtimeData.bloodGemValue=value;
            UISystem.Instance.runtimeEffectDataView.bloodGemValueText.text="Blood Gem: "+value.ToString();
        }
    }

    public static int CurrentWaveIndex{
        get=>runtimeData.currentWaveIndex;
        set{
            runtimeData.currentWaveIndex=value;
            UISystem.Instance.runtimeEffectDataView.waveText.text="Wave: "+value.ToString()+"/"+Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas.Count;
        }
    }
    public static int CurrentStageIndex{
        get=>runtimeData.currentStageIndex;
        set{
            runtimeData.currentStageIndex=value;
            UISystem.Instance.runtimeEffectDataView.stageText.text="Stage: "+value.ToString()+"/"+Model.CurrentLevelData.enemyStageDatas.Count;
        }
    }
    public static LevelData CurrentLevelData{
        get=>runtimeData.currentLevelData;
        set{
            runtimeData.currentLevelData=value;
        }
    }
    public static void Init(){
        Config.Init();
        ResetRuntimeData();
    }

    public static void ResetRuntimeData(){
        runtimeData.Reset();
        UISystem.Instance.runtimeEffectDataView.healthText.text="Health: "+Model.CurrentHealth.ToString()+"/"+Model.MaxHealth.ToString();
        UISystem.Instance.runtimeEffectDataView.turnText.text="Turn: "+Model.CurrentTurn.ToString();
        UISystem.Instance.runtimeEffectDataView.electricityText.text="Electricity: "+Model.Electricity.ToString();
        UISystem.Instance.runtimeEffectDataView.coinText.text="Coin: "+Model.Coin.ToString();
        UISystem.Instance.runtimeEffectDataView.bloodGemValueText.text="Blood Gem: "+Model.BloodGemValue.ToString();

        UISystem.Instance.runtimeEffectDataView.waveText.text="Wave: "+Model.CurrentWaveIndex.ToString()+"/"+Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas.Count;
        UISystem.Instance.runtimeEffectDataView.stageText.text="Stage: "+Model.CurrentStageIndex.ToString()+"/"+Model.CurrentLevelData.enemyStageDatas.Count;
    }
}