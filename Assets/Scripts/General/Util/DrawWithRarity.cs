using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SingleRarityData{

    public CardRarity rarity;
    public int weight;
    public int maxPityCount;
    public int currentPityCount;
    public int rarityIndex;
}

[System.Serializable]
public class RarityData{
    public List<SingleRarityData> singleRarityDatas;
    public bool isUsePitySystem;
}

public static class DrawWithRarity
{
   public static CardRarity GetSingleCardRarity(RarityData Data){
        List<SingleRarityData> rarityDatas = Data.singleRarityDatas;
        bool isUsePitySystem = Data.isUsePitySystem;
        CardRarity rarity=CardRarity.Common;

        // 如果启用保底系统，先检查保底
        if(isUsePitySystem){
            // 检查保底：找出所有触发保底的稀有度
            List<SingleRarityData> pityTriggeredRarities = new List<SingleRarityData>();
            foreach(var rarityData in rarityDatas){
                if(rarityData.currentPityCount >= rarityData.maxPityCount && rarityData.maxPityCount > 0){
                    pityTriggeredRarities.Add(rarityData);
                }
            }

            // 如果有保底触发，优先给最高稀有度的
            if(pityTriggeredRarities.Count > 0){
                // 按稀有度从高到低排序
                pityTriggeredRarities.Sort((a, b) => a.rarityIndex.CompareTo(b.rarityIndex));
                
                // 选择最高稀有度的保底
                SingleRarityData highestPityRarity = pityTriggeredRarities[0];
                rarity = highestPityRarity.rarity;
                
                // 重置该稀有度的保底计数
                highestPityRarity.currentPityCount = 0;
                
                // 更新其他稀有度的保底计数
                foreach(var rarityData in rarityDatas){
                    if(rarityData.rarity != rarity){
                        // 如果这个稀有度也触发了保底但没有被选中，保持保底状态（不重置，也不+1）
                        // 如果这个稀有度没有触发保底，保底计数+1
                        bool isAlsoPityTriggered = pityTriggeredRarities.Any(p => p.rarity == rarityData.rarity);
                        if(!isAlsoPityTriggered){
                            rarityData.currentPityCount++;
                        }
                        // 如果isAlsoPityTriggered为true，说明这个稀有度也保底了但没被选中，保持currentPityCount不变（顺延到下一抽）
                    }
                }
                
                return rarity;
            }
        }

        // 正常按权重抽卡
        // 计算总权重
        int allWeight=0;
        foreach(var rarityData in rarityDatas){
            allWeight+=rarityData.weight;
        }

        // 如果总权重为0，返回默认值
        if(allWeight == 0){
            return CardRarity.Common;
        }

        // 生成0到allWeight之间的随机数
        int randomWeight=Random.Range(0,allWeight);
        
        // 根据权重选择rarity：累加权重，当累加值大于随机数时选择
        int accumulatedWeight = 0;
        foreach(var rarityData in rarityDatas){
            accumulatedWeight += rarityData.weight;
            if(randomWeight < accumulatedWeight){
                rarity = rarityData.rarity;
                break;
            }
        }

        // 如果启用保底系统，更新保底计数：抽中的稀有度重置，其他稀有度+1
        if(isUsePitySystem){
            foreach(var rarityData in rarityDatas){
                if(rarityData.rarity == rarity){
                    rarityData.currentPityCount = 0;
                }else{
                    rarityData.currentPityCount++;
                }
            }
        }

        return rarity;
    }
}