using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData{
    public string Name;
    public string Description;
    public int Attack;
    public ElementType ElementType;
    public CardName CardNameEnum;
    public int CurrentCountdown;
    public int MaxCountdown;

    public Action OnEnter;
    public Func<CardView, IEnumerator> OnTurnStart;
    public Action OnLeave;
    public Func<CardView, IEnumerator> OnAttack;
    public Action<CardView> OnInit;
    public Func<CardView, IEnumerator> OnCountdownEnd;

    public CardData Clone(){
        CardData clone = new CardData(CardNameEnum);
        clone.Name = Name;
        clone.Description = Description;
        clone.Attack = Attack;
        clone.CurrentCountdown = CurrentCountdown;
        clone.MaxCountdown = MaxCountdown;
        clone.ElementType = ElementType;
        clone.CardNameEnum = CardNameEnum;
        clone.OnInit = OnInit;
        clone.OnEnter = OnEnter;
        clone.OnLeave = OnLeave;
        clone.OnAttack = OnAttack;
        clone.OnTurnStart = OnTurnStart;
        clone.OnCountdownEnd = OnCountdownEnd;
        return clone;
    }

    public CardData(CardName cardNameEnum){
        this.CardNameEnum = cardNameEnum;
    }

    public CardData SetDescription(string description){
        Description = description;
        return this;
    }

    public CardData SetAttack(int attack){
        Attack = attack;
        return this;
    }

    public CardData SetElementType(ElementType elementType){
        ElementType = elementType;
        return this;
    }

    public CardData SetOnInit(Action<CardView> action){
        OnInit = action;
        return this;
    }

    public CardData SetOnEnter(Action action){
        OnEnter = action;
        return this;
    }

    public CardData SetOnLeave(Action action){
        OnLeave = action;
        return this;
    }

    public CardData SetOnAttack(Func<CardView, IEnumerator> action){
        OnAttack = action;
        return this;
    }

    public CardData SetOnTurnStart(Func<CardView, IEnumerator> action){
        OnTurnStart = action;
        return this;
    }

    public CardData SetOnCountdownEnd(int maxCountdown,Func<CardView, IEnumerator> action){
        MaxCountdown = maxCountdown;
        CurrentCountdown = 0;
        OnCountdownEnd = action;
        return this;
    }
}

