using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    public BulletData bulletData;
    public int Attack;
    public int Life;
    public ElementType ElementType;
    public BulletMoveType BulletMoveType;

    public Bullet(BulletData bulletData){
        this.bulletData = bulletData.Clone();
        Attack = this.bulletData.Attack;
        Life = this.bulletData.Life;
        ElementType = this.bulletData.ElementType;
        BulletMoveType = this.bulletData.BulletMoveType;
    }
}
