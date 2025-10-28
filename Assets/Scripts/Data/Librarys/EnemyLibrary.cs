using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName{
    Enemy_Default,
}

public static class EnemyLibrary{
    public static List<EnemyData> enemyDatas = new List<EnemyData>(){
        new EnemyData(EnemyName.Enemy_Default)
            .SetDescription("Enemy_Default Description")
            .SetHealth(10),
    };
}