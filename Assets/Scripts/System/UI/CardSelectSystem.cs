using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;


public class CardSelectSystem : Singleton<CardSelectSystem>
{
    public RarityData rarityData;
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
            
            CardData cardData=CardLibrary.GetCardDatasByRarity(DrawWithRarity.GetSingleCardRarity(rarityData)).Draw();
            cardDatas.Add(cardData);
            cardSelectPanelView.AddCardSelectItem(cardData);
        }

    }

    public void ManualRefresh(){
        if(Model.Coin<Model.RefreshCardCost){
            TipsController.Instance.ShowTips("Not enough coin");
            return;
        }

        InGameEconomySystem.Instance.SpendCoin(
            CoordinateConverter.UIToWorld(refreshButton.transform.position),
            Model.RefreshCardCost);

        Refresh();
    }

    public void SelectCard(CardData cardData){
        Model.CardsInDeck.Add(new Card(cardData));
        isSelectingCard = false;
        HideCardSelectView();
    }

    public void GetCardRarity(int cardCount){
        string rarityString="";
        for(int i=0;i<cardCount;i++){
            CardRarity rarity=DrawWithRarity.GetSingleCardRarity(rarityData);
            rarityString+=i+1+": ";
            rarityString+=rarity.ToString()+" ";
        }
        Debug.Log(rarityString);
    }

    /// <summary>
    /// 验证权重算法：抽100次卡，统计各稀有度比例并与权重比例对比
    /// </summary>
    public void ValidateWeightAlgorithm(){
        
        // 计算总权重
        int allWeight = 0;
        Dictionary<CardRarity, int> weightDict = new Dictionary<CardRarity, int>();
        foreach(var rarityData in rarityData.singleRarityDatas){
            allWeight += rarityData.weight;
            weightDict[rarityData.rarity] = rarityData.weight;
        }

        if(allWeight == 0){
            Debug.LogError("总权重为0，无法验证");
            return;
        }

        // 统计各稀有度出现次数
        Dictionary<CardRarity, int> countDict = new Dictionary<CardRarity, int>();
        foreach(var rarityData in rarityData.singleRarityDatas){
            countDict[rarityData.rarity] = 0;
        }

        // 抽100次卡
        for(int i = 0; i < testCount; i++){
            CardRarity rarity = DrawWithRarity.GetSingleCardRarity(rarityData);
            countDict[rarity]++;
        }

        // 输出验证结果
        System.Text.StringBuilder logBuilder = new System.Text.StringBuilder();
        logBuilder.AppendLine("========== 权重算法验证结果 ==========");
        logBuilder.AppendLine($"总抽卡次数: {testCount}");
        logBuilder.AppendLine($"总权重: {allWeight}");
        logBuilder.AppendLine("--------------------------------------");
        
        foreach(var rarityData in rarityData.singleRarityDatas){
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
