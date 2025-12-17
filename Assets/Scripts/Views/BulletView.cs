using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletView : MonoBehaviour
{
    public Bullet bullet;
    public CardView sourceCardView;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text attackText;
    public Vector3 targetPosition;
    public EnemyView targetEnemy;
    public Vector3 dir;

    public void Init(Bullet bullet,CardView sourceCardView,EnemyView TargetEnemy=null){
        this.bullet = bullet;
        this.sourceCardView = sourceCardView;
        image.sprite = ResourcesLoader.LoadBulletSprite(bullet.bulletData.BulletNameEnum.ToString());
        attackText.text = bullet.Attack.ToString();
        SetTarget(bullet.BulletMoveType,TargetEnemy);

    }

    /// <summary>
    /// 设置子弹目标
    /// </summary>
    /// <param name="bulletMoveType"></param>
    /// <param name="targetEnemySelectByCard"></param>
    public void SetTarget(BulletMoveType bulletMoveType,EnemyView targetEnemySelectByCard=null){
        switch(bulletMoveType){
            case BulletMoveType.Normal:
                // Normal类型通过碰撞检测命中敌人，不需要targetEnemy
                targetPosition=Vector3.zero;
                targetEnemy = null; // 允许为null，通过OnTriggerEnter检测碰撞
                break;
            case BulletMoveType.Closest:
                EnemyView closestEnemy = EnemySystem.Instance.GetClosestEnemyView();
                if(closestEnemy != null){
                    targetEnemy = closestEnemy;
                    targetPosition = closestEnemy.transform.position;
                } else {
                    targetEnemy = null;
                    targetPosition = Vector3.zero;
                }
                break;
            case BulletMoveType.Random:
                EnemyView randomEnemy = EnemySystem.Instance.GetRandomEnemyView();
                if(randomEnemy != null){
                    targetEnemy = randomEnemy;
                    targetPosition = randomEnemy.transform.position;
                } else {
                    targetEnemy = null;
                    targetPosition = Vector3.zero;
                }
                break;
            case BulletMoveType.SelectByCard:
                if(targetEnemySelectByCard != null){
                    targetEnemy = targetEnemySelectByCard;
                    targetPosition = targetEnemySelectByCard.transform.position;
                } else {
                    targetEnemy = null;
                    targetPosition = Vector3.zero;
                }
                break;
            case BulletMoveType.Martial:
                targetPosition=Vector3.zero;
                targetEnemy = null;
                break;
        }
    }

    public void SetDirection(Vector3 dir){
        this.dir = dir;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        // Normal类型通过碰撞检测命中，不需要targetEnemy检查
        if(bullet.BulletMoveType != BulletMoveType.Normal && targetEnemy==null){
            Dead();
            return;
        }
        
        // 对于有目标位置的子弹类型，检查是否到达目标
        if(targetPosition!=Vector3.zero && targetEnemy != null && Vector3.Distance(transform.position,targetPosition)<=0.1f){
            OnGetTarget(targetEnemy);
            Dead();
        }

        Move();
    }

    public void Move(){
        switch(bullet.BulletMoveType){
            case BulletMoveType.Normal:
                transform.Translate(dir.normalized * Time.deltaTime*20,Space.World);
                break;
            case BulletMoveType.Closest:
                transform.Translate((targetPosition - transform.position).normalized * Time.deltaTime*20,Space.World);
                break;
            case BulletMoveType.Random:
                transform.Translate((targetPosition - transform.position).normalized * Time.deltaTime*20,Space.World);
                break;
            case BulletMoveType.SelectByCard:
                transform.Translate((targetPosition - transform.position).normalized * Time.deltaTime*20,Space.World);
                break;
        }
    }

    //到达目标
    public void OnGetTarget(EnemyView enemyView){
        if(enemyView!=null){
            enemyView.Damage(bullet.Attack,sourceCardView);
            bullet.bulletData.OnHitEnemy?.Invoke(this,enemyView);
            // 在cardView上启动协程，避免BulletView被销毁后协程被中断
            if(sourceCardView != null && sourceCardView.gameObject != null){
                sourceCardView.StartCoroutine(EventSystem.Instance.CheckEvent(new EventInfo(sourceCardView,EventType.OnBulletHitEnemy,enemyView,this)));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy")){
            EnemyView enemyView = other.gameObject.GetComponent<EnemyView>();
            OnGetTarget(enemyView);
            
            bullet.Life--;
            if(bullet.Life <= 0){
                Dead();
            }
            
        }

        if(other.gameObject.CompareTag("Void")){
            Dead();
        }
    }

    public void Dead(){
        BulletSystem.Instance.bulletInBattlefield.Remove(this);
        Destroy(this.gameObject);
    }
}
