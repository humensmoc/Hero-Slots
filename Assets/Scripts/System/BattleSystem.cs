using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : Singleton<BattleSystem>
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<AllCardShotGA>(AllCardShotPerformer);
        ActionSystem.AttachPerformer<BulletCollisionGA>(BulletCollisionPerformer);
        ActionSystem.SubscribeReaction<NextTurnGA>(NextTurnPreReaction,ReactionTiming.PRE);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<AllCardShotGA>();
        ActionSystem.DetachPerformer<BulletCollisionGA>();
        ActionSystem.UnsubscribeReaction<NextTurnGA>(NextTurnPreReaction,ReactionTiming.PRE);
    }

    private IEnumerator AllCardShotPerformer(AllCardShotGA allCardShotGA){
        Debug.Log("AllCardShotPerformer");
        for(int x = 0; x < CardSystem.Instance.cardsInBattlefield.GetLength(0); x++){
            for(int y = 0; y < CardSystem.Instance.cardsInBattlefield.GetLength(1); y++){
                if(CardSystem.Instance.cardsInBattlefield[x,y]==null)
                    continue;
                
                BulletView bulletView = BulletSystem.Instance.CreateBullet(
                    new Bullet(GameInitializer.Instance.testBulletData),
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.position,
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.rotation);

                BulletSystem.Instance.Shot(
                    bulletView,
                    CardSystem.Instance.battlefieldView.cardViews[x,y].transform.right*10);

                yield return null;
            }
        }
    }

    private IEnumerator BulletCollisionPerformer(BulletCollisionGA bulletCollisionGA){
        
        yield return null;
    }

    private void NextTurnPreReaction(NextTurnGA nextTurnGA){
        AllCardShotGA allCardShotGA = new AllCardShotGA();
        ActionSystem.Instance.AddReaction(allCardShotGA);
    }
}
