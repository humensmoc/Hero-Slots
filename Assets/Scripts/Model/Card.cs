using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Card
{
    public CardData cardData;
    public string Name;
    public string Description;
    public Sprite Image;
    public int Attack;
    public ElementType ElementType;
    public Card(CardData cardData){
        this.cardData = cardData;
        Name = cardData.Name;
        Description = cardData.Description;
        Image = cardData.Image;
        Attack = cardData.Attack;
        ElementType = cardData.ElementType;
    }
}
