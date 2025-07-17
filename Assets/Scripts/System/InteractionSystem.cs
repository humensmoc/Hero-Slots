using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : Singleton<InteractionSystem>
{
    [SerializeField]private LayerMask targetLayerMask;
    public bool PlayerIsDragging{get ; set ;}=false;
    
    public bool PlayerCanInteract(){
        if(!ActionSystem.Instance.IsPerforming)
        {
            return true;

        }else
        {
            return false;
        }
    }

    public bool PlayerCanHover(){
        if(PlayerIsDragging)return false;
        return true;
    }

    public HeroSlotView EndTargeting( Vector3 endPosition){
        if(Physics.Raycast(endPosition,Vector3.forward,out RaycastHit hit,10,targetLayerMask)
            &&hit.collider!=null&&hit.collider.TryGetComponent(out HeroSlotView heroSlotView)){
            return heroSlotView;
        }
        return null;
    }
}
