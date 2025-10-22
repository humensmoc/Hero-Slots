using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Enemy")]
public class ScriptableEnemyData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField][field: TextArea(3,10)] public string Description {get; private set;}
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public int Health {get; private set;}
}

public class EnemyData{
    public string Name;
    public EnemyName EnemyNameEnum;
    public string Description;
    public int Health;

    public EnemyData Clone(){
        EnemyData clone = new EnemyData(EnemyNameEnum);
        clone.Name=Name;
        clone.EnemyNameEnum=EnemyNameEnum;
        clone.Description=Description;
        clone.Health=Health;
        return clone;
    }

    public EnemyData(EnemyName enemyName){
        Name = enemyName.ToString();
    }

    public EnemyData SetDescription(string description){
        Description = description;
        return this;
    }
    public EnemyData SetHealth(int health){
        Health = health;
        return this;
    }
}
