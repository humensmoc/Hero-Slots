using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    private readonly EnemyData enemyData;
    public string Name => enemyData.Name;
    public string Description => enemyData.Description;
    public Sprite Image => enemyData.Image;
    public int MaxHealth;
    public int Health;

    public Enemy(EnemyData enemyData){
        this.enemyData = enemyData;
        Health = enemyData.Health;
        MaxHealth = enemyData.Health;
    }
}
