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


    public void Init(EnemyData enemyData){
        AddEnemy(enemyData,1);
    }

    public IEnumerator MoveAllEnemyPerformer(MoveAllEnemyGA moveAllEnemyGA){
        for(int i = 0; i < enemyViews.Count; i++){
            enemyViews[i].transform.Translate(Vector3.left);
            enemyViews[i].x++;
            yield return new WaitForSeconds(0.15f);
        }
        yield return null;
        
        CardSelectSystem.Instance.ShowCardSelectView();
        CardSelectSystem.Instance.Refresh();
    }

    private void DrawAllCardsPostReaction_MoveAllEnemy(DrawAllCardsGA drawAllCardsGA){
        MoveAllEnemyGA moveAllEnemyGA = new MoveAllEnemyGA();
        ActionSystem.Instance.AddReaction(moveAllEnemyGA);
    }

    private void MoveAllEnemyPostReaction_AddEnemy(MoveAllEnemyGA moveAllEnemyGA){
        AddEnemyGA addEnemyGA = new AddEnemyGA(EnemyLibrary.enemyDatas[0]);
        ActionSystem.Instance.AddReaction(addEnemyGA);
    }

    private IEnumerator AddEnemyPerformer(AddEnemyGA addEnemyGA){
        AddEnemy(addEnemyGA.enemyData,Mathf.Min(TurnSystem.Instance.currentTurn/3+1,5));
        yield return new WaitForSeconds(0.15f);
    }

    public void AddEnemy(EnemyData enemyData,int Count){

        
        int[] emptyYIndexs = enemyPositionYs;
        for(int i=0;i<Count;i++){
            Enemy enemy=new Enemy(enemyData);

            enemy.Health+=math.max(2*TurnSystem.Instance.currentTurn,0);
            
            enemies.Add(enemy);

            int yIndex = emptyYIndexs[UnityEngine.Random.Range(0,emptyYIndexs.Length)];
            EnemyView enemyView = EnemyCreator.Instance.CreateEnemyView(enemy,enemyParent.position+new Vector3(0,yIndex*1.2f,0),Quaternion.identity,0,yIndex);
            enemyViews.Add(enemyView);

            emptyYIndexs = emptyYIndexs.Where(y => y != yIndex).ToArray();
        }
    }

    public void RemoveEnemy(Enemy enemy){           
        Debug.Log($"RemoveEnemy called for {enemy.Name}");
        
        EnemyView enemyView = enemyViews.Find(view => view.enemy == enemy);
        if(enemyView != null){
            Debug.Log($"Found EnemyView for {enemy.Name}, removing and destroying");
            enemyViews.Remove(enemyView);
            enemies.Remove(enemy);
            Destroy(enemyView.gameObject);
        }else{
            Debug.LogError($"Could not find EnemyView for enemy {enemy.Name}!");
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
