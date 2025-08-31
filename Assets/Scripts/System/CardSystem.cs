using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSystem : Singleton<CardSystem>
{
    public List<Card> cardsInDeck{get ; private set;}=new();
    public Card[,] cardsInBattlefield { get; private set; } = new Card[5, 5];
    public List<CardView> cardViews{get;private set;}=new();
    [SerializeField] float cardPositionInterval;
    [SerializeField] Transform cardParent;  
    public BattlefieldView battlefieldView;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawAllCardsGA>(DrawAllCardsPerformer);
        ActionSystem.AttachPerformer<RemoveAllCardsGA>(RemoveAllCardsPerformer);
        ActionSystem.SubscribeReaction<AllHeroShotGA>(AllHeroShotPostReaction_RemoveAllCards,ReactionTiming.POST);
        ActionSystem.SubscribeReaction<RemoveAllCardsGA>(RemoveAllCardsPostReaction_DrawAllCards,ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawAllCardsGA>();
        ActionSystem.DetachPerformer<RemoveAllCardsGA>();
        ActionSystem.UnsubscribeReaction<AllHeroShotGA>(AllHeroShotPostReaction_RemoveAllCards,ReactionTiming.POST);
        ActionSystem.UnsubscribeReaction<RemoveAllCardsGA>(RemoveAllCardsPostReaction_DrawAllCards,ReactionTiming.POST);
    }

    public void Init(List<CardData> cardDatas)
    {
        foreach (CardData cardData in cardDatas)
        {
            cardsInDeck.Add(new Card(cardData));
        }

        StartCoroutine(DrawAllCardsPerformer(new DrawAllCardsGA()));
    }


#region Performer
    /// <summary>
    /// 一次性全部抽卡，直到牌堆为空或战场满
    /// </summary>
    /// <param name="nextTurnGA"></param>
    /// <returns></returns>
    private IEnumerator DrawAllCardsPerformer(DrawAllCardsGA drawAllCardsGA)
    {
        yield return HeroSystem.Instance.DrawAllHero();
        
        // 检查cardsInDeck和cardsInBattlefield是否有效
        if (cardsInDeck == null || cardsInBattlefield == null)
            yield break;

        // 统计战场空位
        List<Vector2Int> emptySlots = new List<Vector2Int>();
        for (int x = 0; x < cardsInBattlefield.GetLength(0); x++)
        {
            for (int y = 0; y < cardsInBattlefield.GetLength(1); y++)
            {
                if (cardsInBattlefield[x, y] == null)
                {
                    emptySlots.Add(new Vector2Int(x, y));
                }
            }
        }

        // 如果没有空位，直接结束
        if (emptySlots.Count == 0)
            yield break;

        // 一直抽卡直到牌堆为空或战场满
        while (cardsInDeck.Count > 0 && emptySlots.Count > 0)
        {
            yield return DrawCard(emptySlots);
        }

        
    }

    /// <summary>
    /// 移除所有卡牌
    /// </summary>
    /// <param name="nextTurnGA"></param>
    /// <returns></returns>
    private IEnumerator RemoveAllCardsPerformer(RemoveAllCardsGA removeAllCardsGA){

        yield return HeroSystem.Instance.DiscardAllHero();

        for (int x = 0; x < cardsInBattlefield.GetLength(0); x++)
        {
            for (int y = 0; y < cardsInBattlefield.GetLength(1); y++)
            {
                if(cardsInBattlefield[x,y]==null)
                    continue;
                cardsInDeck.Add(cardsInBattlefield[x,y]);
                cardsInBattlefield[x,y] = null;
                cardViews.Remove(battlefieldView.cardViewsInBattlefield[x,y]);
                yield return battlefieldView.RemoveCard(x,y);
            }
        }

        
    }


#endregion

#region Reaction
    /// <summary>
    /// 下一回合开始前，移除所有卡牌
    /// </summary>
    /// <param name="nextTurnGA"></param>
    private void AllHeroShotPostReaction_RemoveAllCards(AllHeroShotGA allHeroShotGA){
        RemoveAllCardsGA removeAllCardsGA = new RemoveAllCardsGA();
        ActionSystem.Instance.AddReaction(removeAllCardsGA);
    }

    /// <summary>
    /// 下一回合开始后，抽取所有卡牌
    /// </summary>
    /// <param name="nextTurnGA"></param>
    private void RemoveAllCardsPostReaction_DrawAllCards(RemoveAllCardsGA removeAllCardsGA){
        DrawAllCardsGA drawAllCardsGA = new DrawAllCardsGA();
        ActionSystem.Instance.AddReaction(drawAllCardsGA);
    }
    
#endregion

    /// <summary>
    /// 单次抽卡，将一张牌放到一个空位上
    /// </summary>
    /// <param name="emptySlots">可用空位列表（会被移除已用空位）</param>
    private IEnumerator DrawCard(List<Vector2Int> emptySlots = null)
    {
        // 检查cardsInDeck和cardsInBattlefield是否有效
        if (cardsInDeck == null || cardsInBattlefield == null)
            yield break;

        // 如果没有传入空位列表，则重新统计
        bool needUpdateEmptySlots = false;
        if (emptySlots == null)
        {
            emptySlots = new List<Vector2Int>();
            needUpdateEmptySlots = true;
        }

        if (needUpdateEmptySlots)
        {
            for (int x = 0; x < cardsInBattlefield.GetLength(0); x++)
            {
                for (int y = 0; y < cardsInBattlefield.GetLength(1); y++)
                {
                    if (cardsInBattlefield[x, y] == null)
                    {
                        emptySlots.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        // 如果没有空位或没有牌可抽，直接结束
        if (emptySlots.Count == 0 || cardsInDeck.Count == 0)
            yield break;

        // 随机从牌堆抽一张
        int cardIndex = Random.Range(0, cardsInDeck.Count);
        Card card = cardsInDeck[cardIndex];
        cardsInDeck.RemoveAt(cardIndex);

        // 随机选择一个空位
        int slotIndex = Random.Range(0, emptySlots.Count);
        Vector2Int pos = emptySlots[slotIndex];
        cardsInBattlefield[pos.x, pos.y] = card;
        emptySlots.RemoveAt(slotIndex);

        CardView cardView = CardCreator.Instance.CreateCardView(card, cardParent.position, cardParent.rotation,pos.x,pos.y);
        cardViews.Add(cardView);
        battlefieldView.cardViewsInBattlefield[pos.x, pos.y] = cardView; 
        Tween tween = cardView.transform.DOLocalMove(cardParent.position + new Vector3(pos.x * cardPositionInterval, pos.y * cardPositionInterval, 0), 0.15f);
        yield return tween.WaitForCompletion();
    }

}
