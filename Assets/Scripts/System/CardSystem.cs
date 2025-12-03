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
        cardsInDeck.Add(new Card(cardDatas[Random.Range(0, cardDatas.Count)]));
        cardsInDeck.Add(new Card(cardDatas[Random.Range(0, cardDatas.Count)]));
        cardsInDeck.Add(new Card(cardDatas[Random.Range(0, cardDatas.Count)]));

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

        List<Card> cardsToAdd = new List<Card>();
        if(cardsInDeck.Count > 0){
            foreach(Card card in cardsInDeck){
                cardsToAdd.Add(card);
            }
        }
        // 一直抽卡直到牌堆为空或战场满
        while (cardsToAdd.Count > 0 && emptySlots.Count > 0)
        {
            yield return DrawCard(emptySlots,cardsToAdd);
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
                // cardsInDeck.Add(cardsInBattlefield[x,y]);
                cardsInBattlefield[x,y].CardData.OnLeave?.Invoke();
                cardsInBattlefield[x,y] = null;

                

                cardViews.Remove(battlefieldView.cardViewsInBattlefield[x,y]);
                yield return battlefieldView.RemoveCard(x,y);
            }
        }

        if(!CardSelectSystem.Instance.gameObject.activeSelf)CardSelectSystem.Instance.gameObject.SetActive(true);
        CardSelectSystem.Instance.ShowCardSelectView();
        CardSelectSystem.Instance.Refresh();
        
        yield return new WaitUntil(()=>!CardSelectSystem.Instance.isSelectingCard);
        // yield return new WaitUntil(()=>!HeroSelectSystem.Instance.isSelectingHero);
        
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

    public void DeleteCardInBattleField(CardView cardView){
        cardsInDeck.Remove(cardView.card);
        cardsInBattlefield[cardView.x,cardView.y] = null;
        cardViews.Remove(cardView);
        Destroy(cardView.gameObject);
        battlefieldView.cardViewsInBattlefield[cardView.x,cardView.y] = null;
    }

    public void DeleteCardInDeck(Card card){
        bool isCardInBattlefield = false;
        CardView cardViewInBattlefield = null;

        foreach(CardView cardView in cardViews){
            if(cardView.card == card){
                isCardInBattlefield = true;
                cardViewInBattlefield = cardView;
                break;
            }
        }

        if(isCardInBattlefield){
            DeleteCardInBattleField(cardViewInBattlefield);
        }else{
            cardsInDeck.Remove(card);
        }
    }

    /// <summary>
    /// 单次抽卡，将一张牌放到一个空位上
    /// </summary>
    /// <param name="emptySlots">可用空位列表（会被移除已用空位）</param>
    private IEnumerator DrawCard(List<Vector2Int> emptySlots = null,List<Card> cardsToAdd = null)
    {
        // 检查cardsInDeck和cardsInBattlefield是否有效
        if (cardsToAdd == null || cardsInBattlefield == null)
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
        if (emptySlots.Count == 0 || cardsToAdd.Count == 0)
            yield break;

        // 随机从牌堆抽一张
        int cardIndex = Random.Range(0, cardsToAdd.Count);
        Card card = cardsToAdd[cardIndex];
        cardsToAdd.RemoveAt(cardIndex);

        // 随机选择一个空位
        int slotIndex = Random.Range(0, emptySlots.Count);
        Vector2Int pos = emptySlots[slotIndex];
        cardsInBattlefield[pos.x, pos.y] = card;
        emptySlots.RemoveAt(slotIndex);

        CardView cardView = CardCreator.Instance.CreateCardView(card, cardParent.position, cardParent.rotation,pos.x,pos.y);
        cardViews.Add(cardView);
        battlefieldView.cardViewsInBattlefield[pos.x, pos.y] = cardView; 
        Tween tween = cardView.transform.DOLocalMove(cardParent.position + new Vector3(pos.x * cardPositionInterval, pos.y * cardPositionInterval, 0), 0.05f);
        yield return tween.WaitForCompletion();
    }

    public CardView GetCardView(int x,int y){
        return battlefieldView.cardViewsInBattlefield[x,y];
    }

    public List<CardView> GetAllNeighborCardView(CardView cardView){
        List<CardView> cardViews = new List<CardView>();
        // 获取cardView相邻的四个方向(x-1,y), (x+1,y), (x,y-1), (x,y+1)的CardView，排除越界情况
        int x = cardView.x;
        int y = cardView.y;
        int[,] directions = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int nx = x + directions[i, 0];
            int ny = y + directions[i, 1];
            if (nx >= 0 && nx < 5 && ny >= 0 && ny < 5)
            {
                CardView neighbor = battlefieldView.cardViewsInBattlefield[nx, ny];
                if (neighbor != null)
                {
                    cardViews.Add(neighbor);
                }
            }
        }
        return cardViews;
    }

    public CardView GetRandomOneNeighborCardView(CardView cardView){
        List<CardView> cardViews = GetAllNeighborCardView(cardView);
        if(cardViews.Count == 0)
            return null;
        return cardViews[Random.Range(0, cardViews.Count)];
    }

    public CardView GetRandomCardView(){
        return cardViews[Random.Range(0, cardViews.Count)];
    }

    public CardView GetRandomCardViewNotSelf(CardView cardView){
        List<CardView> cardViews = new List<CardView>();
        for(int i=0;i<this.cardViews.Count;i++){
            if(this.cardViews[i]!=cardView){
                cardViews.Add(this.cardViews[i]);
            }
        }
        return cardViews[Random.Range(0, cardViews.Count)];
    }

    public void Reset()
    {
        cardsInDeck.Clear();
        cardsInBattlefield = new Card[5, 5];
        battlefieldView.cardViewsInBattlefield = new CardView[5, 5];
        for(int i=0;i<5;i++){
            for(int j=0;j<5;j++){
                battlefieldView.cardViewsInBattlefield[i,j] = null;
            }
        }

        foreach(var cardView in cardViews){
            Destroy(cardView.gameObject);
        }
        cardViews.Clear();
    }
}
