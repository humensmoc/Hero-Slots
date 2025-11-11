using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Card
{
    public CardData CardData;
    public string Name;
    public string Description;
    public Sprite Image;
    public int Attack;

    public ElementType ElementType;
    public BulletName BulletNameEnum;
    public CardName CardNameEnum;

    public Card(CardData cardData){
        this.CardData = cardData.Clone();
        Name = this.CardData.Name;
        Description = this.CardData.Description;
        Attack = this.CardData.Attack;

        ElementType = this.CardData.ElementType;
        CardNameEnum = this.CardData.CardNameEnum;
        BulletNameEnum = this.CardData.BulletNameEnum;
    }
}
