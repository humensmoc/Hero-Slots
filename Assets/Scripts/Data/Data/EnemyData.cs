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
    public int Length;

    public EnemyData Clone(){
        EnemyData clone = new EnemyData(EnemyNameEnum);
        clone.Name=Name;
        clone.EnemyNameEnum=EnemyNameEnum;
        clone.Description=Description;
        clone.Health=Health;
        clone.Length=Length;
        return clone;
    }

    public EnemyData(EnemyName enemyName){
        EnemyNameEnum = enemyName;
        Name = enemyName.ToString();
    }

    public List<int> GetOccupiedPositions(int position){
        switch(Length){
            case 1:
                return new List<int>{position};
            case 2:
                return new List<int>{position,position+1};
            case 3:
                return new List<int>{position,position+1,position+2};
            case 4:
                return new List<int>{position,position+1,position+2,position+3};
            case 5:
                return new List<int>{position,position+1,position+2,position+3,position+4};
            default:
                return new List<int>();
        }
    }

    public List<int> GetPossiblePosition(){
        switch(Length){
            case 1:
                return new List<int>{0,1,2,3,4};
            case 2:
                return new List<int>{0,1,2,3};
            case 3:
                return new List<int>{0,1,2};
            case 4:
                return new List<int>{0,1};
            case 5:
                return new List<int>{0};
            default:
                return new List<int>();
        }
    }

    public EnemyData SetDescription(string description){
        Description = description;
        return this;
    }
    public EnemyData SetHealth(int health){
        Health = health;
        return this;
    }
    public EnemyData SetLength(int length){
        Length = length;
        return this;
    }
}
