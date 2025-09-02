using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletView : MonoBehaviour
{
    public Bullet bullet;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text attackText;
    public Vector3 dir;

    public void Init(Bullet bullet){
        this.bullet = bullet;
        image.sprite = bullet.Image;
        attackText.text = bullet.Attack.ToString();
    }

    public void SetDirection(Vector3 dir){
        this.dir = dir;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        transform.Translate(dir * Time.deltaTime,Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy")){
            EnemyView enemyView = other.gameObject.GetComponent<EnemyView>();
                if(enemyView != null){
                    enemyView.Damage(bullet.Attack);
                    bullet.bulletData.BulletEffect.OnHitEnemy?.Invoke(this,enemyView);
                }
            
            bullet.Life--;
            if(bullet.Life <= 0){
                Destroy(this.gameObject);
            }
            
        }
    }
}
