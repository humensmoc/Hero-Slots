using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleSystem : Singleton<BattleSystem>
{

    void OnEnable()
    {
        ActionSystem.AttachPerformer<AllCardShotGA>(AllCardShotPerformer);
        ActionSystem.AttachPerformer<AllHeroShotGA>(AllHeroShotPerformer);
        ActionSystem.AttachPerformer<BulletCollisionGA>(BulletCollisionPerformer);
        ActionSystem.SubscribeReaction<NextTurnGA>(NextTurnPostReaction_AllCardShot,ReactionTiming.POST);
        ActionSystem.SubscribeReaction<AllCardShotGA>(AllCardShotPostReaction_AllHeroShot,ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AllCardShotGA>();
        ActionSystem.DetachPerformer<AllHeroShotGA>();
        ActionSystem.DetachPerformer<BulletCollisionGA>();
        ActionSystem.UnsubscribeReaction<NextTurnGA>(NextTurnPostReaction_AllCardShot,ReactionTiming.POST);
        ActionSystem.UnsubscribeReaction<AllCardShotGA>(AllCardShotPostReaction_AllHeroShot,ReactionTiming.POST);
    }
#region Performer
    private IEnumerator AllCardShotPerformer(AllCardShotGA allCardShotGA){
        Debug.Log("AllCardShotPerformer");
        for(int x = 0; x < CardSystem.Instance.cardsInBattlefield.GetLength(0); x++){
            for(int y = 0; y < CardSystem.Instance.cardsInBattlefield.GetLength(1); y++){
                if(CardSystem.Instance.cardsInBattlefield[x,y]==null)
                    continue;

                Tween tw = CardSystem.Instance.battlefieldView.cardViews[x,y].transform.DOScale(1.1f,0.075f).OnComplete(()=>{
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.DOScale(1f,0.075f);
                });
                yield return tw.WaitForCompletion();

                Bullet bullet=new Bullet(GameInitializer.Instance.testBulletData);
                bullet.Attack=CardSystem.Instance.battlefieldView.cardViews[x,y].attack;
                Debug.Log("bullet.Attack:"+bullet.Attack);
                BulletView bulletView = BulletSystem.Instance.CreateBullet(
                    bullet,
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.position,
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.rotation);

                BulletSystem.Instance.Shot(
                    bulletView,
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.right*10);

                yield return null;
            }
        }
    }

    private IEnumerator AllHeroShotPerformer(AllHeroShotGA allHeroShotGA){
        for(int i=0;i<HeroSystem.Instance.heroViews.Count;i++){
            HeroView heroView = HeroSystem.Instance.heroViews[i];
            Tween tw = heroView.transform.DOScale(1.1f,0.075f).OnComplete(()=>{
                heroView.transform.DOScale(1f,0.075f);
            });
            yield return tw.WaitForCompletion();

            Tween tw2 = HeroSystem.Instance.heroViews[i].transform.DOShakePosition(0.1f,0.1f,10,90,false,true);
            yield return tw2.WaitForCompletion();
            
            Bullet bullet=new Bullet(GameInitializer.Instance.testBulletData);
            bullet.Attack=HeroSystem.Instance.heroViews[i].attack;

            Debug.Log("bullet.Attack:"+bullet.Attack);

            BulletView bulletView = BulletSystem.Instance.CreateBullet(
                bullet,
                HeroSystem.Instance.heroViews[i].transform.position,
                HeroSystem.Instance.heroViews[i].transform.rotation);

            BulletSystem.Instance.Shot(
                bulletView,
                HeroSystem.Instance.heroViews[i].transform.right*10);

        }
        yield return null;
    }

    private IEnumerator BulletCollisionPerformer(BulletCollisionGA bulletCollisionGA){
        
        yield return null;
    }
#endregion

#region Reaction
    private void AllCardShotPostReaction_AllHeroShot(AllCardShotGA allCardShotGA){
        AllHeroShotGA allHeroShotGA = new AllHeroShotGA();
        ActionSystem.Instance.AddReaction(allHeroShotGA);
    }

    private void NextTurnPostReaction_AllCardShot(NextTurnGA nextTurnGA){
        AllCardShotGA allCardShotGA = new AllCardShotGA();
        ActionSystem.Instance.AddReaction(allCardShotGA);

    }
#endregion

    public List<CardView> GetCardViewByYIndex(int yIndex){
        List<CardView> cardViews = new List<CardView>();
        for(int i=0;i<CardSystem.Instance.cardViews.Count;i++){
            if(CardSystem.Instance.cardViews[i].y==yIndex){
                cardViews.Add(CardSystem.Instance.cardViews[i]);
            }
        }
        return cardViews;
    }

    public List<HeroView> GetHeroViewByYIndex(int yIndex){
        List<HeroView> heroViews = new List<HeroView>();
        for(int i=0;i<HeroSystem.Instance.heroViews.Count;i++){
            if(HeroSystem.Instance.heroViews[i].y==yIndex){
                heroViews.Add(HeroSystem.Instance.heroViews[i]);
            }
        }
        return heroViews;
    }

    public List<EnemyView> GetEnemyViewByYIndex(int yIndex){
        List<EnemyView> enemyViews = new List<EnemyView>();
        for(int i=0;i<EnemySystem.Instance.enemyViews.Count;i++){
            if(EnemySystem.Instance.enemyViews[i].y==yIndex){
                enemyViews.Add(EnemySystem.Instance.enemyViews[i]);
            }
        }
        return enemyViews;
    }

}
