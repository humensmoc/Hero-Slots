using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class BulletView : MonoBehaviour
{
    public Bullet bullet;
    [SerializeField] private SpriteRenderer image;
    public Vector3 dir;

    public void Init(Bullet bullet){
        this.bullet = bullet;
        image.sprite = bullet.Image;
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
            // BulletCollisionGA bulletCollisionGA = new BulletCollisionGA(bullet,other.gameObject,this);
            // ActionSystem.Instance.AddReaction(bulletCollisionGA);
            EnemyView enemyView = other.gameObject.GetComponent<EnemyView>();
                if(enemyView != null){
                    enemyView.Damage(bullet.Attack);
                }
            
            Destroy(this.gameObject);
        }
    }
}
