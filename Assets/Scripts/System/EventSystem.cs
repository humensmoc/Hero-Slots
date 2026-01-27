using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventInfo{
    public CardView cardView;
    public EventType eventType;
    public EnemyView enemyView;
    public BulletView bulletView;
    public HeroView heroView;
    public EventInfo(CardView cardView,EventType eventType,EnemyView enemyView=null,BulletView bulletView=null,HeroView heroView=null){
        this.cardView = cardView;
        this.eventType = eventType;
        this.enemyView = enemyView;
        this.bulletView=bulletView;
        this.heroView=heroView;
    }
}

public enum EventType{
    OnCardAttack,
    OnHeroAttack,
    OnMartialAttackHitEnemy,
    OnBulletHitEnemy
}

public class EventSystem : Singleton<EventSystem>
{
    public Action OnGameStart;
    public Action OnGameLose;
    public Action OnGameWin;

    /// <summary>
    /// 事件函数列表
    /// </summary>
    Dictionary<EventType,List<Func<EventInfo, IEnumerator>>> eventFunctions=new Dictionary<EventType,List<Func<EventInfo, IEnumerator>>>();
    
    /// <summary>
    /// 添加事件函数
    /// </summary>
    /// <param name="actionFunction">事件函数</param>
    /// <param name="eventType">事件类型</param>
    public void AddAction(Func<EventInfo, IEnumerator> actionFunction,EventType eventType){
        if(!eventFunctions.ContainsKey(eventType)){
            eventFunctions[eventType]=new List<Func<EventInfo, IEnumerator>>();
        }
        eventFunctions[eventType].Add(actionFunction);
    }

    /// <summary>
    /// 移除事件函数
    /// </summary>
    /// <param name="actionFunction">事件函数</param>
    /// <param name="eventType">事件类型</param>
    public void RemoveAction(Func<EventInfo, IEnumerator> actionFunction,EventType eventType){
        eventFunctions[eventType].Remove(actionFunction);
    }
    
    /// <summary>
    /// 清除所有事件函数
    /// </summary>
    public void ClearActions(){
        eventFunctions.Clear();
    }

    /// <summary>
    /// 检查事件
    /// </summary>
    /// <param name="eventInfo">事件信息</param>
    /// <returns>协程</returns>
    public IEnumerator CheckEvent(EventInfo eventInfo){
        if(eventFunctions.ContainsKey(eventInfo.eventType)){
            foreach(var actionFunction in eventFunctions[eventInfo.eventType]){
                yield return actionFunction(eventInfo);
            }
        }
    }
    
}
