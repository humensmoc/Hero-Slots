using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] List<CardData> testCardDatas;
    public List<Card> cardsInDeck{get ; private set;}=new();
    public Card[,] cardsInBattlefield { get; private set; } = new Card[5, 5];
    [SerializeField] float cardPositionInterval = 0.15f;
    [SerializeField] Transform cardParent;
    [SerializeField] BattlefieldView battlefieldView;
    private void Start()
    {
        Init(testCardDatas);
        StartCoroutine(DrawAllCardS());
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,50),"Draw")){
            StartCoroutine(DrawAllCardS());
        }
        if(GUI.Button(new Rect(10,70,100,50),"Remove")){
            StartCoroutine(RemoveAllCardS());
        }
    }

    public void Init(List<CardData> cardDatas)
    {
        foreach (CardData cardData in cardDatas)
        {
            cardsInDeck.Add(new Card(cardData));
        }
    }

    /// <summary>
    /// 一次性全部抽卡，直到牌堆为空或战场满
    /// </summary>
    private IEnumerator DrawAllCardS()
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

    private IEnumerator RemoveAllCardS(){
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
