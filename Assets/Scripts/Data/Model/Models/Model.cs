using System.Collections.Generic;

public static class Model
{
    public static RuntimeData runtimeData=new RuntimeData();

#region Config
    public static int MaxHealth{get=>Config.MAX_HEALTH;}
    
    //敌人移动几次之后对我方造成伤害
    public static int EnemyDamageTrun{get=>Config.ENEMY_DAMAGE_TURN;}

    public static int CoinPerEnemy{get=>Config.COIN_PER_ENEMY;}

    public static int DeleteCardCost{get=>Config.DELETE_CARD_COST;}

    public static int RefreshCardCost{get=>Config.REFRESH_CARD_COST;}

    public static int RefreshHeroCost{get=>Config.REFRESH_HERO_COST;}
    
    public static float CardPositionInterval{get=>Config.CARD_POSITION_INTERVAL;}

    public static float HeroPositionInterval{get=>Config.HERO_POSITION_INTERVAL;}
#endregion

    public static int CurrentHealth{
        get=>runtimeData.currentHealth;
        set{
            runtimeData.currentHealth=value;
            UISystem.Instance.runtimeEffectDataView.healthText.text="Health: "+value.ToString()+"/"+Model.MaxHealth.ToString();
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

#region Turn System
    public static int CurrentTurn{
        get=>runtimeData.currentTurn;
        set{
            runtimeData.currentTurn=value;
            UISystem.Instance.runtimeEffectDataView.turnText.text="Turn: "+value.ToString();
        }
    }
    public static List<EndTurnBlocker> EndTurnBlockers{
        get=>runtimeData.endTurnBlockers;
        set{
            runtimeData.endTurnBlockers=value;
        }
    }
#endregion
#region Enemy System
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

    public static List<Enemy> Enemies{
        get=>runtimeData.enemies;
        set{
            runtimeData.enemies=value;
        }
    }
    public static List<EnemyView> EnemyViews{
        get=>runtimeData.enemyViews;
        set{
            runtimeData.enemyViews=value;
        }
    }
#endregion

#region Card System
    public static List<Card> CardsInDeck{
        get=>runtimeData.cardsInDeck;
        set{
            runtimeData.cardsInDeck=value;
        }
    }
    public static Card[,] CardsInBattlefield{
        get=>runtimeData.cardsInBattlefield;
        set{
            runtimeData.cardsInBattlefield=value;
        }
    }
    public static List<CardView> CardViews{
        get=>runtimeData.cardViews;
        set{
            runtimeData.cardViews=value;
        }
    }
#endregion

#region Bullet System
    public static List<BulletView> BulletInBattlefield{
        get=>runtimeData.bulletInBattlefield;
        set{
            runtimeData.bulletInBattlefield=value;
        }
    }
#endregion

#region Hero System
    public static List<Hero> HeroesInDeck{
        get=>runtimeData.heroesInDeck;
        set{
            runtimeData.heroesInDeck=value;
        }
    }
    public static List<Hero> HeroesInBattlefield{
        get=>runtimeData.heroesInBattlefield;
        set{
            runtimeData.heroesInBattlefield=value;
        }
    }
    public static List<HeroView> HeroViews{
        get=>runtimeData.heroViews;
        set{
            runtimeData.heroViews=value;
        }
    }
    public static int CurrentHeroSlotIndex{
        get=>runtimeData.currentHeroSlotIndex;
        set{
            runtimeData.currentHeroSlotIndex=value;
        }
    }
#endregion

#region Relic System
    public static List<Relic> Relics{
        get=>runtimeData.relics;
        set{
            runtimeData.relics=value;
        }
    }
    public static List<RelicView> RelicViews{
        get=>runtimeData.relicViews;
        set{
            runtimeData.relicViews=value;
        }
    }
#endregion
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