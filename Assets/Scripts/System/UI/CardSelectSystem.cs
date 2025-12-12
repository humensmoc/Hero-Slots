using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

[System.Serializable]
public class RarityData{

    public CardRarity rarity;
    public int weight;
    public int maxPityCount;
    public int currentPityCount;
}

public class CardSelectSystem : Singleton<CardSelectSystem>
{
    public List<RarityData> rarityDatas=new();
    public bool isUsePitySystem;
    public int testCount;
    public CardSelectPanelView cardSelectPanelView;
    public GameObject cardSelectPanel;
    public GameObject cardSelectItemPrefab;
    public List<CardData> cardDatas=new();
    public int optionCount;

    public Button refreshButton;
    public Button skipButton;

    public bool isSelectingCard = false;

    /// <summary>
    /// 是否显示卡牌选择面板的body
    /// </summary>
    public bool isShow = false;
    
    public void Init(){
        cardSelectPanelView.Init();
        refreshButton.onClick.AddListener(ManualRefresh);
        skipButton.onClick.AddListener(HideCardSelectView);
    }

    public void ShowCardSelectView(){
        isSelectingCard = true;
        cardSelectPanel.SetActive(true);
        cardSelectPanelView.ShowPanel();
    }

    public void HideCardSelectView(){
        cardSelectPanelView.HidePanel();
        isSelectingCard = false;
    }

    public void Refresh(){

        

        cardSelectPanelView.RemoveAllCardSelectItem();
        cardDatas.Clear();
        CardLibrary.cardDatas.ForEach(cardData => cardDatas.Add(cardData.Clone()));

        for(int i=0;i<optionCount;i++){
            if(cardDatas.Count == 0) break; // 防止从空列表抽取
            
            CardData cardData=CardLibrary.GetCardDatasByRarity(GetSingleCardRarity()).Draw();
            cardDatas.Add(cardData);
            cardSelectPanelView.AddCardSelectItem(cardData);
        }

    }

    public void ManualRefresh(){
        if(RuntimeEffectData.coin<Model.refreshCardCost){
            TipsController.Instance.ShowTips("Not enough coin");
            return;
        }

        InGameEconomySystem.Instance.SpendCoin(
            CoordinateConverter.UIToWorld(refreshButton.transform.position),
            Model.refreshCardCost);

        Refresh();
    }

    public void SelectCard(CardData cardData){
        CardSystem.Instance.cardsInDeck.Add(new Card(cardData));
        isSelectingCard = false;
        HideCardSelectView();
    }

    public void GetCardRarity(int cardCount){
        string rarityString="1 : ";
        for(int i=0;i<cardCount;i++){
            CardRarity rarity=GetSingleCardRarity();
            rarityString+=rarity.ToString()+" , "+i+" : ";
        }
        Debug.Log(rarityString);
    }

    /// <summary>
    /// 获取稀有度的排序值（用于排序，值越大稀有度越高）
    /// </summary>
    private int GetRaritySortValue(CardRarity rarity){
        return rarity switch{
            CardRarity.Common => 0,
            CardRarity.Rare => 1,
            CardRarity.Epic => 2,
            CardRarity.Legendary => 3,
            _ => 0
        };
    }

    /// <summary>
    /// 获取单个卡牌的稀有度
    /// </summary>
    /// <returns></returns>
    public CardRarity GetSingleCardRarity(){
        CardRarity rarity=CardRarity.Common;

        // 如果启用保底系统，先检查保底
        if(isUsePitySystem){
            // 检查保底：找出所有触发保底的稀有度
            List<RarityData> pityTriggeredRarities = new List<RarityData>();
            foreach(var rarityData in rarityDatas){
                if(rarityData.currentPityCount >= rarityData.maxPityCount && rarityData.maxPityCount > 0){
                    pityTriggeredRarities.Add(rarityData);
                }
            }

            // 如果有保底触发，优先给最高稀有度的
            if(pityTriggeredRarities.Count > 0){
                // 按稀有度从高到低排序
                pityTriggeredRarities.Sort((a, b) => GetRaritySortValue(b.rarity).CompareTo(GetRaritySortValue(a.rarity)));
                
                // 选择最高稀有度的保底
                RarityData highestPityRarity = pityTriggeredRarities[0];
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

    /// <summary>
    /// 验证权重算法：抽100次卡，统计各稀有度比例并与权重比例对比
    /// </summary>
    public void ValidateWeightAlgorithm(){
        
        // 计算总权重
        int allWeight = 0;
        Dictionary<CardRarity, int> weightDict = new Dictionary<CardRarity, int>();
        foreach(var rarityData in rarityDatas){
            allWeight += rarityData.weight;
            weightDict[rarityData.rarity] = rarityData.weight;
        }

        if(allWeight == 0){
            Debug.LogError("总权重为0，无法验证");
            return;
        }

        // 统计各稀有度出现次数
        Dictionary<CardRarity, int> countDict = new Dictionary<CardRarity, int>();
        foreach(var rarityData in rarityDatas){
            countDict[rarityData.rarity] = 0;
        }

        // 抽100次卡
        for(int i = 0; i < testCount; i++){
            CardRarity rarity = GetSingleCardRarity();
            countDict[rarity]++;
        }

        // 输出验证结果
        System.Text.StringBuilder logBuilder = new System.Text.StringBuilder();
        logBuilder.AppendLine("========== 权重算法验证结果 ==========");
        logBuilder.AppendLine($"总抽卡次数: {testCount}");
        logBuilder.AppendLine($"总权重: {allWeight}");
        logBuilder.AppendLine("--------------------------------------");
        
        foreach(var rarityData in rarityDatas){
            int count = countDict[rarityData.rarity];
            float actualPercentage = (float)count / testCount * 100f;
            float expectedPercentage = (float)rarityData.weight / allWeight * 100f;
            float difference = actualPercentage - expectedPercentage;
            
            logBuilder.AppendLine($"{rarityData.rarity}:");
            logBuilder.AppendLine($"  权重: {rarityData.weight} ({expectedPercentage:F2}%)");
            logBuilder.AppendLine($"  实际出现: {count}次 ({actualPercentage:F2}%)");
            logBuilder.AppendLine($"  差异: {difference:F2}%");
        }
        
        logBuilder.AppendLine("======================================");
        Debug.Log(logBuilder.ToString());
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if(GUILayout.Button("Refresh",GUILayout.Width(100),GUILayout.Height(100))){
            GetCardRarity(3);
        }
        if(GUILayout.Button("验证权重算法(100次)",GUILayout.Width(100),GUILayout.Height(100))){
            ValidateWeightAlgorithm();
        }
    }
#endif
}
