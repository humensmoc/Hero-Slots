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
    public int MaxHealth{get;private set;}
    public int Health{get;private set;}

    public void Init(Enemy enemy){
        this.enemy = enemy;
        image.sprite = enemy.Image;
        MaxHealth = enemy.Health;
        Health = enemy.Health;
        healthText.text = "HP:"+Health.ToString()+"/"+MaxHealth.ToString();
        originalColor = image.color;
    }

    public void Damage(int damage){
        image.DOColor(Color.white,0.1f).OnComplete(()=>{
            image.DOColor(originalColor,0.1f);
        });
        image.transform.DOShakePosition(0.1f,0.1f,10,90,false,true);

        Health -= damage;
        healthText.text = "HP:"+Health.ToString()+"/"+MaxHealth.ToString();
        if(Health <= 0){
            Destroy(this.gameObject);
        }
    }
}
