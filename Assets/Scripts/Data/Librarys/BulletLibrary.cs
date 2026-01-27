using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum BulletName{
    Bullet_Normal,
    Bullet_Explode,
    Bullet_Transparent,
    Bullet_Bounce,
    Bullet_Martial,
    Bullet_Missile,
    Bullet_Mayhem,
    Bullet_Dart,
}


public static class BulletLibrary
{
    // 递归执行弹跳攻击，避免while循环中回调导致的死循环
    private static void ElectricBounce(BulletView bulletView, EnemyView fromEnemyView, int remainingBounces)
    {
        if (remainingBounces <= 0) return;
        
        EnemyView nearestEnemyView = EnemySystem.Instance.GetNearestEnemyView(fromEnemyView);
        
        // 如果找不到下一个目标敌人，结束弹跳
        if (nearestEnemyView == null) return;
        
        ObjectPool.Instance.CreateFlyingImageToTarget(
            fromEnemyView.transform.position, 
            nearestEnemyView.transform.position,
            ()=>{
                // 在回调执行时再次检查目标是否仍然存在（可能在飞行过程中被其他攻击击杀）
                if(nearestEnemyView != null && nearestEnemyView.gameObject != null && nearestEnemyView.gameObject.activeInHierarchy){
                    nearestEnemyView.Damage(bulletView.bullet.Attack);
                    ElectricBounce(bulletView, nearestEnemyView, remainingBounces - 1);
                }
                // 如果目标已经消失，则终止弹跳链
            }
        );
    }

    public static List<BulletData> bulletDatas = new List<BulletData>(){
        new BulletData(BulletName.Bullet_Normal)
            .SetAttack(1)
            .SetLife(1)
            .SetElementType(ElementType.Element_Fire)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Normal Init");
            }),

        new BulletData(BulletName.Bullet_Transparent)
            .SetAttack(1)
            .SetLife(3)
            .SetElementType(ElementType.Element_Fire)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Transparent Init");
            }),

        new BulletData(BulletName.Bullet_Explode)
            .SetAttack(1)
            .SetLife(1)
            .SetElementType(ElementType.Element_Fire)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Explode Init");
            })
            .SetHitEnemyEvent((bulletView,enemyView) => {
                // Debug.Log("Bullet_Explode Hit Enemy");
                if(enemyView.enemy.Health<bulletView.bullet.Attack){
                    List<EnemyView> attackedEnemyViews=new();
                    List<EnemyView> allEnemyViews=Model.EnemyViews;
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

        new BulletData(BulletName.Bullet_Bounce)
            .SetAttack(1)
            .SetLife(1)
            .SetElementType(ElementType.Element_Electricity)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Bounce Init");
            })
            .SetHitEnemyEvent((bulletView,enemyView) => {
                int bounceCount = Model.Electricity/2;
                
                // 递归执行弹跳逻辑，避免while循环死循环问题
                ElectricBounce(bulletView, enemyView, bounceCount);
                
            }),

        new BulletData(BulletName.Bullet_Martial)
            .SetAttack(1)
            .SetLife(1)
            .SetElementType(ElementType.Element_Fire)
            .SetInitEvent((bulletView) => {
                // Debug.Log("Bullet_Martial Init");
            })
            .SetHitEnemyEvent((bulletView,enemyView) => {
                // Debug.Log("Bullet_Martial Hit Enemy");
            }),
        
        new BulletData(BulletName.Bullet_Missile)
            .SetAttack(3)
            .SetLife(1)
            .SetElementType(ElementType.Element_Fire)
            .SetBulletMoveType(BulletMoveType.Closest)
            .SetHitEnemyEvent((bulletView,enemyView)=>{
            }),
        
        new BulletData(BulletName.Bullet_Mayhem)
            .SetAttack(1)
            .SetLife(1)
            .SetElementType(ElementType.Element_Fire)
            .SetBulletMoveType(BulletMoveType.Random)
            .SetHitEnemyEvent((bulletView,enemyView)=>{
            }),
        
        new BulletData(BulletName.Bullet_Dart)
            .SetAttack(1)
            .SetLife(1)
            .SetElementType(ElementType.Element_Fire)
            .SetBulletMoveType(BulletMoveType.SelectByCard)
            .SetHitEnemyEvent((bulletView,enemyView)=>{
            }),
    };

    public static BulletData GetBulletDataByName(BulletName name) {
        foreach (var data in BulletLibrary.bulletDatas) {
            if (data.BulletNameEnum == name) {
                return data;
            }
        }
        // Return the first bullet data as a default if not found
        return bulletDatas[0];
    }

}
