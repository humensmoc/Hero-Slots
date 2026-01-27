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
    public HoverInfoPanelData hoverInfoPanelData;
    public BoxCollider boxCollider;
    public GameObject imageGameObject;
    public int x;
    public int y;

    public void Init(Enemy enemy,int x,int y){
        damageCountdown.text=" ";
        this.enemy = enemy;
        image.sprite = ResourcesLoader.LoadEnemySprite(enemy.enemyData.EnemyNameEnum.ToString());
        imageGameObject.transform.localPosition = new Vector3(0,0.6f*(enemy.enemyData.Length-1),0);
        boxCollider.size = new Vector3(1,enemy.enemyData.Length,1);
        boxCollider.center = new Vector3(0,0.6f*(enemy.enemyData.Length-1),0);
        hoverInfoPanelData = new HoverInfoPanelData(HoverInfoPanelType.Enemy, image.sprite, enemy.enemyData.EnemyNameEnum.ToString(), enemy.enemyData.Description);
        healthText.text = "HP:"+enemy.Health.ToString()+"/"+enemy.MaxHealth.ToString();
        originalColor = image.color;
        this.x = x;
        this.y = y;
    }

    public void Damage(int damage,CardView cardView=null){
        image.DOColor(Color.white,0.1f).OnComplete(()=>{
            image.DOColor(originalColor,0.1f);
        });
        image.transform.DOShakePosition(0.1f,0.1f,10,90,false,true);

        enemy.Health -= damage;
        ObjectPool.Instance.CreateJumpingText(damage.ToString(),transform.position,JumpingTextType.Normal);
        healthText.text = "HP:"+enemy.Health.ToString()+"/"+enemy.MaxHealth.ToString();

        if(cardView != null){
            DamageRankSystem.Instance.damageRankPanelView.AddDamageRankData(cardView,damage);
        }
        
        if(enemy.Health <= 0){
            Debug.Log($"Enemy {enemy.enemyData.EnemyNameEnum.ToString()} should be removed, calling RemoveEnemy");

            if(cardView != null){
                cardView.KillEnemy(this);
            }

            Dead();
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.left);
        x++;

        damageCountdown.text=x+"/"+Model.EnemyDamageTrun;

        if(x>=Model.EnemyDamageTrun){
            Model.CurrentHealth--;
            if(Model.CurrentHealth<=0){
                Debug.Log("Game Over");
                EventSystem.Instance.OnGameLose?.Invoke();
            }
            Dead();
        }
    }

    public void Dead()
    {
        if(Model.EnemyViews.Contains(this)){
            EnemySystem.Instance.RemoveEnemy(enemy);
        }

        GameObject enemyDeadBodyView = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyDeadBodyView"),transform.position,Quaternion.identity);
        
        enemyDeadBodyView.GetComponentInChildren<SpriteRenderer>().DOFade(0f,5f).OnComplete(()=>{
            Destroy(enemyDeadBodyView);
        });
    }

    void OnMouseEnter()
    {
        transform.DOScale(1.1f, 0.15f);
        HoverInfoController.Instance.MouseEnterTargetView(hoverInfoPanelData);
    }

    void OnMouseExit()
    {
        transform.DOScale(1f, 0.15f);
        HoverInfoController.Instance.MouseExitTargetView(hoverInfoPanelData);
    }

    void OnMouseDown()
    {
        Dead();
    }


}
