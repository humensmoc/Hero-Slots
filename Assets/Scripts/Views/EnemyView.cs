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
    [SerializeField] private Color originalColor;
    public int x;
    public int y;

    public void Init(Enemy enemy,int x,int y){
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
        
        Debug.Log($"Enemy {enemy.Name} took {damage} damage, health now: {enemy.Health}");
        
        if(enemy.Health <= 0){
            Debug.Log($"Enemy {enemy.Name} should be removed, calling RemoveEnemy");
            // EnemySystem.Instance.RemoveEnemy(enemy);

            if(EnemySystem.Instance.enemies.Contains(enemy)){
                EnemySystem.Instance.RemoveEnemy(enemy);
            }

            if(EnemySystem.Instance.enemyViews.Contains(this)){
                EnemySystem.Instance.enemyViews.Remove(this);
            }

            Destroy(this.gameObject);
        }
    }
}
