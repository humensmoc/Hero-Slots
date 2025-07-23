using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyCreator : Singleton<EnemyCreator>
{
    [SerializeField] GameObject enemyViewPrefab;
    public EnemyView CreateEnemyView(Enemy enemy,Vector3 position,Quaternion rotation,int x,int y)
    {
        GameObject enemyInstance = Instantiate(enemyViewPrefab,position,rotation);
        enemyInstance.transform.localScale = Vector3.zero;
        enemyInstance.transform.DOScale(Vector3.one, 0.15f);
        EnemyView enemyView = enemyInstance.GetComponent<EnemyView>();
        enemyView.Init(enemy,x,y);
        return enemyView;
    }
}
