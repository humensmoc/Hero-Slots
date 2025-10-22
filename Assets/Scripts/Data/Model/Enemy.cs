using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public EnemyData enemyData;
    public string Name;
    public string Description;
    public int MaxHealth;
    public int Health;

    public Enemy(EnemyData enemyData){
        this.enemyData = enemyData.Clone();
        Name = this.enemyData.Name;
        Description = this.enemyData.Description;
        Health = this.enemyData.Health;
        MaxHealth = this.enemyData.Health;
    }
}
