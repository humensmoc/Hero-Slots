using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyView : MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] private SpriteRenderer image;  
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text damageCountdown;
    [SerializeField] private Color originalColor;
    public int x;
    public int y;

    public void Init(Enemy enemy,int x,int y){
        damageCountdown.text=" ";
        this.enemy = enemy;
        image.sprite = ResourcesLoader.LoadEnemySprite(enemy.Name);
        healthText.text = "HP:"+enemy.Health.ToString()+"/"+enemy.MaxHealth.ToString();
        originalColor = image.color;
        this.x = x;
        this.y = y;
    }

    public void Damage(int damage){
        image.DOColor(Color.white,0.1f).OnComplete(()=>{
            image.DOColor(originalColor,0.1f);
        });
        image.transform.DOShakePosition(0.1f,0.1f,10,90,false,true);

        enemy.Health -= damage;
        ObjectPool.Instance.CreateJumpingText(damage.ToString(),transform.position,JumpingTextType.Normal);
        healthText.text = "HP:"+enemy.Health.ToString()+"/"+enemy.MaxHealth.ToString();
        
        if(enemy.Health <= 0){
            Debug.Log($"Enemy {enemy.Name} should be removed, calling RemoveEnemy");

            Dead();
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.left);
        x++;

        damageCountdown.text=x+"/"+Model.enemyDamageX;

        if(x>=Model.enemyDamageX){
            Model.currentHealth--;
            if(Model.currentHealth<=0){
                Debug.Log("Game Over");
                EventSystem.Instance.OnGameLose?.Invoke();
            }
            Dead();
        }
    }

    public void Dead()
    {
        if(EnemySystem.Instance.enemyViews.Contains(this)){
            EnemySystem.Instance.RemoveEnemy(enemy);
        }
    }
}
