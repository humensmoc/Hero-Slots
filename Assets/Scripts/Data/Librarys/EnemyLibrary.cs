using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName{
    Enemy_Default,
}

public class WaveData{
    public List<EnemyName> enemyNames = new List<EnemyName>();

    public WaveData(List<EnemyName> inputEnemyNames){
        enemyNames = inputEnemyNames;
    }

    public WaveData Clone(){
        WaveData clone = new WaveData(new List<EnemyName>(enemyNames));
        return clone;
    }
}

public class StageData{
    public List<WaveData> enemyWaveDatas = new List<WaveData>();

    public StageData(List<WaveData> inputEnemyWaveDatas){
        enemyWaveDatas = inputEnemyWaveDatas;
    }

    public StageData Clone(){
        List<WaveData> clonedWaveDatas = new List<WaveData>();
        foreach(var waveData in enemyWaveDatas){
            clonedWaveDatas.Add(waveData.Clone());
        }
        StageData clone = new StageData(clonedWaveDatas);
        return clone;
    }
}

public class LevelData{
    public List<StageData> enemyStageDatas = new();

    public LevelData(List<StageData> inputEnemyStageDatas){
        enemyStageDatas=inputEnemyStageDatas;
    } 

    public LevelData Clone(){
        List<StageData> clonedStageDatas = new List<StageData>();
        foreach(var stageData in enemyStageDatas){
            clonedStageDatas.Add(stageData.Clone());
        }
        LevelData clone = new LevelData(clonedStageDatas);
        return clone;
    }
}

public static class EnemyLibrary{

    public static LevelData testLevelData = new LevelData(new List<StageData>(){
        
        new StageData(new List<WaveData>(){
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
            }),
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
            }),
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
            }),
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
            }),
        }),
        new StageData(new List<WaveData>(){
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
            }),
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
            }),
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
            }),
            new WaveData(new List<EnemyName>(){
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
                EnemyName.Enemy_Default,
            }),
        }),
    });
    public static List<EnemyData> enemyDatas = new List<EnemyData>(){
        new EnemyData(EnemyName.Enemy_Default)
            .SetDescription("Enemy_Default Description")
            .SetHealth(10),
    };

    public static List<EnemyData> GetEnemyDatas(List<EnemyName> enemyNames){
        List<EnemyData> result = new List<EnemyData>();
        foreach(var enemyName in enemyNames){
            EnemyData enemyData = enemyDatas.Find(e => e.EnemyNameEnum == enemyName);
            if(enemyData != null){
                result.Add(enemyData);
            }
        }
        return result;
    }
}