using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public enum FlyingTextType{
    AddElectricity,
    AddBloodGem,
    PowerUpBloodGem,
}

public class FlyingText : ObjectPoolItem
{
    public TMP_Text text;
    public SpriteRenderer spriteRenderer;

    private float currentHorizontalSpeed;
    private float fontSize;
    
    // 飞行到目标的参数
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float flyDuration;

    public Action onComplete;

    public void Update(){
        DoEffect();
    }

    public void Init(string text, FlyingTextType type, Vector3 startPos, Vector3 targetPos, Action onComplete){
        fontSize=this.text.fontSize;
        this.text.text = text;
        startPosition=startPos;
        targetPosition=targetPos;
        flyDuration=Vector3.Distance(startPosition,targetPosition)/VFXConfig.Instance.flySpeed;

        // 重置文本透明度
        Color color = this.text.color;
        color.a = 1f;
        this.text.color = color;

        switch(type){
            case FlyingTextType.AddElectricity:
                this.text.enabled=false;
                this.spriteRenderer.enabled=true;
                this.spriteRenderer.sprite=ResourcesLoader.LoadEffectSprite("AddElectricity");
                // 初始化运动参数
                currentHorizontalSpeed = UnityEngine.Random.Range(VFXConfig.Instance.horizontalSpeedMin, VFXConfig.Instance.horizontalSpeedMax);
                break;
            case FlyingTextType.AddBloodGem:
                this.text.enabled=false;
                this.spriteRenderer.enabled=true;
                this.spriteRenderer.sprite=ResourcesLoader.LoadEffectSprite("AddBloodGem");
                // 初始化运动参数
                currentHorizontalSpeed = UnityEngine.Random.Range(VFXConfig.Instance.horizontalSpeedMin, VFXConfig.Instance.horizontalSpeedMax);
                break;
            case FlyingTextType.PowerUpBloodGem:
                this.text.enabled=false;
                this.spriteRenderer.enabled=true;
                this.spriteRenderer.sprite=ResourcesLoader.LoadEffectSprite("PowerUpBloodGem");
                // 初始化运动参数
                currentHorizontalSpeed = UnityEngine.Random.Range(VFXConfig.Instance.horizontalSpeedMin, VFXConfig.Instance.horizontalSpeedMax);
                break;
        }

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
                        this.text.fontSize = fontSize;
                        onComplete?.Invoke();   
                    }));
                });
        }
    }
    
    public void ResetFontSize(){
        // 重置字体大小到原始值
        this.text.fontSize = fontSize;
    }
}
