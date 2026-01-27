using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

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
        // Debug.Log("AllCardShotPerformer");

        //OnTurnStart
        for(int x = 0; x < Model.CardsInBattlefield.GetLength(0); x++){
            for(int y = 0; y < Model.CardsInBattlefield.GetLength(1); y++){
                if(Model.CardsInBattlefield[y,x]==null)
                    continue;
                
                if(CardSystem.Instance.battlefieldView.cardViewsInBattlefield[y,x].card.CardData.OnTurnStart!=null){
                    yield return CardSystem.Instance.battlefieldView.cardViewsInBattlefield[y,x]
                        .card.CardData.OnTurnStart(CardSystem.Instance.battlefieldView.cardViewsInBattlefield[y,x]);
                }
            }
        }
    
        //OnAttack
        for(int x = 0; x < Model.CardsInBattlefield.GetLength(0); x++){
            for(int y = 0; y < Model.CardsInBattlefield.GetLength(1); y++){
                if(Model.CardsInBattlefield[y,x]==null)
                    continue;
                
                yield return CardSystem.Instance.battlefieldView.cardViewsInBattlefield[y,x].Shot();
            }
        }
    }

    private IEnumerator AllHeroShotPerformer(AllHeroShotGA allHeroShotGA){
        for(int i=0;i<Model.HeroViews.Count;i++){
            HeroView heroView = Model.HeroViews[i];
            // 检查HeroView是否仍然有效
            if(heroView != null && heroView.gameObject != null){
                yield return heroView.Shot();
            }
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
        for(int i=0;i<Model.CardViews.Count;i++){
            if(Model.CardViews[i].y==yIndex){
                cardViews.Add(Model.CardViews[i]);
            }
        }
        return cardViews;
    }

    public List<HeroView> GetHeroViewByYIndex(int yIndex){
        List<HeroView> heroViews = new List<HeroView>();
        for(int i=0;i<Model.HeroViews.Count;i++){
            if(Model.HeroViews[i].y==yIndex){
                heroViews.Add(Model.HeroViews[i]);
            }
        }
        return heroViews;
    }

    public List<EnemyView> GetEnemyViewByYIndex(int yIndex){
        List<EnemyView> enemyViews = new List<EnemyView>();
        for(int i=0;i<Model.EnemyViews.Count;i++){
            if(Model.EnemyViews[i].y==yIndex){
                enemyViews.Add(Model.EnemyViews[i]);
            }
        }
        return enemyViews;
    }

}
