using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemyGA : GameAction
{
    public EnemyData enemyData;

    public AddEnemyGA(EnemyData enemyData){
        this.enemyData = enemyData;
    }
}
