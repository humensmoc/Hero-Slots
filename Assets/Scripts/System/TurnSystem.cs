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
        yield return null;
    }
#endregion

#region Reaction

#endregion
}
