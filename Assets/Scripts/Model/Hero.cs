using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Runtime.Serialization;

[Serializable]
public class Hero
{
    public HeroData heroData;
    public string Name;
    public string Description;
    public Sprite Image;
    public int Attack;
    public int MaxEnergy;
    public int Energy;
    public bool isSkillCharged=> Energy>=MaxEnergy;
    public ElementType ElementType;
    public Hero(HeroData heroData){
        this.heroData = heroData;
        Name = heroData.Name;
        Description = heroData.Description;
        Image = heroData.Image;
        Attack = heroData.Attack;
        MaxEnergy = heroData.MaxEnergy;
        Energy = 0;
        ElementType = heroData.ElementType;
    }

    
}
