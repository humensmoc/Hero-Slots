using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemyGA : GameAction
{
    public List<EnemyData> enemyDatas;

    public AddEnemyGA(List<EnemyData> enemyDatas){
        this.enemyDatas = enemyDatas;
    }
}
