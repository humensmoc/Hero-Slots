using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName ="Data/Hero")]
public class ScriptableHeroData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField][field: TextArea(3,10)] public string Description {get; private set;}
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public int Attack {get; private set;}
    [field: SerializeField] public HeroType HeroType {get; private set;}
    [field: SerializeField] public int MaxEnergy {get; private set;}
    [field: SerializeField] public ElementType ElementType {get; private set;}
    [field: SerializeField] public HeroEffect HeroEffect ;
}

public class HeroData{
    public string Name;
    public string Description;
    public int Attack;
    public int MaxEnergy;

    public ElementType ElementType;
    public HeroType HeroType;

    public Action<HeroView> OnInit;
    public Action<HeroView> OnDead;
    public Action<int,int> OnMove;
    public Action<CardView,HeroView> OnGetCharged;
    public Action<HeroView> OnSkill;
    public Action<int,int> OnOtherHeroMove;
    public Action<HeroView> OnOtherHeroAttack;
    public Action<CardView> OnCardGenerate;
    public Action<CardView> OnCardAttack;

    public HeroData(HeroType heroType){
        this.HeroType = heroType;
    }

    public HeroData Clone(){
        HeroData clone = new HeroData(HeroType);
        clone.Name=Name;
        clone.Description=Description;
        clone.Attack=Attack;
        clone.MaxEnergy=MaxEnergy;

        clone.ElementType=ElementType;
        clone.HeroType=HeroType;

        clone.OnInit=OnInit;
        clone.OnDead=OnDead;
        clone.OnSkill=OnSkill;
        clone.OnOtherHeroAttack=OnOtherHeroAttack;
        clone.OnCardGenerate=OnCardGenerate;
        clone.OnCardAttack=OnCardAttack;
        clone.OnMove=OnMove;
        clone.OnGetCharged=OnGetCharged;
        clone.OnOtherHeroMove=OnOtherHeroMove;

        return clone;
    }

    public HeroData SetName(string name){
        Name = name;
        return this;
    }

    public HeroData SetDescription(string description){
        Description = description;
        return this;
    }

    public HeroData SetAttack(int attack){
        Attack = attack;
        return this;
    }

    public HeroData SetMaxEnergy(int maxEnergy){
        MaxEnergy = maxEnergy;
        return this;
    }

    public HeroData SetElementType(ElementType elementType){
        ElementType = elementType;
        return this;
    }

    public HeroData SetInitEvent(Action<HeroView> action){
        OnInit = action;
        return this;
    }

    public HeroData SetSkillEvent(Action<HeroView> action){
        OnSkill = action;
        return this;
    }
}
