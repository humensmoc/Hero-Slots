using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : Singleton<TurnSystem>
{
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
        yield return null;
    }
#endregion

#region Reaction

#endregion
}
