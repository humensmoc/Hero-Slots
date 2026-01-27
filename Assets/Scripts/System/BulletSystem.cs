using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EndTurnBlocker{
    MartialAttack,
    Electricity,
}

public class BulletSystem : Singleton<BulletSystem>
{
    public bool GetIsAllBulletDestroyed(){
        bool isAllBulletDestroyed=false;
        
        if(Model.BulletInBattlefield.Count==0&&Model.EndTurnBlockers.Count==0){
            isAllBulletDestroyed=true;
        }else{
            isAllBulletDestroyed=false;
        }

        return isAllBulletDestroyed;
    }
    public void Shot(BulletView bulletView,Vector3 dir)
    {
        bulletView.dir=dir;
    }
    public BulletView CreateBullet(Bullet bullet,Vector3 position,Quaternion rotation,CardView sourceCardView,EnemyView targetEnemy=null)
    {
        BulletView bulletView = BulletCreator.Instance.CreateBulletView(bullet,position,rotation,sourceCardView,targetEnemy);
        Model.BulletInBattlefield.Add(bulletView);
        return bulletView;
    }
}
