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
        Model.CurrentLevelData = levelData.Clone();
        Model.CurrentStageIndex=0;
        Model.CurrentWaveIndex=0;
        AddEnemy(EnemyLibrary.GetEnemyDatas(Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas[Model.CurrentWaveIndex].enemyNames));
        Model.CurrentWaveIndex++;
    }

    private void DrawAllCardsPostReaction_MoveAllEnemy(DrawAllCardsGA drawAllCardsGA){
        MoveAllEnemyGA moveAllEnemyGA = new MoveAllEnemyGA();
        ActionSystem.Instance.AddReaction(moveAllEnemyGA);
    }

    private void MoveAllEnemyPostReaction_AddEnemy(MoveAllEnemyGA moveAllEnemyGA){
        // 只有当前stage还有wave时才添加敌人
        if(Model.CurrentWaveIndex < Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas.Count){
            AddEnemyGA addEnemyGA = new AddEnemyGA(EnemyLibrary.GetEnemyDatas(Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas[Model.CurrentWaveIndex].enemyNames));
            ActionSystem.Instance.AddReaction(addEnemyGA);
        }
    }
    

    public IEnumerator MoveAllEnemyPerformer(MoveAllEnemyGA moveAllEnemyGA){
        // 只推进wave索引，不推进stage（stage在所有敌人被击杀后才推进）
        if(Model.CurrentWaveIndex < Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas.Count - 1){
            
        }else{
            // Debug.Log($"All waves in stage {currentStageIndex} have been spawned. Waiting for all enemies to be defeated.");
        }

        for(int i = enemyViews.Count-1; i >= 0; i--){
            enemyViews[i].Move();
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;

    }

    private IEnumerator AddEnemyPerformer(AddEnemyGA addEnemyGA){
        if(Model.CurrentWaveIndex==Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas.Count){
            yield return null;
        }else{
            
            AddEnemy(EnemyLibrary.GetEnemyDatas(Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas[Model.CurrentWaveIndex].enemyNames));
            Model.CurrentWaveIndex++;
            yield return new WaitForSeconds(0.15f);
        }
       
    }

    public void AddEnemy(List<EnemyData> enemyDataList){
        // 如果传入的list为空或null，直接返回
        if(enemyDataList == null || enemyDataList.Count == 0){
            // Debug.LogError("enemyDataList is null or empty");
            return;
        }

        // 复制敌人数据列表并洗牌
        List<EnemyData> enemiesToSpawn = new List<EnemyData>(enemyDataList);
        enemiesToSpawn.Shuffle();

        // 获取可用的Y坐标位置
        List<int> emptyYIndexs = new List<int>(enemyPositionYs);
        
        
        // 生成敌人
        for(int i=0; i<enemiesToSpawn.Count; i++){

            List<int> possiblePositions = enemiesToSpawn[i].GetPossiblePosition();
            List<int> canPlacePositions = possiblePositions.Where(position => {
                List<int> occupiedPositions = enemiesToSpawn[i].GetOccupiedPositions(position);
                return occupiedPositions.All(pos => emptyYIndexs.Contains(pos));
            }).ToList();
            if(canPlacePositions.Count == 0){
                // Debug.Log($"No possible positions found for enemy {enemiesToSpawn[i].Name}");
                continue;
                
            }else{
                // Debug.Log($"Possible positions found for enemy {enemiesToSpawn[i].Name}: {string.Join(", ", canPlacePositions)}");
            }

            int yIndex = canPlacePositions[UnityEngine.Random.Range(0, canPlacePositions.Count)];

            // Debug.Log($"enemy {enemiesToSpawn[i].Name} selected yIndex: {yIndex}");

            Enemy enemy = new Enemy(enemiesToSpawn[i]);
            // enemy.Health += math.max(2*TurnSystem.Instance.currentTurn, 0);
            enemies.Add(enemy);

            EnemyView enemyView = EnemyCreator.Instance.CreateEnemyView(
                enemy,
                enemyParent.position + new Vector3(0, yIndex*1.2f, 0),
                Quaternion.identity,
                0,
                yIndex
            );
            enemyViews.Add(enemyView);

            // 从可用位置中移除所有被占据的Y坐标
            List<int> occupiedPositions = enemiesToSpawn[i].GetOccupiedPositions(yIndex);
            emptyYIndexs = emptyYIndexs.Where(y => !occupiedPositions.Contains(y)).ToList();
            // Debug.Log($"after emptyYIndexs: {string.Join(", ", emptyYIndexs)}");
            // Debug.Log($"-----------------------------------------");
        }
    }

    public void RemoveEnemy(Enemy enemy){           
        // Debug.Log($"RemoveEnemy called for {enemy.Name}");
        
        EnemyView enemyView = enemyViews.Find(view => view.enemy == enemy);
        if(enemyView != null){
            // Debug.Log($"Found EnemyView for {enemy.Name}, removing and destroying");
            Vector3 enemyPosition = enemyView.transform.position;
            enemyViews.Remove(enemyView);
            enemies.Remove(enemy);
            Destroy(enemyView.gameObject);
            
            InGameEconomySystem.Instance.AddCoin(enemyPosition, Model.CoinPerEnemy);
            
            // 检查是否所有敌人都被击杀了
            CheckStageCompletion();
        }else{
            // Debug.LogError($"Could not find EnemyView for enemy {enemy.Name}!");
        }
    }
    
    private void CheckStageCompletion(){
        // 检查是否所有敌人都被击杀了
        if(enemies.Count == 0){
            // 检查当前stage的所有wave是否都已经生成完毕
            bool allWavesSpawned = Model.CurrentWaveIndex >= Model.CurrentLevelData.enemyStageDatas[Model.CurrentStageIndex].enemyWaveDatas.Count - 1;
            
            if(allWavesSpawned){
                // Debug.Log($"Stage {currentStageIndex} completed! All enemies defeated.");
                
                // 进入下一个stage
                if(Model.CurrentStageIndex < Model.CurrentLevelData.enemyStageDatas.Count - 1){
                    // Debug.Log("Opening shop and advancing to next stage...");
                    UISystem.Instance.inGameShopPanelView.isNeedOpenInGameShop=true;
                    Model.CurrentStageIndex++;
                    Model.CurrentWaveIndex = 0;
                }else{
                    // Debug.Log("Level completed! All stages finished.");
                    // 这里可以添加关卡完成的逻辑
                }
            }else{
                // Debug.Log($"All current enemies defeated, but stage {currentStageIndex} still has more waves to spawn.");/
            }
        }
    }

    /// <summary>
    /// 获取距离输入的敌人最近的敌人
    /// </summary>
    /// <param name="enemyView"></param>
    /// <returns></returns>
    public EnemyView GetNearestEnemyView(EnemyView enemyView){
        List<EnemyView> othersEnemyViews=enemyViews.Where(view => view != enemyView).ToList();
        if(othersEnemyViews.Count > 0){
            return othersEnemyViews.OrderBy(view => Vector3.Distance(view.transform.position, enemyView.transform.position)).FirstOrDefault();
        }else{
            return null;
        }
    }

    /// <summary>
    /// 获取随机的敌人
    /// </summary>
    /// <returns></returns>
    public EnemyView GetRandomEnemyView(){
        if(enemyViews == null || enemyViews.Count == 0){
            return null;
        }
        return enemyViews[UnityEngine.Random.Range(0, enemyViews.Count)];
    }

    /// <summary>
    /// 获取x轴距离主城的敌人
    /// </summary>
    /// <returns></returns>
    public EnemyView GetClosestEnemyView(){
        if(enemyViews == null || enemyViews.Count == 0){
            return null;
        }
        return enemyViews.OrderBy(view => -view.x).FirstOrDefault();
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
