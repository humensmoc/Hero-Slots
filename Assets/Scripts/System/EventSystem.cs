using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventInfo{
    public CardView cardView;
    public EventType eventType;
    public EnemyView enemyView;
    public BulletView bulletView;
    public EventInfo(CardView cardView,EventType eventType,EnemyView enemyView=null,BulletView bulletView=null){
        this.cardView = cardView;
        this.eventType = eventType;
        this.enemyView = enemyView;
        this.bulletView=bulletView;
    }
}

public enum EventType{
    OnCardAttack,
    OnHeroAttack,
    OnMartialAttackHitEnemy,
    OnDartShot,
    OnBulletHitEnemy
}

public class EventSystem : Singleton<EventSystem>
{
    public Action OnGameStart;
    public Action OnGameLose;
    public Action OnGameWin;
    List<Func<EventInfo, IEnumerator>> OnCardAttack_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    List<Func<EventInfo, IEnumerator>> OnHeroAttack_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    List<Func<EventInfo, IEnumerator>> OnMartialAttackHitEnemy_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    List<Func<EventInfo, IEnumerator>> OnDartShot_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    List<Func<EventInfo,IEnumerator>> OnBulletHitEnemy_ActionFunctions=new List<Func<EventInfo, IEnumerator>>();
    
    public void AddAction(Func<EventInfo, IEnumerator> actionFunction,EventType eventType){
        switch(eventType){
            case EventType.OnCardAttack:
                OnCardAttack_ActionFunctions.Add(actionFunction);
                break;
            case EventType.OnHeroAttack:
                OnHeroAttack_ActionFunctions.Add(actionFunction);
                break;
            case EventType.OnMartialAttackHitEnemy:
                OnMartialAttackHitEnemy_ActionFunctions.Add(actionFunction);
                break;
            case EventType.OnDartShot:
                OnDartShot_ActionFunctions.Add(actionFunction);
                break;
            case EventType.OnBulletHitEnemy:
                OnBulletHitEnemy_ActionFunctions.Add(actionFunction);
                break;
        }
    }
    
    public void RemoveAction(Func<EventInfo, IEnumerator> actionFunction,EventType eventType){
        switch(eventType){
            case EventType.OnCardAttack:
                OnCardAttack_ActionFunctions.Remove(actionFunction);
                break;
            case EventType.OnHeroAttack:
                OnHeroAttack_ActionFunctions.Remove(actionFunction);
                break;
            case EventType.OnMartialAttackHitEnemy:
                OnMartialAttackHitEnemy_ActionFunctions.Remove(actionFunction);
                break;
            case EventType.OnDartShot:
                OnDartShot_ActionFunctions.Remove(actionFunction);
                break;
            case EventType.OnBulletHitEnemy:
                OnBulletHitEnemy_ActionFunctions.Remove(actionFunction);
                break;
        }
    }
    
    public void ClearActions(){
        OnCardAttack_ActionFunctions.Clear();
        OnHeroAttack_ActionFunctions.Clear();
        OnMartialAttackHitEnemy_ActionFunctions.Clear();
        OnDartShot_ActionFunctions.Clear();
        OnBulletHitEnemy_ActionFunctions.Clear();
    }

    public IEnumerator CheckEvent(EventInfo eventInfo){
        switch(eventInfo.eventType){

            case EventType.OnCardAttack:

                if(OnCardAttack_ActionFunctions.Count>0){
                    foreach(var actionFunction in OnCardAttack_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;

            case EventType.OnHeroAttack:
            
                if(OnHeroAttack_ActionFunctions.Count>0){
                    foreach(var actionFunction in OnHeroAttack_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;
                
            case EventType.OnMartialAttackHitEnemy:

                if(OnMartialAttackHitEnemy_ActionFunctions.Count>0){
                    foreach(var actionFunction in OnMartialAttackHitEnemy_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;

            case EventType.OnDartShot:

                if(OnDartShot_ActionFunctions.Count>0){
                    foreach(var actionFunction in OnDartShot_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;
            
            case EventType.OnBulletHitEnemy:

                if(OnBulletHitEnemy_ActionFunctions.Count>0){
                    foreach(var actionFunction in OnBulletHitEnemy_ActionFunctions){
                        yield return actionFunction(eventInfo); // 传递eventInfo参数
                    }
                }
                break;
        }
    }
    
}
