using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType{
    Card_A,
    Card_B,
}

public enum HreoType{
    hero_Alpha,
}

public enum EnemyType{
    Enemy_Default,
}
public class CardDataLibrary : MonoBehaviour
{
    public static List<CardData> GetCardDatas(){
        List<CardData> cardDatas = new List<CardData>(){
        };
        return cardDatas;
    }
}
