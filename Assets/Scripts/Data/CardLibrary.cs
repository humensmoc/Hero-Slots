using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardName{
    Card_A,
    Card_B,
    Card_C,
}

public class CardEffect{
    public CardName CardNameEnum;
    public Action<CardView> OnNearCardAttack;
    public Action OnGenerate;
    public Action OnDestroy;
    public Action<CardView> OnInit;
    public Action<CardView> OnOtherCardAttack;
    public Action<CardView> OnOtherCardDestroy;
    public Action<CardView> OnOtherCardGenerate;

    public CardEffect Clone(){
        CardEffect clone = new CardEffect(CardNameEnum);
        clone.OnInit = OnInit;
        clone.OnGenerate = OnGenerate;
        clone.OnDestroy = OnDestroy;
        clone.OnNearCardAttack = OnNearCardAttack;
        clone.OnOtherCardAttack = OnOtherCardAttack;
        clone.OnOtherCardDestroy = OnOtherCardDestroy;
        clone.OnOtherCardGenerate = OnOtherCardGenerate;
        return clone;
    }

    public CardEffect(CardName cardNameEnum){
        this.CardNameEnum = cardNameEnum;
    }

    public CardEffect SetInitEvent(Action<CardView> action){
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
        new CardEffect(CardName.Card_A)
            .SetInitEvent((cardView) => {
                // Debug.Log("Card_A Init");
            }),
    #endregion

    #region Card_B
        new CardEffect(CardName.Card_B)
            .SetInitEvent((cardView) => {
                // Debug.Log("Card_B Init");
            }),
    #endregion

    #region Card_C
        new CardEffect(CardName.Card_C)
            .SetInitEvent((cardView) => {
                // Debug.Log("Card_C Init");
            }),
    #endregion
    };
}
