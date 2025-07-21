using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionGA : GameAction
{
    public Bullet Bullet{get;private set;}
    public GameObject Target{get;private set;}
    public BulletView BulletView{get;private set;}

    public BulletCollisionGA(Bullet bullet, GameObject target, BulletView bulletView){
        Bullet = bullet;
        Target = target;
        BulletView = bulletView;
    }
}
