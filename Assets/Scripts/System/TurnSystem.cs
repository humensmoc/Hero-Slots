using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : Singleton<TurnSystem>
{
    [Header("RuntimeData")]
    public int currentTurn=0;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<NextTurnGA>(NextTurnPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<NextTurnGA>();
    }

#region Performer
    public IEnumerator NextTurnPerformer(NextTurnGA nextTurnGA){
        Debug.Log("Next Turn");
        currentTurn++;
        if(currentTurn==3||currentTurn==6){
            HeroSelectSystem.Instance.ShowHeroSelectView();
            HeroSelectSystem.Instance.Refresh();
        }
        yield return null;
    }
#endregion

#region Reaction

#endregion
}
