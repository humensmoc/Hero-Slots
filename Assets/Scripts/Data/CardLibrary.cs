using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType{
    Card_A,
    Card_B,
}

public class CardEffect{
    public CardType cardType;
    public Action<CardView> OnNearCardAttack;
    public Action OnGenerate;
    public Action OnDestroy;
    public Action OnInit;
    public Action<CardView> OnOtherCardAttack;
    public Action<CardView> OnOtherCardDestroy;
    public Action<CardView> OnOtherCardGenerate;

    public CardEffect(CardType cardType){
        this.cardType = cardType;
    }

    public CardEffect SetInitEvent(Action action){
        OnInit = action;
        return this;
    }

    public CardEffect SetGenerateEvent(Action action){
        OnGenerate = action;
        return this;
    }

    public CardEffect SetDestroyEvent(Action action){
        OnDestroy = action;
        return this;
    }

    public CardEffect SetNearCardAttackEvent(Action<CardView> action){
        OnNearCardAttack = action;
        return this;
    }
}

public static class CardLibrary
{
    public static List<CardEffect> cardEffects = new List<CardEffect>(){
    #region Card_A
        new CardEffect(CardType.Card_A)
            .SetInitEvent(() => {
                Debug.Log("Card_A Init");
            })
    #endregion
    };
}
