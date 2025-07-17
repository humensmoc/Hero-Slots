using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private readonly HeroData heroData;
    public string Name => heroData.Name;
    public string Description => heroData.Description;
    public Sprite Image => heroData.Image;
    public int Attack => heroData.Attack;

    public Hero(HeroData heroData){
        this.heroData = heroData;
    }
}
