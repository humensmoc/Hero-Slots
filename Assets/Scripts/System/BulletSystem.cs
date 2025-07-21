using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletSystem : Singleton<BulletSystem>
{


    public void Shot(BulletView bulletView,Vector3 dir)
    {
        bulletView.dir = dir;
    }
    public BulletView CreateBullet(Bullet bullet,Vector3 position,Quaternion rotation)
    {
        return BulletCreator.Instance.CreateBulletView(bullet,position,rotation);
    }
}
