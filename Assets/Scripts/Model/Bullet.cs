using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    private readonly BulletData bulletData;
    public int Attack => bulletData.Attack;
    public Sprite Image => bulletData.Image;

    public Bullet(BulletData bulletData){
        this.bulletData = bulletData;
    }
}
