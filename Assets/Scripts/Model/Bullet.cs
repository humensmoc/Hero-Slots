using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    public BulletData bulletData;
    public int Attack;
    public Sprite Image => bulletData.Image;
    public int Life ;

    public Bullet(BulletData bulletData){
        this.bulletData = bulletData;
        Attack = bulletData.Attack;
        Life = bulletData.Life;
    }
}
