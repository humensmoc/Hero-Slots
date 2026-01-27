using System.Collections.Generic;

public class RuntimeData
{
    public int currentHealth;
    public int electricity;
    public int coin;
    public int bloodGemValue;

    public int currentTurn;
    public List<EndTurnBlocker> endTurnBlockers = new ();

    public int currentWaveIndex;
    public int currentStageIndex;
    public LevelData currentLevelData;
    public List<Enemy> enemies=new();
    public List<EnemyView> enemyViews=new();

    public List<Card> cardsInDeck=new();
    public Card[,] cardsInBattlefield=new Card[5,5];
    public List<CardView> cardViews=new();

    public List<BulletView> bulletInBattlefield = new ();

    public List<Hero> heroesInDeck=new();
    public List<Hero> heroesInBattlefield=new List<Hero>(){null, null, null, null, null};
    public List<HeroView> heroViews=new();
    public int currentHeroSlotIndex;

    public List<Relic> relics=new();
    public List<RelicView> relicViews=new();

    public void Reset(){
        this.currentHealth=Model.MaxHealth;
        this.electricity=0;
        this.coin=0;
        this.bloodGemValue=1;

        this.currentTurn=0;
        this.endTurnBlockers.Clear();

        this.currentWaveIndex=0;
        this.currentStageIndex=0;
        this.currentLevelData=EnemyLibrary.testLevelData;

        this.enemies.Clear();
        this.enemyViews.Clear();

        this.cardsInDeck.Clear();
        this.cardsInBattlefield=new Card[5,5];
        this.cardViews.Clear();

        this.bulletInBattlefield.Clear();

        this.heroesInDeck.Clear();
        this.heroesInBattlefield=new List<Hero>(){null, null, null, null, null};
        this.heroViews.Clear();
        this.currentHeroSlotIndex=0;

        this.relics.Clear();
        this.relicViews.Clear();
    }
}