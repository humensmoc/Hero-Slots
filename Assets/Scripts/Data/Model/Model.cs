using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class Model
{
    public const int maxHealth=3;

    public static int currentHealth=0;

    //敌人移动几次之后对我方造成伤害
    public const int enemyDamageX=3;

    public static void Init(){
        currentHealth=maxHealth;
    }
    
}