using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public CardData cardData;
    public string Name => cardData.Name;
    public string Description => cardData.Description;
    public Sprite Image => cardData.Image;
    public int Attack => cardData.Attack;

    public Card(CardData cardData){
        this.cardData = cardData;
    }
}
