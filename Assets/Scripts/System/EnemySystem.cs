using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

public class EnemySystem : Singleton<EnemySystem>
{
    public List<Enemy> enemies{get;private set;}=new();
    public List<EnemyView> enemyViews{get;private set;}=new();
    [SerializeField] Transform enemyParent;
    public int[] enemyPositionYs={0,1,2,3,4};

    public LevelData currentLevelData;
    public int currentStageIndex=0;
    public int currentWaveIndex=0;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<MoveAllEnemyGA>(MoveAllEnemyPerformer);
        ActionSystem.AttachPerformer<AddEnemyGA>(AddEnemyPerformer);
        ActionSystem.SubscribeReaction<DrawAllCardsGA>(DrawAllCardsPostReaction_MoveAllEnemy,ReactionTiming.POST);
        ActionSystem.SubscribeReaction<MoveAllEnemyGA>(MoveAllEnemyPostReaction_AddEnemy,ReactionTiming.POST);
    }
    
    void OnDisable()
    {
        ActionSystem.DetachPerformer<MoveAllEnemyGA>();
        ActionSystem.DetachPerformer<AddEnemyGA>();
        ActionSystem.UnsubscribeReaction<DrawAllCardsGA>(DrawAllCardsPostReaction_MoveAllEnemy,ReactionTiming.POST);
        ActionSystem.UnsubscribeReaction<MoveAllEnemyGA>(MoveAllEnemyPostReaction_AddEnemy,ReactionTiming.POST);
    }


    public void Init(LevelData levelData){
        currentLevelData = levelData.Clone();
        currentStageIndex=0;
        currentWaveIndex=0;
        AddEnemy(EnemyLibrary.GetEnemyDatas(currentLevelData.enemyStageDatas[currentStageIndex].enemyWaveDatas[currentWaveIndex].enemyNames));
    }

    private void DrawAllCardsPostReaction_MoveAllEnemy(DrawAllCardsGA drawAllCardsGA){
        MoveAllEnemyGA moveAllEnemyGA = new MoveAllEnemyGA();
        ActionSystem.Instance.AddReaction(moveAllEnemyGA);
    }

    private void MoveAllEnemyPostReaction_AddEnemy(MoveAllEnemyGA moveAllEnemyGA){
        // 只有当前stage还有wave时才添加敌人
        if(currentWaveIndex < currentLevelData.enemyStageDatas[currentStageIndex].enemyWaveDatas.Count){
            AddEnemyGA addEnemyGA = new AddEnemyGA(EnemyLibrary.GetEnemyDatas(currentLevelData.enemyStageDatas[currentStageIndex].enemyWaveDatas[currentWaveIndex].enemyNames));
            ActionSystem.Instance.AddReaction(addEnemyGA);
        }
    }
    

    public IEnumerator MoveAllEnemyPerformer(MoveAllEnemyGA moveAllEnemyGA){
        // 只推进wave索引，不推进stage（stage在所有敌人被击杀后才推进）
        if(currentWaveIndex < currentLevelData.enemyStageDatas[currentStageIndex].enemyWaveDatas.Count - 1){
            
        }else{
            Debug.Log($"All waves in stage {currentStageIndex} have been spawned. Waiting for all enemies to be defeated.");
        }

        for(int i = enemyViews.Count-1; i >= 0; i--){
            enemyViews[i].Move();
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;

    }

    private IEnumerator AddEnemyPerformer(AddEnemyGA addEnemyGA){
        if(currentWaveIndex==currentLevelData.enemyStageDatas[currentStageIndex].enemyWaveDatas.Count-1){
            yield return null;
        }else{
            AddEnemy(addEnemyGA.enemyDatas);
            currentWaveIndex++;
            yield return new WaitForSeconds(0.15f);
        }
       
    }

    public void AddEnemy(List<EnemyData> enemyDataList){
        // 如果传入的list为空或null，直接返回
        if(enemyDataList == null || enemyDataList.Count == 0){
            return;
        }

        // 如果敌人数量大于5，随机选择5个
        List<EnemyData> enemiesToSpawn = new List<EnemyData>();
        if(enemyDataList.Count > 5){
            // 创建一个副本并随机打乱，然后取前5个
            List<EnemyData> shuffledList = new List<EnemyData>(enemyDataList);
            for(int i = 0; i < 5; i++){
                int randomIndex = UnityEngine.Random.Range(i, shuffledList.Count);
                EnemyData temp = shuffledList[i];
                shuffledList[i] = shuffledList[randomIndex];
                shuffledList[randomIndex] = temp;
            }
            enemiesToSpawn = shuffledList.GetRange(0, 5);
        }else{
            enemiesToSpawn = new List<EnemyData>(enemyDataList);
        }

        // 获取可用的Y坐标位置
        int[] emptyYIndexs = enemyPositionYs;
        
        // 生成敌人
        for(int i=0; i<enemiesToSpawn.Count; i++){
            Enemy enemy = new Enemy(enemiesToSpawn[i]);
            // enemy.Health += math.max(2*TurnSystem.Instance.currentTurn, 0);
            enemies.Add(enemy);

            // 从剩余的Y位置中随机选择一个
            int yIndex = emptyYIndexs[UnityEngine.Random.Range(0, emptyYIndexs.Length)];
            EnemyView enemyView = EnemyCreator.Instance.CreateEnemyView(
                enemy,
                enemyParent.position + new Vector3(0, yIndex*1.2f, 0),
                Quaternion.identity,
                0,
                yIndex
            );
            enemyViews.Add(enemyView);

            // 从可用位置中移除已使用的Y坐标
            emptyYIndexs = emptyYIndexs.Where(y => y != yIndex).ToArray();
        }
    }

    public void RemoveEnemy(Enemy enemy){           
        Debug.Log($"RemoveEnemy called for {enemy.Name}");
        
        EnemyView enemyView = enemyViews.Find(view => view.enemy == enemy);
        if(enemyView != null){
            Debug.Log($"Found EnemyView for {enemy.Name}, removing and destroying");
            Vector3 enemyPosition = enemyView.transform.position;
            enemyViews.Remove(enemyView);
            enemies.Remove(enemy);
            Destroy(enemyView.gameObject);
            
            InGameEconomySystem.Instance.AddCoin(enemyPosition, Model.coinPerEnemy);
            
            // 检查是否所有敌人都被击杀了
            CheckStageCompletion();
        }else{
            Debug.LogError($"Could not find EnemyView for enemy {enemy.Name}!");
        }
    }
    
    private void CheckStageCompletion(){
        // 检查是否所有敌人都被击杀了
        if(enemies.Count == 0){
            // 检查当前stage的所有wave是否都已经生成完毕
            bool allWavesSpawned = currentWaveIndex >= currentLevelData.enemyStageDatas[currentStageIndex].enemyWaveDatas.Count - 1;
            
            if(allWavesSpawned){
                Debug.Log($"Stage {currentStageIndex} completed! All enemies defeated.");
                
                // 进入下一个stage
                if(currentStageIndex < currentLevelData.enemyStageDatas.Count - 1){
                    Debug.Log("Opening shop and advancing to next stage...");
                    UISystem.Instance.inGameShopPanelView.SetActive(true);
                    currentStageIndex++;
                    currentWaveIndex = 0;
                }else{
                    Debug.Log("Level completed! All stages finished.");
                    // 这里可以添加关卡完成的逻辑
                }
            }else{
                Debug.Log($"All current enemies defeated, but stage {currentStageIndex} still has more waves to spawn.");
            }
        }
    }

    public EnemyView GetNearestEnemyView(EnemyView enemyView){
        List<EnemyView> othersEnemyViews=enemyViews.Where(view => view != enemyView).ToList();
        if(othersEnemyViews.Count > 0){
            return othersEnemyViews.OrderBy(view => Vector3.Distance(view.transform.position, enemyView.transform.position)).FirstOrDefault();
        }else{
            return null;
        }
    }

    public void Reset()
    {
        enemies.Clear();
        foreach(var enemyView in enemyViews){
            Destroy(enemyView.gameObject);
        }
        enemyViews.Clear();
    }
}
