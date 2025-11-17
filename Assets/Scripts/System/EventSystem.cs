using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventInfo{
    public CardView cardView;
    public EventType eventType;
    public EventInfo(CardView cardView,EventType eventType){
        this.cardView = cardView;
        this.eventType = eventType;
    }
}

public enum EventType{
    CardAttack,
    HeroAttack,
}

public class EventSystem : Singleton<EventSystem>
{
    public Action OnGameStart;
    public Action OnGameOver;
    List<Func<EventInfo, IEnumerator>> CardAttack_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    List<Func<EventInfo, IEnumerator>> HeroAttack_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    
    public void AddAction(Func<EventInfo, IEnumerator> actionFunction,EventType eventType){
        switch(eventType){
            case EventType.CardAttack:
                CardAttack_ActionFunctions.Add(actionFunction);
                break;
            case EventType.HeroAttack:
                HeroAttack_ActionFunctions.Add(actionFunction);
                break;
        }
    }
    
    public void RemoveAction(Func<EventInfo, IEnumerator> actionFunction,EventType eventType){
        switch(eventType){
            case EventType.CardAttack:
                CardAttack_ActionFunctions.Remove(actionFunction);
                break;
            case EventType.HeroAttack:
                HeroAttack_ActionFunctions.Remove(actionFunction);
                break;
        }
    }
    
    public void ClearActions(){
        CardAttack_ActionFunctions.Clear();
        HeroAttack_ActionFunctions.Clear();
    }

    public IEnumerator CheckEvent(EventInfo eventInfo){
        switch(eventInfo.eventType){

            case EventType.CardAttack:

                if(CardAttack_ActionFunctions.Count>0){
                    foreach(var actionFunction in CardAttack_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;

            case EventType.HeroAttack:
            
                if(HeroAttack_ActionFunctions.Count>0){
                    foreach(var actionFunction in HeroAttack_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;
                
        }
    }
    
}
