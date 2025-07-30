using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class BattleSystem : Singleton<BattleSystem>
{
    public Action<CardView> OnCardAttack;

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
        // Debug.Log("AllCardShotPerformer");
        for(int x = 0; x < CardSystem.Instance.cardsInBattlefield.GetLength(0); x++){
            for(int y = 0; y < CardSystem.Instance.cardsInBattlefield.GetLength(1); y++){
                if(CardSystem.Instance.cardsInBattlefield[x,y]==null)
                    continue;
                
                yield return CardSystem.Instance.battlefieldView.cardViews[x,y].Shot();
            }
        }
    }

    private IEnumerator AllHeroShotPerformer(AllHeroShotGA allHeroShotGA){
        for(int i=0;i<HeroSystem.Instance.heroViews.Count;i++){
            HeroView heroView = HeroSystem.Instance.heroViews[i];
            yield return heroView.Shot();

        }
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
