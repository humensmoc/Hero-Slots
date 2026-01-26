public class RuntimeData
{
    public int currentHealth;
    public int currentTurn;
    public int electricity;
    public int coin;
    public int bloodGemValue;

    public int currentWaveIndex;
    public int currentStageIndex;
    public LevelData currentLevelData;

    public void Reset(){
        this.currentHealth=Model.MaxHealth;
        this.currentTurn=0;
        this.electricity=0;
        this.coin=0;
        this.bloodGemValue=1;

        this.currentWaveIndex=0;
        this.currentStageIndex=0;
        this.currentLevelData=EnemyLibrary.testLevelData;
    }
}