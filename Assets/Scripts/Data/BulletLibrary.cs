using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletName{
    Bullet_Normal,
    Bullet_Explode,
    Bullet_Transparent,
    Bullet_C,
}

public class BulletEffect{
    public BulletName BulletNameEnum;
    public Action<BulletView> OnInit;
    public Action<BulletView,EnemyView> OnHitEnemy;

    public BulletEffect(BulletName bulletNameEnum){
        this.BulletNameEnum = bulletNameEnum;
    }

    public BulletEffect Clone(){
        BulletEffect clone = new BulletEffect(BulletNameEnum);
        clone.OnInit = OnInit;
        clone.OnHitEnemy = OnHitEnemy;
        return clone;
    }

    public BulletEffect SetInitEvent(Action<BulletView> action){
        OnInit = action;
        return this;
    }

    public BulletEffect SetHitEnemyEvent(Action<BulletView,EnemyView> action){
        OnHitEnemy = action;
        return this;
    }
}

public static class BulletLibrary
{
    public static List<BulletEffect> bulletEffects = new List<BulletEffect>(){
    #region Bullet_Normal
        new BulletEffect(BulletName.Bullet_Normal)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Normal Init");
            })
            .SetHitEnemyEvent((bulletView,enemyView) => {
                // Debug.Log("Bullet_Normal Hit Enemy");
            }),
    #endregion

    #region Bullet_Transparent

        new BulletEffect(BulletName.Bullet_Transparent)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Transparent Init");
            })
            .SetHitEnemyEvent((bulletView,enemyView) => {
                // Debug.Log("Bullet_Transparent Hit Enemy");
            }),
    #endregion

    #region Bullet_Explode
        new BulletEffect(BulletName.Bullet_Explode)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Explode Init");
            })
            .SetHitEnemyEvent((bulletView,enemyView) => {
                // Debug.Log("Bullet_Explode Hit Enemy");
                if(enemyView.enemy.Health<bulletView.bullet.Attack){
                    List<EnemyView> attackedEnemyViews=new();
                    List<EnemyView> allEnemyViews=EnemySystem.Instance.enemyViews;
                     EnemyView nearstNewEnemyView=null;
                    int extraAttack=bulletView.bullet.Attack-enemyView.enemy.Health;
                    
                    //explode the bullet
                    while(extraAttack>0){
                        if(allEnemyViews.Count>0){
                            nearstNewEnemyView = null; // 重置寻找目标

                            //find the nearest enemy view
                            foreach(EnemyView EV in allEnemyViews){
                                if(EV != enemyView && !attackedEnemyViews.Contains(EV)){
                                    if(nearstNewEnemyView == null){
                                        nearstNewEnemyView = EV;
                                    }else{
                                        float dist1 = Vector3.Distance(enemyView.transform.position, EV.transform.position);
                                        float dist2 = Vector3.Distance(enemyView.transform.position, nearstNewEnemyView.transform.position);
                                        if(dist1 < dist2){
                                            nearstNewEnemyView = EV;
                                        }
                                    }
                                }
                            }
                            
                            if(nearstNewEnemyView != null){
                                int enemyHealth = nearstNewEnemyView.enemy.Health;
                                int actualDamage = Mathf.Min(extraAttack, enemyHealth); // 计算实际造成的伤害
                                nearstNewEnemyView.Damage(actualDamage);
                                attackedEnemyViews.Add(nearstNewEnemyView);
                                allEnemyViews.Remove(nearstNewEnemyView);
                                extraAttack -= actualDamage; // 减去实际造成的伤害
                            }else{
                                // 没有可攻击的敌人了，退出循环
                                break;
                            }
                        }else{
                            // 没有敌人了，退出循环
                            break;
                        }
                    }

                }
            }),
    #endregion
    };
}
