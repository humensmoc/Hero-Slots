using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    public List<Enemy> enemies{get;private set;}=new();
    public List<EnemyView> enemyViews{get;private set;}=new();
    [SerializeField] Transform enemyParent;

    public void Init(EnemyData enemyData){
        for(int i = 0; i < 5; i++){
            Enemy enemy = new Enemy(enemyData); 
            enemies.Add(enemy);
            EnemyView enemyView = EnemyCreator.Instance.CreateEnemyView(enemy,enemyParent.position+new Vector3(0,i*1.2f,0),Quaternion.identity);
            enemyViews.Add(enemyView);
        }
    }

    public void AddEnemy(Enemy enemy){
        enemies.Add(enemy);
        EnemyView enemyView = EnemyCreator.Instance.CreateEnemyView(enemy,enemyParent.position,Quaternion.identity);
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
