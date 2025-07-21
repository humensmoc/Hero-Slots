using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSystem : Singleton<CardSystem>
{
    public List<Card> cardsInDeck{get ; private set;}=new();
    public Card[,] cardsInBattlefield { get; private set; } = new Card[5, 5];
    [SerializeField] float cardPositionInterval = 0.15f;
    [SerializeField] Transform cardParent;  
    public BattlefieldView battlefieldView;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawAllCardsGA>(DrawAllCardsPerformer);
        ActionSystem.AttachPerformer<RemoveAllCardsGA>(RemoveAllCardsPerformer);
        ActionSystem.SubscribeReaction<NextTurnGA>(NextTurnPreReaction,ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<NextTurnGA>(NextTurnPostReaction,ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawAllCardsGA>();
        ActionSystem.DetachPerformer<RemoveAllCardsGA>();
        ActionSystem.UnsubscribeReaction<NextTurnGA>(NextTurnPreReaction,ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<NextTurnGA>(NextTurnPostReaction,ReactionTiming.POST);
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,50),"Next Turn")){
            ActionSystem.Instance.Perform(new NextTurnGA());
        }
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
        for (int x = 0; x < cardsInBattlefield.GetLength(0); x++)
        {
            for (int y = 0; y < cardsInBattlefield.GetLength(1); y++)
            {
                if(cardsInBattlefield[x,y]==null)
                    continue;
                cardsInDeck.Add(cardsInBattlefield[x,y]);
                cardsInBattlefield[x,y] = null;
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
    private void NextTurnPreReaction(NextTurnGA nextTurnGA){
        RemoveAllCardsGA removeAllCardsGA = new RemoveAllCardsGA();
        ActionSystem.Instance.AddReaction(removeAllCardsGA);
    }

    /// <summary>
    /// 下一回合开始后，抽取所有卡牌
    /// </summary>
    /// <param name="nextTurnGA"></param>
    private void NextTurnPostReaction(NextTurnGA nextTurnGA){
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

        CardView cardView = CardCreator.Instance.CreateCardView(card, cardParent.position, cardParent.rotation);
        battlefieldView.cardViews[pos.x, pos.y] = cardView; 
        Tween tween = cardView.transform.DOLocalMove(cardParent.position + new Vector3(pos.x * cardPositionInterval, pos.y * cardPositionInterval, 0), 0.15f);
        yield return tween.WaitForCompletion();
    }

}
