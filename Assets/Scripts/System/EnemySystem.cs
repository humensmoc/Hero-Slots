using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    public List<Enemy> enemies{get;private set;}=new();
    public List<EnemyView> enemyViews{get;private set;}=new();
    [SerializeField] Transform enemyParent;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<MoveAllEnemyGA>(MoveAllEnemyPerformer);
        ActionSystem.AttachPerformer<AddEnemyGA>(AddEnemyPerformer);
        ActionSystem.SubscribeReaction<NextTurnGA>(NextTurnPreReaction,ReactionTiming.POST);
        ActionSystem.SubscribeReaction<NextTurnGA>(AddEnemyPreReaction,ReactionTiming.PRE);
    }
    
    void OnDisable()
    {
        ActionSystem.DetachPerformer<MoveAllEnemyGA>();
        ActionSystem.DetachPerformer<AddEnemyGA>();
        ActionSystem.UnsubscribeReaction<NextTurnGA>(NextTurnPreReaction,ReactionTiming.POST);
        ActionSystem.UnsubscribeReaction<NextTurnGA>(AddEnemyPreReaction,ReactionTiming.PRE);
    }


    public void Init(EnemyData enemyData){
        AddEnemy(new Enemy(enemyData));
    }

    public IEnumerator MoveAllEnemyPerformer(MoveAllEnemyGA moveAllEnemyGA){
        for(int i = 0; i < enemyViews.Count; i++){
            enemyViews[i].transform.Translate(Vector3.left);
            yield return new WaitForSeconds(0.15f);
        }
        yield return null;
    }

    private void NextTurnPreReaction(NextTurnGA nextTurnGA){
        MoveAllEnemyGA moveAllEnemyGA = new MoveAllEnemyGA();
        ActionSystem.Instance.AddReaction(moveAllEnemyGA);
    }

    private void AddEnemyPreReaction(NextTurnGA nextTurnGA){
        AddEnemyGA addEnemyGA = new AddEnemyGA(GameInitializer.Instance.testEnemyData);
        ActionSystem.Instance.AddReaction(addEnemyGA);
    }

    private IEnumerator AddEnemyPerformer(AddEnemyGA addEnemyGA){
        AddEnemy(new Enemy(addEnemyGA.enemyData));
        yield return new WaitForSeconds(0.15f);
    }

    public void AddEnemy(Enemy enemy){
        enemies.Add(enemy);
        EnemyView enemyView = EnemyCreator.Instance.CreateEnemyView(enemy,enemyParent.position+new Vector3(0,Random.Range(0,5)*1.2f,0),Quaternion.identity);
        enemyViews.Add(enemyView);
    }

    public void RemoveEnemy(Enemy enemy){           
        enemies.Remove(enemy);
        EnemyView enemyView = enemyViews.Find(view => view.enemy == enemy);
        if(enemyView != null){
            enemyViews.Remove(enemyView);
            Destroy(enemyView.gameObject);
        }
    }
}
