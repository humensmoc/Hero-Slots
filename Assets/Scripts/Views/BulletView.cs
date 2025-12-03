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
    public Vector3 dir;

    public void Init(Bullet bullet,CardView sourceCardView){
        this.bullet = bullet;
        this.sourceCardView = sourceCardView;
        image.sprite = ResourcesLoader.LoadBulletSprite(bullet.bulletData.BulletNameEnum.ToString());
        attackText.text = bullet.Attack.ToString();
    }

    public void SetDirection(Vector3 dir){
        this.dir = dir;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        transform.Translate(dir * Time.deltaTime*3,Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy")){
            EnemyView enemyView = other.gameObject.GetComponent<EnemyView>();
                if(enemyView != null){
                    enemyView.Damage(bullet.Attack,sourceCardView);
                    bullet.bulletData.OnHitEnemy?.Invoke(this,enemyView);
                }
            
            bullet.Life--;
            if(bullet.Life <= 0){
                Destroy(this.gameObject);
            }
            
        }
    }
}
