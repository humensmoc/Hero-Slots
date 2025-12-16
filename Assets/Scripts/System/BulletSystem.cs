using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EndTurnBlocker{
    MartialAttack,
    Electricity,
    Dart,
}

public class BulletSystem : Singleton<BulletSystem>
{
    public List<BulletView> bulletInBattlefield = new ();

    public List<EndTurnBlocker> endTurnBlockers = new ();


    public bool GetIsAllBulletDestroyed(){
        bool isAllBulletDestroyed=false;
        
        if(bulletInBattlefield.Count==0&&endTurnBlockers.Count==0){
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
    public BulletView CreateBullet(Bullet bullet,Vector3 position,Quaternion rotation,CardView sourceCardView)
    {
        BulletView bulletView = BulletCreator.Instance.CreateBulletView(bullet,position,rotation,sourceCardView);
        bulletInBattlefield.Add(bulletView);
        return bulletView;
    }
}
