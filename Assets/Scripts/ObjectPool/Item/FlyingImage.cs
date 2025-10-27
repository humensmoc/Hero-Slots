using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class FlyingImage : ObjectPoolItem
{
    
    // 飞行到目标的参数
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float flyDuration;

    public Action onComplete;

    public void Update(){
        DoEffect();
    }

    public void Init(Vector3 startPos, Vector3 targetPos, Action onComplete){
        startPosition=startPos;
        targetPosition=targetPos;
        flyDuration=Vector3.Distance(startPosition,targetPosition)/VFXConfig.Instance.flySpeed;
        this.onComplete=onComplete;
    }

    public void DoEffect(){
        // 检查是否已经在飞行中
        if(!DOTween.IsTweening(transform)){
            // 设置起始位置
            transform.position = startPosition;
            
            // 开始飞行动画
            transform.DOMove(targetPosition, flyDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    // 飞行完成后淡出
                    
                    ObjectPool.Instance.ReturnObject(this,(()=>{
                        onComplete?.Invoke();   
                    }));
                });
        }
    }
}
