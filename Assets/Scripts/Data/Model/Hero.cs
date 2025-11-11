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
    public int Attack;
    public int MaxEnergy;
    public int Energy;
    public bool isSkillCharged=> Energy>=MaxEnergy;
    public int value_1;
    public HeroType HeroType;
    public ElementType ElementType;
    public BulletName BulletNameEnum;
    public Hero(HeroData heroData){
        this.heroData = heroData.Clone();
        Name = this.heroData.Name;
        Description = this.heroData.Description;
        Attack = this.heroData.Attack;
        MaxEnergy = this.heroData.MaxEnergy;
        Energy = 0;
        ElementType = this.heroData.ElementType;
        HeroType = this.heroData.HeroType;
        BulletNameEnum=this.heroData.BulletNameEnum;
    }

    
}
