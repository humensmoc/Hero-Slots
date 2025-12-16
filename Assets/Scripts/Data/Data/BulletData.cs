using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletData{
    public int Attack;
    public int Life;
    public ElementType ElementType;
    public BulletName BulletNameEnum;
    public BulletMoveType BulletMoveType;

    public Action<BulletView> OnInit;
    public Action<BulletView,EnemyView> OnHitEnemy;

    public BulletData(BulletName bulletNameEnum){
        this.BulletNameEnum = bulletNameEnum;
    }

    public BulletData Clone(){
        BulletData clone = new BulletData(BulletNameEnum);
        clone.Attack=Attack;
        clone.Life=Life;
        clone.ElementType=ElementType;
        clone.BulletNameEnum=BulletNameEnum;
        clone.BulletMoveType=BulletMoveType;
        clone.OnInit = OnInit;
        clone.OnHitEnemy = OnHitEnemy;
        return clone;
    }

    public BulletData SetBulletMoveType(BulletMoveType bulletMoveType){
        BulletMoveType = bulletMoveType;
        return this;
    }

    public BulletData SetAttack(int attack){
        Attack = attack;
        return this;
    }

    public BulletData SetLife(int life){
        Life = life;
        return this;
    }

    public BulletData SetElementType(ElementType elementType){
        ElementType = elementType;
        return this;
    }

    public BulletData SetInitEvent(Action<BulletView> action){
        OnInit = action;
        return this;
    }

    public BulletData SetHitEnemyEvent(Action<BulletView,EnemyView> action){
        OnHitEnemy = action;
        return this;
    }
}
